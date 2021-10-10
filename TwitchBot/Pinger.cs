using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TwitchBot
{
    class Pinger
    {
        IrcClient client;
        Thread sender;

        public Pinger(IrcClient client)
        {
            this.client = client;
            sender = new Thread(new ThreadStart(Run));
        }

        public void Start()
        {
            sender.IsBackground = true;
            sender.Start();
        }

        private void Run()
        {
            while(true)
            {
                Console.WriteLine("sending PING");
                client.SendIrcMessage("PING irc.twitch.tv");
                Thread.Sleep(TimeSpan.FromMinutes(5));
                Console.WriteLine("sent PING");
            }
        }
    }
}
