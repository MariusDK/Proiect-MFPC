using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProiectMFPC_V1.src
{
    class LockTable
    {
        public int lockNumber=0;
        public DBConnection dbCon;
        public LockTable()
        {
            dbCon = new DBConnection("management2");

        }
        public bool checkIfTableExists()
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'management' AND table_name = 'lock'";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows == true)
                {
                    dbCon.CloseConnection();
                    return true;
                }
                else
                {
                    dbCon.CloseConnection();
                    return false;
                }
            }
            return false;
        }
        public bool isEmpty()
        {
            var valoare = checkIfTableExists();
            if (dbCon.OpenConnection() == true)
            {
                

                String query = "SELECT count(*) FROM `lock`";
                var cmd = new MySqlCommand(query, dbCon.Connection);

                Console.WriteLine(cmd.ExecuteScalar());
                //var reader = cmd.ExecuteReader();
                int count = (int)(long)cmd.ExecuteScalar();
                if (count == 0)
                {
                    dbCon.CloseConnection();
                    return true;
                }
                else
                {
                    dbCon.CloseConnection();
                    return false;
                }
            }
            return false;
        }
        public int IsLock(int idResursa, string tableName)
        {
            if (isEmpty() == true)
            {
                return 0;
            }
            else
            {
                if (dbCon.OpenConnection() == true)
                {
                    String query = "SELECT * FROM `lock` WHERE objectId=@idResursa AND `table`=@tableName";
                    var cmd = new MySqlCommand(query, dbCon.Connection);
                    cmd.Parameters.AddWithValue("@idResursa", idResursa);
                    cmd.Parameters.AddWithValue("@tableName", tableName);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string idString = reader["id"].ToString();
                            int id = Convert.ToInt32(idString);
                            dbCon.CloseConnection();
                            return id;
                        }
                    }
                    dbCon.CloseConnection();
                }
                return 0;
            }
        }
        public int ObtainLock(string lockType, string lockTable, int lockObject, int idTransaction)
        {
                int nextLockId = GetNextLockId();
                if (dbCon.OpenConnection() == true)
                {
                    String query = "INSERT INTO `lock` (`id`, `type`, `objectId`, `table`, `transactionId`) VALUES (@id, @type, @objectId, @table, @transactionId)";
                    var cmd = new MySqlCommand(query, dbCon.Connection);
                    cmd.Parameters.AddWithValue("@type", lockType);
                    cmd.Parameters.AddWithValue("@objectId", lockObject);
                    cmd.Parameters.AddWithValue("@table", lockTable);
                    cmd.Parameters.AddWithValue("@transactionId", idTransaction);
                try
                {
                    cmd.Parameters.AddWithValue("@id", nextLockId);
                    int result = cmd.ExecuteNonQuery();
                    dbCon.CloseConnection();
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");
                    else
                        return nextLockId;
                }
                catch (Exception)
                {
                    nextLockId = nextLockId + idTransaction;
                    cmd.Parameters.RemoveAt("@id");
                    cmd.Parameters.AddWithValue("@id", nextLockId);
                    int result = cmd.ExecuteNonQuery();
                    dbCon.CloseConnection();
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");
                    else
                        return nextLockId;

                }
                }
            return 0;
        }
        public void UpdateLock(int id, string type)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "UPDATE `lock` SET type=@type WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@id", id);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error updating data into Database!");
            }
        }
        public Lock GetLock(int lockId)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT * FROM `lock` WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", lockId);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string idString = reader["id"].ToString();
                        string type = reader["type"].ToString();
                        string objectId = reader["objectId"].ToString();
                        string table = reader["table"].ToString();
                        string transactionIdString = reader["transactionId"].ToString();
                        int id = Convert.ToInt32(idString);
                        int recordId = Convert.ToInt32(objectId);
                        int transactionId = Convert.ToInt32(transactionIdString);
                        Lock lock1 = new Lock(id, type, recordId, table, transactionId);
                        dbCon.CloseConnection();
                        return lock1;
                    }
                }
            }
            return null;
        }
        public int IsLockTable(string tableName)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT * FROM lock WHERE table=@tableName";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@tableName", tableName);
                var reader = cmd.ExecuteReader();
                while (reader.Read()) ;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string idString = reader["id"].ToString();
                    int id = Convert.ToInt32(idString);
                    return id;
                }
            }
            return 0;
            
        }
        //aici get last element id dupa id++
        public int GetNextLockId()
        {

                this.lockNumber++;
                if (isEmpty() == true)
                {
                    return 1;
                }
                else
                {
                    if (dbCon.OpenConnection() == true)
                    {

                        String query = "SELECT id FROM `lock` ORDER BY ID DESC LIMIT 1";
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
                return 0;
            }
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
        public List<int> GetTransLockList(string transLockId)
        {
            List<int> transWaitLockId = new List<int>();
            string[] transWait = transLockId.Split(";");
            foreach (string tran in transWait)
            {
                int idTran = Convert.ToInt32(tran);
                transWaitLockId.Add(idTran);
            }
            return transWaitLockId;
        }
        public void AddTransactionInWait(Lock lock1)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "UPDATE lock SET id=@id, type=@type, objectId=@objectId, table=@table, transactionId=@transactionId";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", lock1.id);
                cmd.Parameters.AddWithValue("@type", lock1.lockType);
                cmd.Parameters.AddWithValue("@objectId", lock1.recordId);
                cmd.Parameters.AddWithValue("@table", lock1.tableName);
                cmd.Parameters.AddWithValue("@transactionId", lock1.transactionId);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error updating data into Database!");
            }
        }
        public void EliberateLock(int id)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "DELETE FROM lock WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", id);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error delete data into Database!");
            }
        }
        public void EliberateLocks(int tranId)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "DELETE FROM `lock` WHERE `transactionId`=@transactionId";
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
