using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.Native;

namespace FiveM_Online_Client
{
    class main : BaseScript
    {
        public main()
        {
            EventHandlers["playerSpawned"] += new Action(playerSpawned);
        }

        public void playerSpawned()
        {
            TriggerServerEvent("savePlayer");
        }
    }
}
