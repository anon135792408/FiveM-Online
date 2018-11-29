using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using GTAOnline_FiveM;

namespace GTAOnline_FiveM {
    public class DBConnect : BaseScript {


        //database stuff
        private const String SERVER = "127.0.0.1:3306";
        private const String DATABASE = "test";
        private const String UID = "root";
        private const String PASSWORD = "password";
        private static MySqlConnection dbConn;

        public DBConnect() {
        }

        public static void InitializeDB() {
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

        public static DBConnect Insert(String u, String p) {
            String query = string.Format("INSERT INTO users(username, password) VALUES ('{0}', '{1}')", u, p);
            MySqlCommand cmd = new MySqlCommand(query, dbConn);
            dbConn.Open();
            cmd.ExecuteNonQuery();
            int id = (int)cmd.LastInsertedId;
            DBConnect user = new DBConnect();
            dbConn.Close();
            return user;
        }

        public void Update(string u, string p) {
            String query = string.Format("UPDATE users SET username='{0}', password='{1}' WHERE ID={2}");
            MySqlCommand cmd = new MySqlCommand(query, dbConn);
            dbConn.Open();
            cmd.ExecuteNonQuery();
            dbConn.Close();
        }

        public GamePlayer GetPlayerByName(string u) {
            GamePlayer result = new GamePlayer();

            String query = "SELECT * FROM users WHERE Name = '" + u + "'";
            MySqlCommand cmd = new MySqlCommand(query, dbConn);
            dbConn.Open();
            MySqlDataReader reader = (MySqlDataReader)cmd.ExecuteReader();

            while (reader.Read()) {
                result.Heading = float.Parse(reader["Heading"].ToString());

                float x = float.Parse(reader["PosX"].ToString());
                float y = float.Parse(reader["PosY"].ToString());
                float z = float.Parse(reader["PosZ"].ToString());
                result.LastPosition = new Vector3(x, y, z);

                result.Money = (long)reader["Money"];
                result.Xp = (long)reader["XP"];
            }

            return result;
        }

        public void Delete() {
            String query = string.Format("DELETE FROM users WHERE ID={0}");
            MySqlCommand cmd = new MySqlCommand(query, dbConn);
            dbConn.Open();
            cmd.ExecuteNonQuery();
            dbConn.Close();
        }
    }
}