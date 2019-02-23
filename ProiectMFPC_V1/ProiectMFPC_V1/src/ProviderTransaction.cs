using MySql.Data.MySqlClient;
using ProiectMFPC_V1.src;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProiectMFPC_V1
{
    class ProviderTransaction
    {
        private DBConnection dbCon;
        public ProviderTransaction()
        {
            dbCon = new DBConnection("management2");
        }
        public void StoreTransaction(Transaction transaction)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "INSERT INTO transaction (id, Timestamp, Status, operations) VALUES (@id, @timestamp, @status, @operations)";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", transaction.id);
                cmd.Parameters.AddWithValue("@timestamp", transaction.timestamp);
                cmd.Parameters.AddWithValue("@status", transaction.status);
                cmd.Parameters.AddWithValue("@operations", transaction.operationFormatValue);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }
        }
        public void UpdateTransaction(Transaction transaction)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "UPDATE transaction SET Timestamp=@timestamp, Status=@Status, operations=@operations WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", transaction.id);
                cmd.Parameters.AddWithValue("@timestamp", transaction.timestamp);
                cmd.Parameters.AddWithValue("@status", transaction.status);
                cmd.Parameters.AddWithValue("@operations", transaction.operationFormatValue);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error updating data into Database!");
            }
        }
        public Transaction GetTransaction(int id)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT * FROM transaction WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                reader.Read();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string idString = reader["id"].ToString();
                    string timestampString = reader["Timestamp"].ToString();
                    string status = reader["Status"].ToString();
                    string operations = reader["operations"].ToString();
                    List<string> operationList = ConvertOps(operations);
                    int idTrans = Convert.ToInt32(idString);
                    DateTime timestamp = Convert.ToDateTime(timestampString);
                    Transaction transaction = new Transaction(idTrans,timestamp,status,operations);
                    dbCon.CloseConnection();
                    return transaction;
                }
            }
            return null;
        }
        public List<string> ConvertOps(string operations)
        {
            string[] operationsArray = operations.Split("|");
            List<string> operationList = new List<string>();
            foreach (string operation in operationsArray)
            {
                operationList.Add(operation);
            }
            return operationList;
        }
        public bool isEmpty()
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT count(*) FROM transaction";
                var cmd = new MySqlCommand(query, dbCon.Connection);
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
            dbCon.CloseConnection();
            return false;
        }
        public int GetNextIdTransacion()
        {
            if (isEmpty() == true)
            {
                return 1;
            }
            else
            {
                if (dbCon.OpenConnection() == true)
                {
                    String query = "SELECT id FROM transaction ORDER BY ID DESC LIMIT 1";
                    var cmd = new MySqlCommand(query, dbCon.Connection);
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    //Console.WriteLine(reader.FieldCount);
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
}
