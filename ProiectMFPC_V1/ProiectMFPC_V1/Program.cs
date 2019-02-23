using ProiectMFPC_V1.src.Net;
using System;

namespace ProiectMFPC_V1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                RPCServer rpcServer = new RPCServer("127.0.0.1", 2016);
                rpcServer.StartServer();
            }
            catch (Exception ex)
            {
                Console.WriteLine(" $$ " + " Unexpected error occured " + ex.Message.ToString());
            }
        }
    }
}
