using System;
using System.Collections.Generic;

namespace TwitchBot
{
    class Program
    {
        static void Main(string[] args)
        {
            string botName = ""; // Twitch bot name
            string OAuthPass = ""; // OAuthPass to bot
            string channelName = ""; // Channel that your bot will connect to 

            IrcClient client = new IrcClient("irc.twitch.tv", 6667, botName, OAuthPass, channelName);

            // Ping twitch server so it does not disconnect bot
            var pinger = new Pinger(client);
            pinger.Start();

            // Twitch chat commands 
            ChatCommands commands = new ChatCommands();
            commands.SetupCommands();

            string message;

            while (true)
            {
                Console.WriteLine("Reading message");
                message = client.ReadMessage();
                if(message != null)
                {
                    Console.WriteLine($"{message}");
                    // Get the exact text that user entered to the chat witout irc commands
                    message = commands.GetChatCommend(message);

                    // If user's message starst with "!" check if command exists  
                    if(message.StartsWith("!"))
                    {
                        // Check for command
                        var botResponse = commands.ProcessChatCommand(message);

                        // If bot response is empty, command does not exist (might should use try catch, to consider)
                        if (botResponse != "")
                            client.SendChatMessage(botResponse);
                    }
                }
            }
        }
    }
}
