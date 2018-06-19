using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace TheQ.DiceRoller.Shared
{
    public static class TcpClientHelper
    {
        public static async Task<bool> SendMessage(this TcpClient client, string message, CancellationToken token)
        {
            if (!client.Connected)
                return false;

            var stream = client.GetStream();
            var bytes = Encoding.UTF8.GetBytes(message + (char)4);
            await stream.WriteAsync(bytes, 0, bytes.Length, token);
            return true;
        }

        public static async Task<string> WaitForReply(this TcpClient client, CancellationToken token)
        {
            if (!client.Connected)
                return null;

            var message = new byte[2048];
            var buffer = new byte[2048];
            var position = 0;

            while (true)
            {
                using (var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(2000)))
                {
                    var combinedToken = CancellationTokenSource.CreateLinkedTokenSource(token, tokenSource.Token);

                    if (!client.Connected)
                        throw new InvalidOperationException("Connection dropped");

                    token.ThrowIfCancellationRequested();

                    var segment = new ArraySegment<byte>(buffer);
                    var read = await client.Client.ReceiveAsync(segment, SocketFlags.None);

                    if (read > 0)
                    {
                        int oldPos = position;
                        position += read - 1;

                        if (position > message.Length)
                            Array.Resize(ref message, position + 1);

                        Array.Copy(buffer, 0, message, oldPos, read);

                        if (message[position] == 4)
                            return Encoding.UTF8.GetString(message, 0, position);
                    }
                    else
                        await Task.Delay(500, combinedToken.Token);
                }
            }
        }
    }
}