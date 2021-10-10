using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchBot
{
    class ChatCommands
    {
        // To add new command:
        // 1. Add command to the dictionary (!commandName, message content)
        // - for example: commands.Add("!help", "Available commands: <!help!>");
        // 2. Add <!aliasName!> to array called functions
        // 3. Create new method for the command
        // 4. Add delegate with specific alias to dictionary
        // - for example: functionsOneArg.Add("<!help!>", new Func<string, string>(HelpCommand));

        Dictionary<string, Delegate> functionsOneArg;

        Dictionary<string, string> commands;

        string[] functions = { "<!help!>", "<!dice!>", "<!percent!>", "<!random!>" };

        public void SetupCommands()
        {
            functionsOneArg = new Dictionary<string, Delegate>();

            commands = new Dictionary<string, string>();

            commands.Add("!help", "Available commands: <!help!>");
            commands.Add("!dice", "You rolled: <!dice!>");
            commands.Add("!percentExample", "Your luck today: <!percent!>");
            commands.Add("!randomExample", "Your random number: <!random!><!-4_21!>");

            functionsOneArg.Add("<!help!>", new Func<string, string>(HelpCommand));
            functionsOneArg.Add("<!dice!>", new Func<string, string>(DiceCommand));
            functionsOneArg.Add("<!percent!>", new Func<string, string>(PercentCommand));
            functionsOneArg.Add("<!random!>", new Func<string, string>(RandomNumberCommand));
        }

        public string GetChatCommend(string message)
        {
            return message.Substring(message.IndexOf(":", 1) + 1);
        }

        public string ProcessChatCommand(string message)
        {
            foreach (var command in commands)
            {
                if (command.Key == message)
                {
                    string commandMessage = command.Value;
                    for (int i = 0; i < functions.Length; i++)
                    {
                        if (commandMessage.Contains(functions[i]))
                        {
                            if (functionsOneArg.ContainsKey(functions[i]))
                            {
                                commandMessage = functionsOneArg[functions[i]].DynamicInvoke(commandMessage).ToString();
                            }
                            i--;
                        }
                    }
                    Console.WriteLine(commandMessage);
                    return commandMessage;
                }

            }
            return "";
        }

        public string HelpCommand(string message)
        {
            string listOfCommands = "";
            foreach (var command in commands.Keys)
            {
                listOfCommands += command + " ";
            }
            message = ReplaceFirst(message, "<!help!>", listOfCommands);

            return message;
        }

        public string DiceCommand(string message)
        {
            Random random = new Random();
            message = ReplaceFirst(message, "<!dice!>", random.Next(1, 7).ToString());
            return message;
        }

        public string PercentCommand(string message)
        {
            Random random = new Random();
            return ReplaceFirst(message, "<!percent!>", random.Next(0, 101).ToString()) + "%";
        }

        public string RandomNumberCommand(string message)
        {
            string splitChar = "_";
            string command = "<!random!>";
            int startIndex = message.IndexOf(command) + command.Length;
            // check if right after the command there is "<!"
            int testIndex = message.IndexOf("<!", startIndex);
            if (message.IndexOf("<!", startIndex) == startIndex)
            {
                // look for "!>" closing parameters piece
                int endIndex = message.IndexOf("!>", startIndex) - 2;
                // interval contains parameters that should be splited with a splitChacter (variable)
                string interval = message.Substring(startIndex + 2, endIndex - startIndex);
                string[] arr = interval.Split(splitChar);

                int lowerBound, upperBound;
                // 
                if(    arr.Length != 2
                    || !int.TryParse(arr[0], out lowerBound)
                    || !int.TryParse(arr[1], out upperBound)
                    || upperBound < lowerBound)
                    return "";
                Random random = new Random();
                return ReplaceFirst(message,
                       command + "<!" + lowerBound.ToString() + splitChar + upperBound.ToString() + "!>",
                       random.Next(lowerBound, upperBound).ToString());
            }
            return "";
        }

        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}
