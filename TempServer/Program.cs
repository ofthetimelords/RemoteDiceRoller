using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared;
using TheQ.DiceRoller.Shared;


namespace TheQ.DiceRoller.TempServer
{
    public partial class Program
    {
        private ConcurrentDictionary<TcpClient, ClientState> Connections = new ConcurrentDictionary<TcpClient, ClientState>();
        private Func<int, int> Generator;

        public static async Task Main(string[] args)
        {
            var program = new Program();
            await program.Run();
        }

        private async Task Run()
        {
            this.DefineRandomGenerator();
            if (this.Generator == null)
                this.Generator = dice => this.Rnd.Next(1, dice + 1);

            this.InputSocket = new TcpListener(IPAddress.Any, 5001);
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

            using (input)
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
                                await input.SendMessage("#AUTH", CancellationToken.None);
                                var response = await input.WaitForReply(CancellationToken.None);

                                Console.WriteLine($"Client responded with {response}");
                                if (response == "100101")
                                    state.CurrentState = State.MustIdentify;
                                else
                                    await input.SendMessage("Invalid ID", CancellationToken.None);
                                break;
                            }
                            case State.MustIdentify:
                            {
                                Console.WriteLine("Client must declare an ID");
                                await input.SendMessage("#ID", CancellationToken.None);
                                var response = await input.WaitForReply(CancellationToken.None);

                                Console.WriteLine($"Client responded with {response}");
                                state.CurrentState = State.Ready;
                                state.Id = response;
                                break;
                            }
                            case State.Ready:
                            {
                                var response = await input.WaitForReply(CancellationToken.None);

                                if (response.StartsWith("#ROLL"))
                                {
                                    var message = JsonConvert.DeserializeObject<RollMessage>(response.Substring("#ROLL".Length));
                                    var res = new ResultMessage
                                    {
                                        Dice = this.RollDice(message.AmountOfDice, message.Dice),
                                        From = state.Id
                                    };

                                    await Task.WhenAll(this.Connections.Where(c => c.Value.CurrentState == State.Ready).Select(conn => conn.Key.SendMessage("#RESULT" + JsonConvert.SerializeObject(res), CancellationToken.None)));
                                }
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unhandled exception: {ex.Message} by {input.Client.RemoteEndPoint} - {state.Id}");
                    input.Close();
                    this.Connections.TryRemove(input, out var value);
                }
            }
        }

        private Random Rnd = new Random();

        private IList<int> RollDice(int amount, DiceType dice) => Enumerable.Range(0, amount).Select(_ => this.Generator((int) dice)).ToList();

        partial void DefineRandomGenerator();
    }
}