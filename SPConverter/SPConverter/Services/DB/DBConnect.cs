using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SPConverter.Services.DB
{
    class DBConnect
    {
        private MySqlConnection _connection;
        private string _server;
        private string _port;
        private string _database;
        private string _uid;
        private string _password;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            _server = "cp418.agava.net";//"cp418.agava.net";
            _port = "2083";
            _database = "sportpr6_wordpress";
            _uid = "sportpr6_new";
            _password = ".aU1Ttf%xwrt";
            string connectionString;
            connectionString = string.Format("SERVER={0};PORT={1};DATABASE={2};UID={3};PASSWORD={4};Connection Timeout=45;", _server, _port, _database, _uid, _password); //string.Format("SERVER={0};PORT={1};DATABASE={2};UID={3};PASSWORD={4};", _server, _port, _database, _uid, _password);

            _connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                if (!_connection.Ping())
                    _connection.Open();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        //Close connection
        private void CloseConnection()
        {
        }

        //Insert statement
        public void Insert()
        {
        }

        //Update statement
        public void Update()
        {
        }

        //Delete statement
        public void Delete()
        {
        }

        //Backup
        public void Backup()
        {
        }

        //Restore
        public void Restore()
        {
        }
    }
}
