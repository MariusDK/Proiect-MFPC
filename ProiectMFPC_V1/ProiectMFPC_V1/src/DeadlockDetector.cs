using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProiectMFPC_V1.src
{
    class DeadlockDetector
    {
        public DBConnection dbCon;
        public DeadlockDetector()
        {
            dbCon = new DBConnection("management2");
        }
        public int AddDataToDeadlockTable(string lockType, string lockTable, int lockObject, int idTransaction, List<int> transWait)
        {
            int nextDeadlockId = GetNextDeadlockId();
            string transWaitLock = GetStringForm(transWait);
            if (dbCon.OpenConnection() == true)
            {
                String query = "INSERT INTO `deadlock` (`id`, `lockType`, `lockTable`, `lockObject`, `transThisLock`, `transWaitsLock`) VALUES (@id, @lockType, @lockTable, @lockObject, @transThisLock, @transWaitsLock)";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                
                cmd.Parameters.AddWithValue("@lockType", lockType);
                cmd.Parameters.AddWithValue("@lockObject", lockObject);
                cmd.Parameters.AddWithValue("@lockTable", lockTable);
                cmd.Parameters.AddWithValue("@transThisLock", idTransaction);
                cmd.Parameters.AddWithValue("@transWaitsLock", transWaitLock);
                try
                {
                    cmd.Parameters.AddWithValue("@id", nextDeadlockId);
                    int result = cmd.ExecuteNonQuery();
                    dbCon.CloseConnection();
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");
                    else
                        return nextDeadlockId;
                }
                catch (Exception)
                {
                    nextDeadlockId = nextDeadlockId + idTransaction;
                    cmd.Parameters.RemoveAt("@id");
                    cmd.Parameters.AddWithValue("@id", nextDeadlockId);
                    int result = cmd.ExecuteNonQuery();
                    dbCon.CloseConnection();
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");
                    else
                        return nextDeadlockId;

                }
            }
            return 0;
        }
        public void UpdateDeadlock(int id, string type)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "UPDATE `deadlock` SET lockType=@type WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@id", id);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error updating data into Database!");
            }
        }
        public bool isEmpty()
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT count(*) FROM `deadlock`";
                var cmd = new MySqlCommand(query, dbCon.Connection);

                //var reader = cmd.ExecuteReader();
                int count = (int)(long)cmd.ExecuteScalar();
                dbCon.CloseConnection();
                if (count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public int GetNextDeadlockId()
        {
            if (isEmpty() == true)
            {
                return 1;
            }
            else
            {
                if (dbCon.OpenConnection() == true)
                {
                    String query = "SELECT id FROM deadlock ORDER BY ID DESC LIMIT 1";
                    var cmd = new MySqlCommand(query, dbCon.Connection);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string idString = reader["id"].ToString();
                        int id = Convert.ToInt32(idString);
                        id++;
                        dbCon.CloseConnection();
                        return id;
                    }

                }
                dbCon.CloseConnection();
            }
            return 0;

        }
        public string GetStringForm(List<int> transactionIds)
        {
            string transactionList = "";
            foreach (int idTrans in transactionIds)
            {
                transactionList = idTrans + ";";
            }
            return transactionList;
        }
        public void EliberateDeadlock(int id)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "DELETE FROM deadlock WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", id);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error delete data into Database!");
            }
        }
        public Deadlock GetDeadlock(int idResursa,string tableName)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT * FROM `deadlock` WHERE `lockObject`=@idResursa AND `lockTable`=@tableName";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@idResursa", idResursa);
                cmd.Parameters.AddWithValue("@tableName", tableName);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string idString = reader["id"].ToString();
                        string type = reader["lockType"].ToString();
                        string objectId = reader["lockObject"].ToString();
                        string table = reader["lockTable"].ToString();
                        string transactionIdString = reader["transThisLock"].ToString();
                        string transWaitsLock = reader["transWaitsLock"].ToString();
                        List<int> transWaitsLockList = GetTransLockList(transWaitsLock);
                        int id = Convert.ToInt32(idString);
                        int recordId = Convert.ToInt32(objectId);
                        int transactionId = Convert.ToInt32(transactionIdString);
                        Deadlock deadlock = new Deadlock(id, type, table, recordId, transactionId, transWaitsLockList);
                        dbCon.CloseConnection();
                        return deadlock;
                    }
                }
                dbCon.CloseConnection();
            }
            return null;
        }
        public List<int> GetTransLockList(string transLockId)
        {
            Console.WriteLine(transLockId);
            List<int> transWaitLockId = new List<int>();
            if (transLockId.Equals(""))
            {
                return transWaitLockId;
            }
            else
            {
                string[] transWait = transLockId.Split(";");
                foreach (string tran in transWait)
                {   if (!tran.Equals(""))
                    {
                        int idTran = Convert.ToInt32(tran);

                        transWaitLockId.Add(idTran);
                    }
                }
                return transWaitLockId;
            }
        }
        public void AddTransactionInWait(Deadlock deadlock)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "UPDATE `deadlock` SET `lockType`=@type, `lockTable`=@table, `lockObject`=@objectId, `transWaitsLock`=@transWaitsLock WHERE `id`=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", deadlock.id);
                cmd.Parameters.AddWithValue("@type", deadlock.lockType);
                cmd.Parameters.AddWithValue("@objectId", deadlock.lockObject);
                cmd.Parameters.AddWithValue("@table", deadlock.lockTable);
                string transWait = GetStringForm(deadlock.transWaitsLock);
                cmd.Parameters.AddWithValue("@transWaitsLock", transWait);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error updating data into Database!");
            }
        }
        public List<Deadlock> GetDeadlocks()
        {
            lock (this)
            {
                List<Deadlock> deadlocks = new List<Deadlock>();
                if (dbCon.OpenConnection() == true)
                {
                    String query = "SELECT * FROM `deadlock`";
                    var cmd = new MySqlCommand(query, dbCon.Connection);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        string idString = reader["id"].ToString();
                        string type = reader["lockType"].ToString();
                        string objectId = reader["lockObject"].ToString();
                        string table = reader["lockTable"].ToString();
                        string transactionIdString = reader["transThisLock"].ToString();
                        string transWaitsLock = reader["transWaitsLock"].ToString();
                        List<int> transWaitsLockList = GetTransLockList(transWaitsLock);
                        int id = Convert.ToInt32(idString);
                        int recordId = Convert.ToInt32(objectId);
                        int transactionId = Convert.ToInt32(transactionIdString);
                        Deadlock deadlock = new Deadlock(id, type, table, recordId, transactionId, transWaitsLockList);
                        deadlocks.Add(deadlock);
                    }

                }
                dbCon.CloseConnection();
                return deadlocks;
        }
    }
    
        public List<Deadlock> GetDeadlocksThatTransWaits(List<Deadlock> deadlocks, int idTrans)
        {
            List<Deadlock> deadlockList = new List<Deadlock>();
            foreach (Deadlock deadlock in deadlocks)
            {
                List<int> transWaitsIds = deadlock.transWaitsLock;
                foreach (int idTr in transWaitsIds)
                {
                    if (idTr == idTrans)
                    {
                        deadlockList.Add(deadlock);
                        break;
                    }
                }
            }
            return deadlockList;
        }
        public bool DeadlockDetectionMecanism(int idTran)
        {
            List<Deadlock> allDeadlocks = GetDeadlocks();
            List<Deadlock> deadlocksThatTransWait = GetDeadlocksThatTransWaits(allDeadlocks, idTran);
            for (int i=0;i<deadlocksThatTransWait.Count;i++)
            {
                if (idTran == deadlocksThatTransWait[i].tranHasLock)
                {
                    return true;
                }
                else {
                    foreach (Deadlock deadlockOfTrans in GetDeadlocksThatTransWaits(allDeadlocks, deadlocksThatTransWait[i].tranHasLock))
                    {
                        deadlocksThatTransWait.Add(deadlockOfTrans);
                    }
                }
            }
            return false;
        }
        public void DeleteDeadlockData(int tranId)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "DELETE FROM deadlock WHERE transThisLock=@transactionId";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@transactionId", tranId);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error delete data into Database!");
            }
        }

    }
}
