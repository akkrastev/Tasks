using System;
using Task1App.Services;

namespace Task1App
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Commands: ");
                Console.WriteLine("add [ClientName] [MethodName] [SessionCount]");
                Console.WriteLine("list");
                Console.WriteLine("start [ClientName]");
                Console.WriteLine("stop [ClientName]");
                Console.WriteLine("list active");
                Console.WriteLine("history");
                Console.WriteLine("Enter a command: ");
                var command = Console.ReadLine();

                string[] commandParams = command.Split(" ");


                switch (commandParams[0])
                {
                    case "add":
                        CommandActions.AddNewClient(commandParams[1], commandParams[2], commandParams[3]);
                        break;
                    case "list":
                        if (commandParams.Length > 1)
                        {
                            CommandActions.GetActiveClientsInfo();
                        }
                        else
                        {
                            CommandActions.GetClientsInfo();
                        }                       
                        break;
                    case "start":
                        CommandActions.StartSession(commandParams[1]);
                        break;
                    case "stop":
                        CommandActions.StopSession(commandParams[1]);
                        break;
                    case "history":
                        CommandActions.ShowHistory();
                        break;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
