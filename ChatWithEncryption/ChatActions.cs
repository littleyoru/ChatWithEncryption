using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ChatWithEncryption
{
    static class ChatActions
    {
        
        public static async void ListenForMessages(NetworkStream stream, bool isServer = false)
        {
            try
            {
                byte[] buffer = new byte[256];
                bool isRunning = true;

                while (isRunning)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    

                    // Decrypt message
                    var exactBuffer = new byte[bytesRead];
                    Array.Copy(buffer, exactBuffer, bytesRead);
                    var crypt = new EncryptionV();
                    var decryptedMessage = crypt.DecryptV(exactBuffer);
                    

                    string messageReceived = Encoding.UTF8.GetString(decryptedMessage, 0, bytesRead);

                    Console.WriteLine("Another Client writes: " + messageReceived);


                    if (isServer)
                    {
                        var messageParts = msgS.Split(':');
                        var receiver = messageParts[0].Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when reading stream or client/server disconnected: " + ex.Message);
            }

        }

        public static async void SendMessage(NetworkStream stream)
        {
            bool isRunning = true;
            while (isRunning)
            {
                // Send a message
                Console.WriteLine("\tEnter the message with the following format: \"<Receiver name>: <Message content>\"");
                Console.WriteLine("tEnter All as name for broadcast messages");
                string msgToSend = Console.ReadLine().Trim();

                // Separate receiver name from rest of message


                // int noOfBytes = Encoding.UTF8.GetBytes(msgToSend, 0, msgToSend.Length, bufferS, 0);

                byte[] buffer = Encoding.UTF8.GetBytes(msgToSend);

                stream.Write(buffer, 0, buffer.Length);
            }
        }

        public static async void ForwardMessage()
        {

        }

    }
}
