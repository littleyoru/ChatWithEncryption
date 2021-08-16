using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ChatWithEncryption
{
    class ChatClient : TcpClient
    {
        public IPEndPoint RemoteEndPoint { get; set; }
        public string ClientName;
        public bool IsLoggedIn = false;


        public ChatClient() : base()
        {
            Console.WriteLine("Starting chat application/ Connecting to server..");

            // IP of server
            Console.WriteLine("IP: ");
            string inputIP = Console.ReadLine().Trim();
            IPAddress ip = IPAddress.Parse(inputIP);

            // port of server
            Console.WriteLine("Port: ");
            string inputPort = Console.ReadLine().Trim();
            int port = int.Parse(inputPort);

            RemoteEndPoint = new IPEndPoint(ip, port);

            // Username
            Console.WriteLine("Your username:");
            ClientName = Console.ReadLine().Trim();

        }

        public void RunClient()
        {
            byte[] buffer = new byte[256];

            Connect(RemoteEndPoint);
            Console.WriteLine("Connection with server established!");

            NetworkStream stream = GetStream();

            // Login with ClientName as username
            Login(stream);

            // wait for login ack from server
            while (!IsLoggedIn)
            {
                WaitForLoginAck(stream);
            }

            // receive message
            ChatActions.ListenForMessages(stream);

            // send message
            ChatActions.SendMessage(stream);
        }

        public void Login(NetworkStream stream)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(ClientName);
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not login to the chat: " + ex.Message);
            }

        }

        public async void WaitForLoginAck(NetworkStream stream)
        {
            try
            {
                byte[] buffer = new byte[256];
                int bytesRead = await stream.ReadAsync(buffer, 0, 256);
                string ackMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (ackMessage == "OK")
                    IsLoggedIn = true;
                else
                {
                    Console.WriteLine("Could not login to the chat..");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in login acknowledgement.. " + ex.Message);
            }
        }

    }
}
