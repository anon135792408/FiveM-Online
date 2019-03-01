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

        public Vector3 LastPosition { get; set; }

        public GamePlayer()
        {
            EventHandlers["savePlayer"] += new Action<Player, float, float, float>(savePlayer);
        }

        public void savePlayer([FromSource]Player player, float posX, float posY, float posZ)
        {
            string dir = "D:\\Games\\FiveMServer\\server-data\\resources\\FiveM-Online\\playerData\\" + License + ".json"; //This will take some improving
            
            if (File.Exists(dir))
            {
                using (StreamReader file = new StreamReader(dir))
                {
                    string json = file.ReadToEnd();
                    GamePlayer tempPlayer = JsonConvert.DeserializeObject<GamePlayer>(json);
                    License = tempPlayer.License;
                    UserName = tempPlayer.UserName;
                    LastIp = tempPlayer.LastIp;
                    LastPosition = tempPlayer.LastPosition;

                    Debug.WriteLine(LastPosition.ToString());

                    player.TriggerEvent("receiveData", LastPosition.X, LastPosition.Y, LastPosition.Z);
                }
            }
            else
            {
                License = player.Identifiers["license"];
                UserName = player.Name;
                LastIp = player.EndPoint;

                LastPosition = new Vector3(posX, posY, posZ);

                File.WriteAllText(dir, JsonConvert.SerializeObject(this));

                using (StreamWriter file = File.CreateText(dir))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, this);
                }
            }
        }
    }
}
