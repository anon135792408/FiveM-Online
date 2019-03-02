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
        bool firstConnect = true;

        public main()
        {
            EventHandlers["playerSpawned"] += new Action(playerSpawned);
            EventHandlers["receiveData"] += new Action<float,float,float>(receiveData);
            Tick += SyncInterval;
        }

        public async Task SyncInterval()
        {
            await Delay(15000);
            TriggerServerEvent("savePlayer", Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z);
        }

        public async void playerSpawned()
        {
            await Delay(1000);
            TriggerServerEvent("getPlayerLastPosition", Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z);
        }

        public void receiveData(float x, float y, float z)
        {
            Game.PlayerPed.Position = World.GetNextPositionOnSidewalk(new Vector3(x, y, z));
            firstConnect = false;
        }
    }
}
