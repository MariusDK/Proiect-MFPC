using ProiectMFPC_V1.src.Util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;

namespace ProiectMFPC_V1.src.Net
{
    class RPCServer
    {
        public delegate Message MethodHandler(Message request);
        private IPEndPoint endPoint;

        public RPCServer(string serverAddress, int port)
        {
            IPAddress iPAddress = IPAddress.Parse(serverAddress);
            this.endPoint = new IPEndPoint(iPAddress, port);
        }
        internal void StartServer()
        {
            Console.WriteLine("Server started");

            Socket serverSocket = null;
            try
            {
                serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(10);
                while (true)
                {
                    Console.WriteLine("Waiting for connections...");
                    Socket clientSocket = serverSocket.Accept();
                    Console.WriteLine("Connection accepted");
                    Task.Factory.StartNew(new ClientHandler(clientSocket).Handle);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("ERROR: Socket exception" + e.Message);
            }
            finally
            {
                if (serverSocket != null)
                    serverSocket.Close();
            }
        }

        private class ClientHandler
        {
            private Socket clientSocket;
 

            public ClientHandler(Socket clientSocket)
            {
                this.clientSocket = clientSocket;
            }

            internal void Handle()
            {
                try
                {
                    Console.WriteLine("Begin resolving requests...");
                    Message request = ReceiveRequest(clientSocket);
                    string methods = request.Content;
                    Console.WriteLine("Methods");
                    Console.WriteLine(request.Content);
                    string transResponses = "";
                    string[] transMeth = methods.Split("&");
                    List<Transaction> transactions = new List<Transaction>();
                    Console.WriteLine(transMeth.Length);
                    foreach (string tranMeth in transMeth)
                    {
                        Transaction transaction = new Transaction();
                        transactions.Add(transaction);
                        transaction.SetOperations(tranMeth);
                        ProviderTransaction providerTransaction = new ProviderTransaction();
                        providerTransaction.StoreTransaction(transaction);
                        System.Threading.Thread myThread = new System.Threading.Thread(new
   System.Threading.ThreadStart(transaction.Run));
                        myThread.Start();
                        //myThread.Join();
                        
                    }
                    Exception exception = null;
                    Message response = null;
                    MethodHandler methodHandler = null;
                    //tratare lista metode
                    Thread.Sleep(2000);
                    response = new Message();
                    foreach (Transaction t in transactions)
                    {
                        response.Content = t.response;
                        if (exception != null)
                        {
                            response = new Message();
                            response.Content = exception.Message;
                        }
                        SendResponse(clientSocket, response);
                        Console.WriteLine("Response sent");
                    }
                }
                catch (Exception serviceException)
                {
                    Console.WriteLine("Service exception " + serviceException.Message);
                }
                finally
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
        private void SendResponse(Socket clientSocket, Message response)
        {
            string responseAsJson = JsonConvert.SerializeObject(response);
            byte[] responseAsBytes = Encoding.ASCII.GetBytes(responseAsJson);
            clientSocket.Send(responseAsBytes);
        }
        private Message ReceiveRequest(Socket clientSocket)
        {
            byte[] bytes = new byte[4096];
            int bytesReceived = clientSocket.Receive(bytes);
            string requestAsString = Encoding.ASCII.GetString(bytes, 0, bytesReceived);
            Message request = JsonConvert.DeserializeObject<Message>(requestAsString);
            return request;
        }
    }
    }
}
