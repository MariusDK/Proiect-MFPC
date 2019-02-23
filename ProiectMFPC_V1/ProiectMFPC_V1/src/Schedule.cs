using System;
using System.Collections.Generic;
using System.Text;

namespace ProiectMFPC_V1.src
{
    class Schedule
    {
        private List<string> operations { get; set; }

        private Schedule()
        {
            operations = new List<string>();
        }

        public void pushOperation(string operation)
        {
            operations.Add(operation);
        }
        public string getNextOperation()
        {
            string operation = operations[0];
            operations.RemoveAt(0);
            return operation;
        }
    }
}
