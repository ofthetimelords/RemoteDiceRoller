using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using Shared;
using TheQ.DiceRoller.Shared;


namespace TheQ.DiceRoller.TempServer
{
    public partial class Program
    {
        private ConcurrentDictionary<TcpClient, ClientState> Connections = new ConcurrentDictionary<TcpClient, ClientState>();
        private Func<int, int> Generator;
        private string AuthKey;

        public static async Task Main(string[] args)
        {
            var program = new Program();
            await program.Run();
        }

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

        private TcpListener InputSocket { get; set; }


        public async void ProcessCommunicationAsync(TcpClient input)
        {
            input.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            input.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            input.NoDelay = true;

            var state = this.Connections.GetOrAdd(input, i => new ClientState
            {
                CurrentState = State.MustAuthenticate
            });
            Console.WriteLine($"Client connected with IP: {input.Client.RemoteEndPoint}");
            await input.SendMessage("Welcome to the Dice Server" + Environment.NewLine, CancellationToken.None);

            using (input)
            using (input.GetStream())
            {
                try
                {
                    while (true)
                    {
                        switch (state.CurrentState)
                        {
                            case State.MustAuthenticate:
                            {
                                Console.WriteLine("Requesting that client authenticates");
                                await input.SendMessage("#AUTH#", CancellationToken.None);
                                var response = await input.WaitForReplies(CancellationToken.None);

                                Console.WriteLine($"Client responded with {response}");
                                if (response.Last() == this.AuthKey) // TODO: Ugly
                                    state.CurrentState = State.MustIdentify;
                                else
                                {
                                    await this.DisconnectClient(input);
                                    return;
                                }

                                break;
                            }
                            case State.MustIdentify:
                            {
                                Console.WriteLine("Client must declare an ID");
                                await input.SendMessage("#ID#", CancellationToken.None);
                                var response = await input.WaitForReplies(CancellationToken.None);

                                Console.WriteLine($"Client responded with {response}");
                                state.CurrentState = State.Ready;
                                state.Id = response.Last();

                                await input.SendMessage("#CONNECTED#", CancellationToken.None);
                                await Task.WhenAll(this.Connections.Where(c => c.Value.CurrentState == State.Ready && c.Key.Connected).Select(conn => conn.Key.SendMessage("#CONNECTEDCLIENT#" + state.Id, CancellationToken.None)));

                                break;
                            }
                            case State.Ready:
                            {
                                var response = (await input.WaitForReplies(CancellationToken.None)).Last();
                                Console.WriteLine($"Client {state.Id} sent a command: {response}");

                                if (response.StartsWith("#ROLL#"))
                                {
                                    var message = JsonConvert.DeserializeObject<RollMessage>(response.Substring("#ROLL#".Length));
                                    Console.WriteLine($"Client {state.Id} requested to roll {message.AmountOfDice} {message.Dice}");

                                    var res = new ResultMessage
                                    {
                                        Dice = this.RollDice(message.AmountOfDice, message.Dice),
                                        DiceType = message.Dice,
                                        From = state.Id
                                    };

                                    var serRes = JsonConvert.SerializeObject(res);
                                    Console.WriteLine($"Sending {serRes} to all clients");
                                    await Task.WhenAll(this.Connections.Where(c => c.Value.CurrentState == State.Ready && c.Key.Connected).Select(conn => conn.Key.SendMessage("#RESULT#" + serRes, CancellationToken.None)));

                                    // Discard any disconnected clients, just in case.
                                    foreach (var disconnected in this.Connections.Where(c => !c.Key.Connected).ToList())
                                    {
                                        try
                                        {
                                            await this.DisconnectClient(disconnected.Key);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                }

                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unhandled exception: {ex.Message} by {(input.Connected ? input.Client?.RemoteEndPoint.ToString() : "?")} - {state.Id}. Disconnecting this client!");
                    await this.DisconnectClient(input);
                }
            }
        }

        private async Task DisconnectClient(TcpClient input)
        {
            if (input.Connected)
                input.Close();

            this.Connections.TryRemove(input, out var value);
            try
            {
                var tasks = this.Connections.Where(c => c.Value.CurrentState == State.Ready && c.Key.Connected).Select(conn => conn.Key.SendMessage("#DISCONNECTEDCLIENT#" + conn.Value.Id, CancellationToken.None));

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {

            }
        }

        private Random Rnd = new Random();

        private IList<int> RollDice(int amount, DiceType dice) => Enumerable.Range(0, amount).Select(_ => this.Generator((int)dice)).ToList();

        partial void DefineRandomGenerator();
    }
}