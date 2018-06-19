using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Shared;
using TheQ.DiceRoller.Client;
using TheQ.DiceRoller.Shared;


namespace TheQ.RemoteDiceRoller
{
    public partial class FormMain : Form
    {
        private TcpClient OutgoingSocket { get; set; }
        private TcpClient IncomingSocket { get; set; }
        private ClientState CurrentState { get; set; }
        private CancellationTokenSource DefaultToken { get; set; } = new CancellationTokenSource();

        private SoundPlayer[] DiceSound { get; } = new[]
        {
            new SoundPlayer(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Media\\1.wav")),
            new SoundPlayer(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Media\\2.wav")),
            new SoundPlayer(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Media\\3.wav")),
            new SoundPlayer(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Media\\4.wav")),
            new SoundPlayer(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Media\\5.wav")),
            new SoundPlayer(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Media\\6.wav")),
            new SoundPlayer(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Media\\Lots.wav"))
        };

        public FormMain()
        {
            this.InitializeComponent();
        }

        private async void Connect_Click(object sender, EventArgs e)
        {
            var url = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ServerUrl"]) ? "localhost" : ConfigurationManager.AppSettings["ServerUrl"];
            var port = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ServerPort"]) ? 5001 : int.Parse(ConfigurationManager.AppSettings["ServerPort"]);
            var authKey = ConfigurationManager.AppSettings["AuthKey"];

            this.DefaultToken.Cancel();
            this.Connect.Enabled = false;
            this.ChangeConnectionStatus(false);

            if (this.OutgoingSocket != null)
            {
                this.OutgoingSocket.Close();
                this.OutgoingSocket.Dispose();
                this.CurrentState = new ClientState();
            }

            this.OutgoingSocket = new TcpClient();

            try
            {
                await this.OutgoingSocket.ConnectAsync(url, port);
            }
            catch (Exception)
            {
                MessageBox.Show("Server didn't respond; connection failed", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

                    if (response.EndsWith("#AUTH#"))
                    {
                        this.AddToLog("Me: " + authKey);
                        await this.OutgoingSocket.SendMessage(authKey, this.DefaultToken.Token);
                    }
                    else if (response.EndsWith("#ID#"))
                    {
                        this.AddToLog("Me: " + this.TextName.Text);
                        await this.OutgoingSocket.SendMessage(this.TextName.Text, this.DefaultToken.Token);
                    }
                    else if (response.EndsWith("#CONNECTED#"))
                    {
                        this.ChangeConnectionStatus(true);
                    }
                    else if (response.EndsWith("#DISCONNECTED#"))
                    {
                        this.ChangeConnectionStatus(false);
                        return;
                    }
                    else if (response.StartsWith("#RESULT#"))
                    {
                        var message = JsonConvert.DeserializeObject<ResultMessage>(response.Substring("#RESULT#".Length));
                        var noofDice = (message.Dice.Count > 7 ? 7 : message.Dice.Count) - 1;
                        this.DiceSound[noofDice].Play();
                        this.ShowDice(message.Dice, message.DiceType);
                        this.LastRollSum.Text = message.Dice.Sum().ToString();
                        this.LastRollBy.Text = message.From;
                    }
                }
            }
            catch (Exception ex)
            {
                this.DefaultToken.Cancel();

                if (this.IsDisposed)
                    return;

                this.AddToLog($"Server: Unhandled exception: {ex.Message}");
                this.Dice.Enabled = false;
                this.ChangeConnectionStatus(false);
            }
        }


        private void ChangeConnectionStatus(bool connected)
        {
            if (!connected)
            {
                this.ConnStatus.ForeColor = Color.Red;
                this.ConnStatus.Text = "Disconnected";
            }
            else
            {
                this.ConnStatus.ForeColor = Color.Lime;
                this.ConnStatus.Text = "Connected";
            }
        }

        private void SaveSettings()
        {
            ConfigurationManager.AppSettings["Name"] = this.TextName.Text;
            //ConfigurationManager.AppSettings["Room"] = this.TextRoom.Text;
        }

        private void ShowDice(IList<int> results, DiceType dice)
        {
            this.DiceResults.Controls.Clear();
            foreach (var die in results)
            {
                var face = new DiceFace();
                face.Height = face.Width = this.DiceResults.Height > 120 ? 120 : this.DiceResults.Height - 10;
                face.SetNumber(dice, die);
                this.DiceResults.Controls.Add(face);
            }
        }

        private void AddToLog(string response)
        {
            this.SysLog.AppendText(response + Environment.NewLine);
        }

        private async void RollD4_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL#" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int) this.D4Amount.Value,
                Dice = DiceType.D4
            }), this.DefaultToken.Token);
        }

        private async void RollD20_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL#" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int) this.D20Amount.Value,
                Dice = DiceType.D20
            }), this.DefaultToken.Token);
        }

        private async void RollD12_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL#" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int) this.D12Amount.Value,
                Dice = DiceType.D12
            }), this.DefaultToken.Token);
        }

        private async void RollD100_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL#" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int) this.D100Amount.Value,
                Dice = DiceType.D100
            }), this.DefaultToken.Token);
        }

        private async void RollD6_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL#" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int) this.D6Amount.Value,
                Dice = DiceType.D6
            }), this.DefaultToken.Token);
        }

        private async void RollD8_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL#" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int) this.D8Amount.Value,
                Dice = DiceType.D8
            }), this.DefaultToken.Token);
        }

        private async void RollD10_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL#" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int) this.D10Amount.Value,
                Dice = DiceType.D10
            }), this.DefaultToken.Token);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.OutgoingSocket.Close();
            Application.DoEvents();
        }
    }
}