using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TwitchBot
{
    class IrcClient
    {
        private string _userName;
        private string _channel;

        private TcpClient _tcpClient;
        private StreamReader _inputStream;
        private StreamWriter _outputStream;

        public IrcClient(string ip, int port, string userName, string password, string channel)
        {
            this._userName = userName;
            this._channel = channel;

            _tcpClient = new TcpClient(ip, port);
            _inputStream = new StreamReader(_tcpClient.GetStream());
            _outputStream = new StreamWriter(_tcpClient.GetStream());

            _outputStream.WriteLine($"PASS {password}");
            _outputStream.WriteLine($"NICK {userName}");
            _outputStream.WriteLine($"USER {userName} 8 * :{userName}");
            _outputStream.WriteLine($"JOIN #{channel}");
            _outputStream.Flush();
        }

        public void SendIrcMessage(string message)
        {
            _outputStream.WriteLine(message);
            _outputStream.Flush();
        }

        public string ReadMessage()
        {
            return _inputStream.ReadLine();
        }

        public void SendChatMessage(string message)
        { 
            SendIrcMessage($":{_userName}!{_userName}@{_userName}.tmi.twitch.tv PRIVMSG #{_channel} :{message}");
        }
    }
}
