using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientMfpc2
{
    public class RpcClient
    {
        private TcpClient tcpClient;
        private NetworkStream netStream;
        private StreamReader clientReader;
        private StreamWriter clientWriter;
        public TcpClient ClientSocket { get { return ClientSocket; } }

        public RpcClient(string host, int port)
        {
            try
            {
                tcpClient = new TcpClient(host, port);
                netStream = tcpClient.GetStream();
                clientReader = new StreamReader(netStream);
                clientWriter = new StreamWriter(netStream);
            }
            catch (SocketException ex)
            {
                throw new ServiceException(ex.Message);
            }
        }
        internal Message SendAndReceive(string data)
        {
            Message request = new Message();
            request.Content = data;
            SendRequest(request);
            return ReceiveResponse();
        }

        private Message ReceiveResponse()
        {
            string[] responses = null;
            string responseAsJson = "";
            Message response = new Message();

            responseAsJson = clientReader.ReadLine();
            responses = responseAsJson.Split('}');
            foreach (string res in responses)
            {   if (!res.Equals(""))
                {
                    string newRes = res + "}";
                    Message message = JsonConvert.DeserializeObject<Message>(newRes);
                    response.Content = response.Content + "\r\n" + message.Content;
                }
            }

            Console.WriteLine(response.Content);
            return response;
        }

        private void SendRequest(Message request)
        {
            string requestAsJson = JsonConvert.SerializeObject(request);
            clientWriter.WriteLine(requestAsJson);
            clientWriter.Flush();
        }
    }
}
