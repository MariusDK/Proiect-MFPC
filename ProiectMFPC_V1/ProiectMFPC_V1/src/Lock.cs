using System;
using System.Collections.Generic;
using System.Text;

namespace ProiectMFPC_V1.src
{
    class Lock
    {
        public int id { get; set; }
        public String lockType { get; set; }
        public int recordId { get; set; }
        public string tableName { get; set; }
        public int transactionId { get; set; }
      

        public Lock()
        {

        }
        public Lock(int id, string lockType, int recordId, string tableName, int transactionId)
        {
            this.id = id;
            this.lockType = lockType;
            this.recordId = recordId;
            this.tableName = tableName;
            this.transactionId = transactionId;
        }

    }
}
