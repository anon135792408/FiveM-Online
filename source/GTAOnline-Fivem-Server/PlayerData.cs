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
        private readonly string _connStr = @"server=127.0.0.1;port=3306;database=fivemonline;uid=root;pwd=;sslmode=none;";

        MySqlConnection _conn;

        public PlayerData()
        {
            EventHandlers.Add("GTAO:SavePlayerData", new Action<Player>(SavePlayerData));
            EventHandlers.Add("GTAO:UpdatePlayerCash", new Action<Player, long>(UpdatePlayerCash));
            EventHandlers.Add("GTAO:CheckIfPlayerExistsInDatabase", new Action<Player>(CheckIfPlayerExistsInDatabase));
            _conn = new MySqlConnection(_connStr);
        }

        public void SavePlayerData([FromSource]Player player)
        {
            string uName = player.Name;
            try
            {
                if (!DoesPlayerExistInDatabase(player))
                {
                    string uId = (string)player.Identifiers["license"];

                    Debug.WriteLine("==========Saving===========");
                    Debug.WriteLine("Name: " + uName + "\nIdentifier: " + uId);
                    Debug.WriteLine("===========================");

                    //Player p = new PlayerList()[id];
                    //string identifier = p.Identifiers["license"].ToString();

                    string query = String.Format("INSERT INTO users(id, username) VALUES('{0}', '{1}')", uId, uName);
                    MySqlCommand cmd = new MySqlCommand(query, _conn);

                    _conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to save player data for: " + uName + "\n Error: " + ex.Message);
                _conn.Close();
            }
            _conn.Close();
        }

        public void UpdatePlayerCash([FromSource]Player player, long cash)
        {
            string uName = player.Name;
            try
            {
                if (DoesPlayerExistInDatabase(player))
                {
                    string uId = (string)player.Identifiers["license"];

                    Debug.WriteLine("=========Updating==========");
                    Debug.WriteLine("Name: " + uName + "\nCash: " + cash.ToString());
                    Debug.WriteLine("===========================");
 

                    string query = String.Format("UPDATE users SET cash='{0}' WHERE id='{1}'", cash, uId);
                    MySqlCommand cmd = new MySqlCommand(query, _conn);

                    _conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update player cash for: " + uName + "\n Error: " + ex.Message);
                _conn.Close();
            }
            _conn.Close();
        }

        public void CheckIfPlayerExistsInDatabase([FromSource]Player player)
        {
            bool result = DoesPlayerExistInDatabase(player);
            Debug.WriteLine(result.ToString());
            player.TriggerEvent("GTAO:SyncPlayerExistBool", result);
        }

        public bool DoesPlayerExistInDatabase([FromSource]Player player)
        {
            int rowCount = 0;
            string uName = player.Name;

            try
            {


                string query = "SELECT * FROM users WHERE username = '" + uName + "'";
                MySqlCommand cmd = new MySqlCommand(query, _conn);

                _conn.Open();

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string rdrName = (string)rdr["username"];
                    if (rdrName.Equals(uName))
                    {
                        rowCount++;
                    }
                }
                rdr.Close();
                _conn.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to find player data for: " + uName + "\n Error: " + ex.Message);
                _conn.Close();
            }

            if (rowCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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

        public IList<string> GetAllUsers()
        {
            IList<string> uNames = new List<string>();

            try
            {
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to retrieve all database records \n Error: " + ex.Message);
                _conn.Close();
            }

            return uNames;
        }

        public Vector3 GetUserLastPosition(string userName)
        {
            Vector3 returnedPosition = new Vector3(0f, 0f, 0f);

            try
            {

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
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to retrievelast position for:" + userName + "\n Error: " + ex.Message);
                _conn.Close();
            }

            return returnedPosition;
        }
    }
}
