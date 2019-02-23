using System;
using System.Collections.Generic;
using System.Text;

namespace ProiectMFPC_V1.src
{
    class Deadlock
    {
        public int id { get; set; }
        public string lockType { get; set; }
        public string lockTable { get; set; }
        public int lockObject { get; set; }
        public int tranHasLock { get; set; }
        public List<int> transWaitsLock { get; set; }

        public Deadlock(int id, string lockType, string lockTable, int lockObject, int tranHasLock, List<int> transWaitsLock)
        {
            this.id = id;
            this.lockType = lockType;
            this.lockTable = lockTable;
            this.lockObject = lockObject;
            this.tranHasLock = tranHasLock;
            this.transWaitsLock = transWaitsLock;
        }
        

    }
}
