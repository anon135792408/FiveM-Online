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
            Tick += SpawnTransition;
            Tick += DeathCheck;
        }

        public async Task DeathCheck()
        {
            if (IsPlayerDead(PlayerId()))
            {
                Debug.WriteLine("Player died!");

                //Wasted screen

                while (IsPlayerDead(PlayerId())) {
                    await Delay(0);
                }
            }
        }

        public async Task SyncInterval()
        {
            await Delay(15000);
            TriggerServerEvent("savePlayer", Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z);
        }

        public async Task SpawnTransition()
        {
            DoScreenFadeOut(500);

            initTransition();

            while (GetPlayerSwitchState() != 5)
            {
                await Delay(0);
            }

            ShutdownLoadingScreen();
            DoScreenFadeOut(0);
            ShutdownLoadingScreenNui();

            DoScreenFadeIn(500);
            while (!IsScreenFadedIn())
            {
                await Delay(0);
            }

            var timer = GetGameTimer();

            while (true)
            {
                await Delay(0);
                if (GetGameTimer() - timer > 5000)
                {
                    SwitchInPlayer(PlayerPedId());

                    while (GetPlayerSwitchState() != 12)
                    {
                        await Delay(0);
                    }

                    break;
                }
            }

            while (IsPlayerDead(PlayerId()))
            {
                await Delay(0);
            }

            Tick -= SpawnTransition;
        }

        public async void playerSpawned()
        {
            await Delay(1000);
            TriggerServerEvent("getPlayerLastPosition", Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z);
        }

        public async void receiveData(float x, float y, float z)
        {
            if (firstConnect)
            {
                Game.PlayerPed.Position = World.GetNextPositionOnSidewalk(new Vector3(x, y, z));
                firstConnect = false;
            }
        }

        public async void initTransition()
        {
            SetManualShutdownLoadingScreenNui(true);

            if (!IsPlayerSwitchInProgress())
            {
                SwitchOutPlayer(PlayerPedId(), 0, 1);
            }
        }
    }
}
