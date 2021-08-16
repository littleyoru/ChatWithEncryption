using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatWithEncryption
{
    class ChatServer : TcpListener
    {
        private static int Port = 13999;
        public static int GetPort() { return Port; }
        public List<ChatClient> ConnectedClients = new List<ChatClient>();
        protected List<Tuple<string, string>> MessageQueue = new List<Tuple<string, string>>(); // <Receiver, Message>

        public ChatServer() : base(IPAddress.Any, 13999)
        {

        }

        public void RunServer()
        {
            Start();
            AcceptClients(this);
            
        }

        public async void AcceptClients(TcpListener listener)
        {
            bool isRunning = true;
            while (isRunning)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                ChatClient chatClient = (ChatClient)client;
                
                NetworkStream stream = chatClient.GetStream();

                // Wait for client login message
                while (!chatClient.IsLoggedIn)
                {
                    chatClient.IsLoggedIn = await WaitForLoginMessage(stream);
                }

                ConnectedClients.Add(chatClient);



                ChatActions.ListenForMessages(stream, true);

            }
        }

        public async Task<bool> WaitForLoginMessage(NetworkStream stream)
        {
            try
            {
                byte[] buffer = new byte[256];
                int bytesRead = await stream.ReadAsync(buffer, 0, 256);
                string username = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in receiving login message from client! " + ex.Message);
                return false;
            }
        }

        public async void SendAckMessage(NetworkStream stream)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes("OK");
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in sending ack message.. " + ex.Message);
            }
        }
    }
}
