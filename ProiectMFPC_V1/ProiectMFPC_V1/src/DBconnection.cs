using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace ProiectMFPC_V1.src
{
    class DBConnection
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        public bool connectionStatus;


        //Constructor
        public DBConnection(string dbName)
        {
            Initialize(dbName);
        }

        //Initialize values
        private void Initialize(string dbName)
        {
            server = "localhost";
            database = dbName;
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }
        public bool isConnected()
        {
            return connectionStatus;
        }
        public MySqlConnection Connection
        { get { return connection; } }
        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                connectionStatus = true;
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                connectionStatus = false;
                return false;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                connectionStatus = false;
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
