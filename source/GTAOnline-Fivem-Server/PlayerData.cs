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
            EventHandlers.Add("GTAO:SavePlayerData", new Action<int, string>(SavePlayerData));
            _conn = new MySqlConnection(_connStr);
        }

        public void SavePlayerData(int id, string name)
        {
            _conn.Open();
            
            Debug.WriteLine(_connStr);

            Debug.WriteLine(_conn.ServerVersion);

            _conn.Close();
            /*string query = "INSERT INTO players(id, name) VALUES(" + id + ", '" + name + "')";
            MySqlCommand cmd = new MySqlCommand(query, _conn);
            _conn.Open();
            //cmd.ExecuteNonQuery();
            _conn.Close();*/
        }
    }
}
