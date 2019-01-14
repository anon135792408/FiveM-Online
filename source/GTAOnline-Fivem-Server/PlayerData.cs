using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MySql.Data.MySqlClient;

namespace GTAOnline_Fivem_Server
{
    class PlayerData : BaseScript
    {
        private readonly string _connStr = @"server=127.0.0.1;port=3306;database=fivemonline;uid=root;pwd=password;sslmode=none;";

        MySqlConnection _conn;

        public PlayerData()
        {
            EventHandlers.Add("GTAO:SavePlayerData", new Action<string, string>(SavePlayerData));
            EventHandlers.Add("GTAO:RetrieveSQLPlayerList", new Action<int>(RetrieveSQLPlayerListForClient));
            EventHandlers.Add("GTAO:RetrievePlayerIdentifier", new Action<int>(RetrievePlayerIdentifier));
            EventHandlers.Add("GTAO:RetrievePlayerLastPos", new Action<int>(RetrievePlayerLastPos));
            _conn = new MySqlConnection(_connStr);
        }

        public void RetrievePlayerLastPos(int id)
        {
            Player p = new PlayerList()[id];
            p.TriggerEvent("GTAO:SyncPlayerLastPos", GetUserLastPosition(p.Name));
        }

        public void SavePlayerData(string id, string name)
        {
            Debug.WriteLine("===========================");
            Debug.WriteLine(id + " " + name);
            Debug.WriteLine("===========================");

            //Player p = new PlayerList()[id];
            //string identifier = p.Identifiers["license"].ToString();

            string query = "INSERT INTO players(name) VALUES('" + name + "')";
            MySqlCommand cmd = new MySqlCommand(query, _conn);
            _conn.Open();

            cmd.ExecuteNonQuery();
            _conn.Close();
        }

        public void RetrieveSQLPlayerListForClient(int id)
        {
            Player p = new PlayerList()[id];
            p.TriggerEvent("GTAO:SyncSQLPlayerList", GetAllUsers());
        }

        public void RetrievePlayerIdentifier(int id)
        {
            Player p = new PlayerList()[id];
            p.TriggerEvent("GTAO:SyncPlayerIdentifier", GetPlayerIdentifierFromId(id));
        }

        public string GetPlayerIdentifierFromId(int id)
        {
            Player p = new PlayerList()[id];
            return p.Identifiers["license"];
        }

        public List<string> GetAllUsers()
        {
            List<string> uNames = new List<string>();

            string query = "SELECT name FROM players";
            MySqlCommand cmd = new MySqlCommand(query, _conn);
            _conn.Open();

            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                string id = (string)rdr["name"];
                uNames.Add(id);
                Debug.WriteLine(id);
            }
            rdr.Close();
            _conn.Close();

            return uNames;
        }

        public Vector3 GetUserLastPosition(string userName)
        {
            Vector3 returnedPosition = new Vector3(0f,0f,0f);

            string query = "SELECT name, posX, posY, posZ FROM players WHERE name = '" + userName + "'";
            MySqlCommand cmd = new MySqlCommand(query, _conn);

            _conn.Open();

            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                returnedPosition = new Vector3((float)rdr["posX"], (float)rdr["posY"], (float)rdr["posZ"]);
            }
            rdr.Close();
            _conn.Close();

            return returnedPosition;
        }
    }
}
