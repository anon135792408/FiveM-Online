﻿using System;
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

            if (player.Identifiers["license"] != String.Empty)
            {
                try
                {
                    using (StreamReader file = new StreamReader(dir))
                    {
                        string json = file.ReadToEnd();
                        GamePlayer tempPlayer = JsonConvert.DeserializeObject<GamePlayer>(json);
                        License = player.Identifiers["license"];
                        UserName = player.Name;
                        LastIp = player.EndPoint;
                        LastPosition = new Vector3(posX, posY, posZ);

                        Debug.WriteLine(LastPosition.ToString());

                        //player.TriggerEvent("receiveData", LastPosition.X, LastPosition.Y, LastPosition.Z);
                    }

                    File.WriteAllText(dir, JsonConvert.SerializeObject(this));

                    using (StreamWriter fileWrite = File.CreateText(dir))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(fileWrite, this);
                    }
                }
                catch
                {
                    Debug.WriteLine("Unable to open player data file: " + dir + ", retrying soon...");
                }
            }
        }
    }
}
