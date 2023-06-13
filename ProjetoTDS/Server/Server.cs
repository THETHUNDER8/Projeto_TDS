using EI.SI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        private const int PORT = 10000;
        private static ConcurrentBag<ClientHandler> clients = new ConcurrentBag<ClientHandler>();

        static void Main(string[] args)
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, PORT);
            TcpListener listener = new TcpListener(endpoint);

            listener.Start();
            Console.WriteLine("SERVER READY");
            int clientCounter = 0;

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                clientCounter++;
                Console.WriteLine("Client {0} connected", clientCounter);
                ClientHandler clientHandler = new ClientHandler(client, clientCounter);
                clients.Add(clientHandler); // Add the client handler to the list
                clientHandler.Handle();
            }
        }

        // method to send a message to all clients
        public static void BroadcastMessage(string message)
        {
            foreach (var client in clients)
            {
                client.SendMessage(message);
            }
        }
    }

    class ClientHandler
    {
        private TcpClient client;
        private int clientID;
        private NetworkStream networkStream;
        private ThreadLocal<ProtocolSI> protocolSIThreadLocal;

        public ClientHandler(TcpClient client, int clientID)
        {
            this.client = client;
            this.clientID = clientID;
            this.networkStream = client.GetStream();
            this.protocolSIThreadLocal = new ThreadLocal<ProtocolSI>(() => new ProtocolSI());
        }

        public void Handle()
        {
            Thread thread = new Thread(ThreadHandler);
            thread.Start();
        }

        private void ThreadHandler()
        {
            try
            {
                while (protocolSIThreadLocal.Value.GetCmdType() != ProtocolSICmdType.EOT)
                {
                    int bytesRead = networkStream.Read(protocolSIThreadLocal.Value.Buffer, 0, protocolSIThreadLocal.Value.Buffer.Length);
                    byte[] ack;

                    switch (protocolSIThreadLocal.Value.GetCmdType())
                    {
                        case ProtocolSICmdType.DATA:
                            string message = protocolSIThreadLocal.Value.GetStringFromData();
                            Console.WriteLine("Client {0}: {1}", clientID, message);

                            // send the message to all clients
                            Server.BroadcastMessage(message);

                            ack = protocolSIThreadLocal.Value.Make(ProtocolSICmdType.ACK);
                            networkStream.Write(ack, 0, ack.Length);
                            break;

                        case ProtocolSICmdType.EOT:
                            Console.WriteLine("Ending Thread from Client {0}", clientID);
                            ack = protocolSIThreadLocal.Value.Make(ProtocolSICmdType.ACK);
                            networkStream.Write(ack, 0, ack.Length);
                            break;
                    }
                }
            }
            catch (IOException ex)
            {
                // if the client disconnects
                Console.WriteLine("Error handling client {0}: {1}", clientID, ex.Message);
            }
            finally
            {
                networkStream.Close();
                client.Close();
            }
        }

        // method to send a message
        public void SendMessage(string message)
        {
            lock (networkStream) // Synchronize access to the network stream
            {
                // Convert the message to bytes
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                // Create the protocolSI data packet
                byte[] data = protocolSIThreadLocal.Value.Make(ProtocolSICmdType.DATA, messageBytes);

                // Send the data packet over the network
                networkStream.Write(data, 0, data.Length);
            }
        }
    }

}