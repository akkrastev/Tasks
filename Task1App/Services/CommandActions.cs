using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Task1App.Models;

namespace Task1App.Services
{
    public class CommandActions 
    {
        public static void AddNewClient(string clientName, string methodName, string sessionCount)
        {
            var path = "C:\\Users\\Ivaylo\\source\\repos\\Task1App\\Data\\methods.json";
            List<Method> methods;

            try
            {
                methods = ReadFromJson<Method>(path);
                var names = methods.Select(m => m.Name).ToArray();

                if (!names.Contains(methodName))
                {
                    throw new Exception("The client will not be stored");
                }

                var tortureMethod = methods.FirstOrDefault(m => m.Name == methodName);

                var newClient = new Client();
                newClient.Name = clientName;
                newClient.TortureDescription = tortureMethod.Description;
                newClient.SessionCount = Int32.Parse(sessionCount);

                var clientJsonPath = "C:\\Users\\Ivaylo\\source\\repos\\Task1App\\Data\\clients.json";
                WriteToJson(clientJsonPath, newClient);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public static void GetClientsInfo()
        {
            try
            {
                var clientJsonPath = "C:\\Users\\Ivaylo\\source\\repos\\Task1App\\Data\\clients.json";
                var clients = ReadFromJson<Client>(clientJsonPath);

                foreach (var client in clients)
                {
                    var resultString = $"{client.Name} is to be {client.TortureDescription} : {client.SessionCount} sessions remaining";
                    Console.WriteLine(resultString);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void StartSession(string clientName)
        {
            try
            {
                var clientJsonPath = "C:\\Users\\Ivaylo\\source\\repos\\Task1App\\Data\\clients.json";
                var clients = ReadFromJson<Client>(clientJsonPath);
                var clientNames = clients.Select(c => c.Name).ToArray();

                if (!clientNames.Contains(clientName))
                {
                    throw new Exception("The Client doest't exist!");
                }

                var client = clients.FirstOrDefault(c => c.Name == clientName);        
                clients.Remove(client);

                if (clients.Select(c => c.TortureDescription).Contains(client.TortureDescription))
                {
                    throw new Exception("Sadly, the equipment required for this session is already in use.");
                }

                client.SessionStatus = SessionStatus.Active;
                client.StartTime = DateTime.UtcNow;
                clients.Add(client);
                File.WriteAllText(clientJsonPath, JsonConvert.SerializeObject(clients));

                Console.WriteLine($"{clientName}'s session has started");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void StopSession(string clientName)
        {
            try
            {
                var clientJsonPath = "C:\\Users\\Ivaylo\\source\\repos\\Task1App\\Data\\clients.json";
                var clients = ReadFromJson<Client>(clientJsonPath);
                var clientNames = clients.Select(c => c.Name).ToArray();

                if (!clientNames.Contains(clientName))
                {
                    throw new Exception("The Client doest't exist!");
                }

                var client = clients.FirstOrDefault(c => c.Name == clientName);
                clients.Remove(client);
                client.SessionStatus = SessionStatus.Finished;
                client.SessionCount--;
                var endTime = DateTime.UtcNow;
                client.Duration = (TimeSpan)(endTime - client.StartTime);
                clients.Add(client);
                File.WriteAllText(clientJsonPath, JsonConvert.SerializeObject(clients));

                Console.WriteLine($"{clientName}'s session has finished");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void GetActiveClientsInfo()
        {
            try
            {
                var clientJsonPath = "C:\\Users\\Ivaylo\\source\\repos\\Task1App\\Data\\clients.json";
                var clients = ReadFromJson<Client>(clientJsonPath);
                var activeClients = clients.Where(c => c.SessionStatus == SessionStatus.Active);

                foreach (var client in activeClients)
                {
                    var resultString = $"{client.Name} is being {client.TortureDescription}";
                    Console.WriteLine(resultString);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void ShowHistory()
        {
            try
            {
                var clientJsonPath = "C:\\Users\\Ivaylo\\source\\repos\\Task1App\\Data\\clients.json";
                var clients = ReadFromJson<Client>(clientJsonPath);
                var finishedClients = clients.Where(c => c.SessionStatus == SessionStatus.Finished);

                foreach(var client in finishedClients)
                {
                    var resultString = $"{client.Name} was {client.TortureDescription} {client.StartTime} {client.Duration }";
                    Console.WriteLine(resultString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static List<T> ReadFromJson<T>(string path)
        {
            var result = new List<T>();

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string json = reader.ReadToEnd();
                    result = JsonConvert.DeserializeObject<List<T>>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }         

            return result;
        }

        private static void WriteToJson(string path, Client client)
        {
            try
            {
                var clients = ReadFromJson<Client>(path);
                clients.Add(client);
                File.WriteAllText(path, JsonConvert.SerializeObject(clients));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }     
        }
    }
}
