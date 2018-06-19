using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared;
using TheQ.DiceRoller.Shared;


namespace TheQ.DiceRoller.TempServer
{
    public partial class Program
    {
        private string AuthKey;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<TcpClient, ClientState>> Connections = new ConcurrentDictionary<string, ConcurrentDictionary<TcpClient, ClientState>>();
        private Func<int, int> Generator;

        private readonly Random Rnd = new Random();

        private TcpListener InputSocket { get; set; }

        public static async Task Main(string[] args)
        {
            var program = new Program();
            await program.Run();
        }


        public async void ProcessCommunicationAsync(TcpClient input)
        {
            input.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            input.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            input.NoDelay = true;

            var state = new ClientState
            {
                CurrentState = State.MustAuthenticate
            };
            Console.WriteLine($"Client connected with IP: {input.Client.RemoteEndPoint}");
            await input.SendMessage("Welcome to the Dice Server" + Environment.NewLine, CancellationToken.None);

            using (input)
            using (input.GetStream())
            {
                try
                {
                    Console.WriteLine("Requesting that client authenticates");
                    await input.SendMessage("#AUTH#", CancellationToken.None);
                    state.CurrentState = State.MustAuthenticateSent;
                    var responses = new List<string>();

                    while (true)
                    {
                        foreach (var resp in await input.WaitForReplies(CancellationToken.None))
                            responses.Add(resp);

                        foreach (var response in responses)
                        {
                            switch (state.CurrentState)
                            {
                                case State.MustAuthenticateSent:
                                {
                                    Console.WriteLine($"Client responded with {response}");
                                    if (response.StartsWith("#AUTHRESPONSE#") && response.Substring("#AUTHRESPONSE#".Length) == this.AuthKey)
                                    {
                                        state.CurrentState = State.MustIdentify;
                                        goto case State.MustIdentify;
                                    }
                                    else
                                    {
                                        await input.SendMessage("#AUTHFAILED#", CancellationToken.None);
                                        await this.DisconnectClient(input, state.Room);
                                        return;
                                    }
                                }
                                case State.MustIdentify:
                                {
                                    Console.WriteLine("Client must declare an ID");
                                    await input.SendMessage("#ID#", CancellationToken.None);
                                    state.CurrentState = State.MustIdentifySent;

                                    break;
                                }
                                case State.MustIdentifySent:
                                {
                                    Console.WriteLine($"Client responded with {response}");
                                    if (response.StartsWith("#IDRESPONSE#"))
                                    {
                                        var parsed = JsonConvert.DeserializeObject<ClientState>(response.Substring("#IDRESPONSE#".Length));

                                        state.Id = parsed.Id;
                                        state.Room = parsed.Room;
                                        state.CurrentState = State.Ready;

                                        var room = this.GetARoom(state.Room);
                                        room.GetOrAdd(input, i => state);


                                        await input.SendMessage("#CONNECTED#", CancellationToken.None);
                                        await Task.WhenAll(room.Where(c => c.Value.CurrentState == State.Ready && c.Key.Connected && c.Key != input)
                                            .Select(conn => conn.Key.SendMessage("#CONNECTEDCLIENT#" + state.Id, CancellationToken.None)));
                                    }

                                    goto case State.Ready;
                                }
                                case State.Ready:
                                {
                                    Console.WriteLine($"Client {state.Id} sent a command: {response}");

                                    if (response.StartsWith("#ROLL#"))
                                    {
                                        var message = JsonConvert.DeserializeObject<RollMessage>(response.Substring("#ROLL#".Length));
                                        Console.WriteLine($"Client {state.Id} requested to roll {message.AmountOfDice} {message.Dice}");

                                        var res = new ResultMessage
                                        {
                                            Dice = this.RollDice(message.AmountOfDice, message.Dice),
                                            DiceType = message.Dice,
                                            From = state.Id,
                                            Room = state.Room
                                        };

                                        var serRes = JsonConvert.SerializeObject(res);
                                        Console.WriteLine($"Sending {serRes} to all clients");
                                        await Task.WhenAll(this.GetARoom(state.Room).Where(c => c.Value.CurrentState == State.Ready && c.Key.Connected).Select(conn => conn.Key.SendMessage("#RESULT#" + serRes, CancellationToken.None)));

                                        // Discard any disconnected clients, just in case.
                                        foreach (var disconnected in this.GetARoom(state.Room).Where(c => !c.Key.Connected).ToList())
                                            await this.DisconnectClient(disconnected.Key, state.Room);
                                    }

                                    break;
                                }
                            }
                        }

                        responses.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unhandled exception: {ex.Message} by {(input.Connected ? input.Client?.RemoteEndPoint.ToString() : "?")} - {state.Id}. Disconnecting this client!");
                    await this.DisconnectClient(input, state.Room);
                }
            }
        }

        private ConcurrentDictionary<TcpClient, ClientState> GetARoom(string room)
        {
            return this.Connections.GetOrAdd(room, dic => new ConcurrentDictionary<TcpClient, ClientState>());
        }

        partial void DefineRandomGenerator();

        private async Task DisconnectClient(TcpClient input, string roomName)
        {
            if (input.Connected)
                input.Close();

            var room = this.GetARoom(roomName);
            room.TryRemove(input, out var value);
            var tasks = room.Where(c => c.Value.CurrentState == State.Ready && c.Key.Connected && c.Key != input).Select(async conn =>
            {
                try
                {
                     await conn.Key.SendMessage("#DISCONNECTEDCLIENT#" + conn.Value.Id, CancellationToken.None);
                }
                catch (Exception ex)
                {

                }
            });

            await Task.WhenAll(tasks);
        }

        private IList<int> RollDice(int amount, DiceType dice) => Enumerable.Range(0, amount).Select(_ => this.Generator((int) dice)).ToList();

        private async Task Run()
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            this.AuthKey = config["AuthKey"];
            var port = int.Parse(config["ListenPort"]);

            this.DefineRandomGenerator();
            if (this.Generator == null)
                this.Generator = dice => this.Rnd.Next(1, dice + 1);

            this.InputSocket = new TcpListener(IPAddress.Any, port);
            this.InputSocket.AllowNatTraversal(true);
            this.InputSocket.Start(10);

            while (true)
            {
                var inputClient = await this.InputSocket.AcceptTcpClientAsync();
                this.ProcessCommunicationAsync(inputClient);
            }
        }
    }
}