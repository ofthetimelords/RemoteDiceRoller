using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Shared;
using TheQ.DiceRoller.Shared;


namespace TheQ.RemoteDiceRoller
{
    public partial class FormMain : Form
    {
        private TcpClient OutgoingSocket { get; set; }
        private TcpClient IncomingSocket { get; set; }
        private ClientState CurrentState { get; set; }
        private CancellationTokenSource DefaultToken { get; set; } = new CancellationTokenSource();

        public FormMain()
        {
            this.InitializeComponent();
        }

        private async void Connect_Click(object sender, EventArgs e)
        {
            this.DefaultToken.Cancel();
            this.Connect.Enabled = false;

            if (this.OutgoingSocket != null)
            {
                this.OutgoingSocket.Close();
                this.OutgoingSocket.Dispose();
                this.CurrentState = new ClientState();
            }

            this.OutgoingSocket = new TcpClient();

            await this.OutgoingSocket.ConnectAsync("localhost", 5001);
            this.OutgoingSocket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            this.OutgoingSocket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            this.OutgoingSocket.NoDelay = true;
            this.Connect.Enabled = true;
            this.DefaultToken = new CancellationTokenSource();
            this.Dice.Enabled = true;

            try
            {
                while (true)
                {
                    if (!this.OutgoingSocket.Connected)
                    {
                        this.AddToLog("Server: Client got disconnected");
                        this.Dice.Enabled = false;
                        this.DefaultToken.Cancel();
                        return;
                    }

                    var response = await this.OutgoingSocket.WaitForReply(this.DefaultToken.Token);
                    this.AddToLog("Server: " + response);

                    if (response.EndsWith("#AUTH"))
                    {
                        await this.OutgoingSocket.SendMessage("100101", this.DefaultToken.Token);
                    }
                    else if (response.EndsWith("#ID"))
                    {
                        await this.OutgoingSocket.SendMessage("James", this.DefaultToken.Token);
                    }
                    else if (response.StartsWith("#RESULT"))
                    {
                        var message = JsonConvert.DeserializeObject<ResultMessage>(response.Substring("#RESULT".Length));

                        this.TempLabel.Text = response;
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddToLog($"Server: Unhandled exception: {ex.Message}");
                this.Dice.Enabled = false;
                this.DefaultToken.Cancel();
            }
        }

        private void AddToLog(string response)
        {
            this.SysLog.AppendText(response + Environment.NewLine);
        }

        private async void RollD4_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int) this.D4Amount.Value,
                Dice = DiceType.D4
            }), this.DefaultToken.Token);
        }

        private async void RollD20_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int) this.D20Amount.Value,
                Dice = DiceType.D20
            }), this.DefaultToken.Token);
        }

        private async void RollD12_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int) this.D12Amount.Value,
                Dice = DiceType.D12
            }), this.DefaultToken.Token);
        }

        private async void RollD100_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int) this.D100Amount.Value,
                Dice = DiceType.D100
            }), this.DefaultToken.Token);
        }

        private async void RollD6_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int) this.D6Amount.Value,
                Dice = DiceType.D6
            }), this.DefaultToken.Token);
        }

        private async void RollD8_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int) this.D8Amount.Value,
                Dice = DiceType.D8
            }), this.DefaultToken.Token);
        }

        private async void RollD10_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int) this.D10Amount.Value,
                Dice = DiceType.D10
            }), this.DefaultToken.Token);
        }
    }
}