using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using FiveM_Online_Server;
using static CitizenFX.Core.Native.API;

namespace GTAOnline_Fivem_Server
{
    class PlayerData : BaseScript
    {
        public PlayerData()
        {
            EventHandlers.Add("GTAO:SavePlayerData", new Action<int, string>(SavePlayerData));
        }

        public void SavePlayerData(int id, string name)
        {
            SqlConnector.SavePlayer(id, name);
        }
    }
}
