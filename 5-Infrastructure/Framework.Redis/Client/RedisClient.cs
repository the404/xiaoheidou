using Framework.Common.LogOperation;
using Framework.Redis.BasicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Redis.Client
{
    public partial class RedisClient
    {
        private TcpClient _tcpClient = null;

        public RedisClient()
        {
            _tcpClient = new TcpClient();
        }

        public void Connect(string serverIP, int serverPort)
        {
            _tcpClient.Connect(serverIP, serverPort);
        }

        public string Send(string command)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }

            if (!command.EndsWith(RedisCommand.NEW_LINE))
            {
                command += RedisCommand.NEW_LINE;
            }

            _tcpClient.Client.Send(Encoding.UTF8.GetBytes(command));

            byte[] buffer = new byte[_tcpClient.ReceiveBufferSize];
            _tcpClient.Client.Receive(buffer);
            string value = Encoding.UTF8.GetString((byte[])buffer.ToArray()).Replace("\0", "").TrimEnd(RedisCommand.NEW_LINE.ToCharArray());
            return value;
        }
    }
}
