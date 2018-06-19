using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Sockets;
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
        public FormMain()
        {
            this.InitializeComponent();
        }

        private ClientState CurrentState { get; set; }
        private CancellationTokenSource DefaultToken { get; set; } = new CancellationTokenSource();

        private SoundPlayer[] DiceSound { get; } =
        {
            new SoundPlayer(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Media\\1.wav")),
            new SoundPlayer(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Media\\2.wav")),
            new SoundPlayer(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Media\\3.wav")),
            new SoundPlayer(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Media\\4.wav")),
            new SoundPlayer(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Media\\5.wav")),
            new SoundPlayer(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Media\\6.wav")),
            new SoundPlayer(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Media\\Lots.wav"))
        };

        private TcpClient OutgoingSocket { get; set; }

        private void AddToLog(string response)
        {
            this.SysLog.AppendText(Environment.NewLine + response);
        }

        private void ChangeConnectionStatus(bool connected)
        {
            if (!connected)
            {
                this.ConnStatus.ForeColor = Color.Red;
                this.ConnStatus.Text = "Disconnected";
                this.Connect.Enabled = true;
                this.Connect.Text = "Connect";
                this.Dice.Enabled = false;
            }
            else
            {
                this.Dice.Enabled = true;
                this.ConnStatus.ForeColor = Color.Lime;
                this.ConnStatus.Text = "Connected";
                this.Connect.Enabled = true;
                this.Connect.Text = "Reconnect";
            }
        }

        private async void Connect_Click(object sender, EventArgs e)
        {
            var url = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ServerUrl"]) ? "localhost" : ConfigurationManager.AppSettings["ServerUrl"];
            var port = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ServerPort"]) ? 5001 : int.Parse(ConfigurationManager.AppSettings["ServerPort"]);
            var authKey = ConfigurationManager.AppSettings["AuthKey"];
            TcpClient socket;

            if ((socket = await this.PrepareConnection(url, port)) == null)
            {
                MessageBox.Show("Server didn't respond; connection failed", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                this.OutgoingSocket = socket;
                var responses = new List<string>();

                while (true)
                {
                    if (!socket.Connected)
                    {
                        this.AddToLog("Server: Client got disconnected");
                        this.ChangeConnectionStatus(false);
                        this.Disconnect(socket);
                        return;
                    }

                    foreach (var resp in await socket.WaitForReplies(this.DefaultToken.Token))
                        responses.Add(resp);

                    foreach (var response in responses)
                    {
                        this.AddToLog("Server: " + response);

                        if (response.EndsWith("#AUTH#"))
                        {
                            this.AddToLog("Me: " + authKey);
                            await socket.SendMessage(authKey, this.DefaultToken.Token);
                        }
                        else if (response.EndsWith("#ID#"))
                        {
                            this.AddToLog("Me: " + this.TextName.Text);
                            await socket.SendMessage(this.TextName.Text, this.DefaultToken.Token);
                        }
                        else if (response.EndsWith("#CONNECTED#"))
                        {
                            this.ChangeConnectionStatus(true);
                        }
                        else if (response.EndsWith("#CONNECTEDCLIENT#"))
                        {
                        }
                        else if (response.EndsWith("#DISCONNECTED#"))
                        {
                            this.ChangeConnectionStatus(false);
                            return;
                        }
                        else if (response.EndsWith("#DISCONNECTEDCLIENT#"))
                        {
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

                    responses.Clear();
                }
            }
            catch (Exception ex)
            {
                this.Disconnect(socket);

                if (this.IsDisposed)
                    return;

                this.AddToLog($"Server: Unhandled exception: {ex.Message}");
                this.ChangeConnectionStatus(false);
            }
        }

        private void Disconnect(TcpClient socket)
        {
            if (!this.DefaultToken.IsCancellationRequested)
            {
                this.DefaultToken.Cancel();
                this.DefaultToken.Dispose();
            }

            if (socket != null)
            {
                socket.Close();
                socket.Dispose();
                this.CurrentState = new ClientState();
            }

            this.ChangeConnectionStatus(false);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.OutgoingSocket.Close();
            Application.DoEvents();
        }

        private async Task<TcpClient> PrepareConnection(string url, int port)
        {
            TcpClient socket;

            try
            {
                this.Connect.Enabled = false;
                this.Disconnect(this.OutgoingSocket);

                socket = new TcpClient();

                await socket.ConnectAsync(url, port);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                this.Connect.Enabled = true;
            }

            socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            socket.NoDelay = true;
            this.DefaultToken = new CancellationTokenSource();
            this.ChangeConnectionStatus(true);
            return socket;
        }

        private async void RollD10_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL#" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int)this.D10Amount.Value,
                Dice = DiceType.D10
            }), this.DefaultToken.Token);
        }

        private async void RollD100_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL#" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int)this.D100Amount.Value,
                Dice = DiceType.D100
            }), this.DefaultToken.Token);
        }

        private async void RollD12_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL#" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int)this.D12Amount.Value,
                Dice = DiceType.D12
            }), this.DefaultToken.Token);
        }

        private async void RollD20_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL#" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int)this.D20Amount.Value,
                Dice = DiceType.D20
            }), this.DefaultToken.Token);
        }

        private async void RollD4_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL#" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int)this.D4Amount.Value,
                Dice = DiceType.D4
            }), this.DefaultToken.Token);
        }

        private async void RollD6_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL#" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int)this.D6Amount.Value,
                Dice = DiceType.D6
            }), this.DefaultToken.Token);
        }

        private async void RollD8_Click(object sender, EventArgs e)
        {
            await this.OutgoingSocket.SendMessage("#ROLL#" + JsonConvert.SerializeObject(new RollMessage
            {
                AmountOfDice = (int)this.D8Amount.Value,
                Dice = DiceType.D8
            }), this.DefaultToken.Token);
        }

        private void ShowDice(IList<int> results, DiceType dice)
        {
            this.DiceResults.Controls.Clear();
            foreach (var die in results)
            {
                var face = new DiceFace();
                face.Height = face.Width = this.DiceResults.Height > 200 ? 200 : this.DiceResults.Height - 10;
                face.SetNumber(dice, die);
                this.DiceResults.Controls.Add(face);
            }
        }
    }
}