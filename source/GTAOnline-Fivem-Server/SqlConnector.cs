using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FiveM_Online_Server
{
    class SqlConnector
    {
        //database stuff
        private const String SERVER = "127.0.0.1";
        private const String DATABASE = "fivemonline";
        private const String UID = "root";
        private const String PASSWORD = "password";
        private static MySqlConnection dbConn;

        public static void InitializeDB()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = SERVER;
            builder.UserID = UID;
            builder.Password = PASSWORD;
            builder.Database = DATABASE;

            String connString = builder.ToString();

            builder = null;

            Console.WriteLine(connString);

            dbConn = new MySqlConnection(connString);

        }

        public static void SavePlayer(int pid, string name)
        {

            InitializeDB();

            String query = string.Format("INSERT INTO players (id, name) VALUES ({0}, '{1}')", pid, name);

            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();

            cmd.ExecuteNonQuery();
            int id = (int)cmd.LastInsertedId;

            dbConn.Close();

        }
    }
}
