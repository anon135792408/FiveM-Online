using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using System.IO;

namespace FiveM_Online_Server
{
    class GamePlayer : BaseScript
    {
        public string License { get; set; }
        public string UserName { get; set; }
        public string LastIp { get; set; }

        public GamePlayer()
        {
            EventHandlers["savePlayer"] += new Action<Player>(savePlayer);
        }

        public void savePlayer([FromSource]Player player)
        {
            License = player.Identifiers["license"];
            UserName = player.Name;
            LastIp = player.EndPoint;

            string dir = "D:\\Games\\FiveMServer\\server-data\\resources\\FiveM-Online\\playerData\\" + License + ".json"; //This will take some improving
            File.WriteAllText(dir, JsonConvert.SerializeObject(this));

            using (StreamWriter file = File.CreateText(dir))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, this);
            }
        }
    }
}
