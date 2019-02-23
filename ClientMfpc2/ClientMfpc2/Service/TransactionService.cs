using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMfpc2.Service
{
    class TransactionService
    {
        private RpcClient rpcClient;
        public RpcClient RpcClient { get { return rpcClient; } }

        public TransactionService(RpcClient rpcClient)
        {
            this.rpcClient = rpcClient;
        }

        internal void CloseConnection()
        {
            rpcClient.ClientSocket.Close();
        }
        public string TransactionCase1()
        {
            string request = "InsertStudent;null;1/Marius/23/1466||GetStudent;1||GetCursById;1||UpdateCurs;1;1/Mate/24/1/Analiza1";
            Console.WriteLine(request);
            string text = rpcClient.SendAndReceive(request).Content;
            return text;
        }
        public string TransactionCase2()
        {
            string request = "InsertStudent;null;1/Daniel/21/868||GetStudent;2||UpdateStudent;2;2/Daniel/22/868||GetStudent;2&InsertProfesor;null" +
                ";1/Profesor1/50/Analiza Matematica||GetProfesor;1||UpdateProfesor;1;1/Prof1/52/Algebra||GetProfesor;1";
            Console.WriteLine(request);
            string text = rpcClient.SendAndReceive(request).Content;
            return text;
        }
        public string TransactionCase3()
        {
            string request = "InsertCurs;null;1/Analiza/30/1/descriere1||GetCurs;1&GetCurs;1||UpdateCurs;1;1/Algebra/31/1/descriere2||GetCurs;1||DeleteCurs;1";
            string text = rpcClient.SendAndReceive(request).Content;
            return text;
        }
        //trei tranzactii
        //deadlock
        public string TransactionCase4()
        {
            string request = "InsertCurs;null;1/Algebra/100/1/descriere1||GetCurs;1||GetStudent;1||DeleteStudent;1&InsertStudent;null;1/Marius/23/1466||" +
                "GetStudent;1||UpdateCurs;1;1/MateAlgebra/120/1/descriere2||GetCurs;1&GetCurs;1||DeleteCurs;1||GetStudent;1";
            string text = rpcClient.SendAndReceive(request).Content;
            return text;

        }
        //trebuie sa avem curs 1
        public string TransactionCase5()
        {
            string request = "GetCurs;1||UpdateCurs;1;1/Algebra/100/1/descriere5||GetStudent;2||UpdateStudent;2;2/Mihai/23/868&" +
                "GetStudent;1||UpdateStudent;1;1/Mircea/25/959||GetCurs;1||UpdateCurs;1;1/Info/200/1/descriere6&" +
                "GetStudent;2||UpdateStudent;2;2/Ioan/23/1466||GetStudent;1||UpdateStudent;1;1/Alin/40/766";
            string text = rpcClient.SendAndReceive(request).Content;
            return text;

        }
        public string TransactionCase6()
        {
            string request = "GetCurs;1||UpdateCurs;1;1/Sisteme dinamice/100/1/descriere5||GetCurs;1&GetStudent;1||UpdateStudent;1;1/Alex/22/954||GetStudent;1&" +
                "GetProfesor;1||UpdateProfesor;1;1/Profesor1732/60/Sisteme dinamice||GetProfesor;1&GetStudent;2||UpdateStudent;2;2/Marian/24/933||GetStudent;2";
            string text = rpcClient.SendAndReceive(request).Content;
            return text;
        }
    }
}
