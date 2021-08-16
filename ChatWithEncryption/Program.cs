using System;
using System.Net;
using System.Net.Sockets;

namespace ChatWithEncryption
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Chose role
                Console.WriteLine("-- WELCOME --\n-- Chose what you want to be:\n1. Server\n2. Client");
                string input = Console.ReadLine();
                int option = int.Parse(input);

                switch (option)
                {
                    // Server role
                    case 1:
                        new ChatServer();
                        break;
                    // Client role
                    case 2:
                        new ChatClient();
                        break;
                    default:
                        Console.WriteLine("Chat could not be started!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong!" + ex.Message);
            }
        }
    }
}
