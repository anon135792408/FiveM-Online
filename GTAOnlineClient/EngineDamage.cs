using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace GTAOnlineClient
{
    class EngineDamage : BaseScript
    {
        bool isRedlining = false;
        int currTimer = 20000;
        Vehicle currVehicle;

        public EngineDamage()
        {
            Tick += OnTick;
            Tick += EngineCounter;
        }

        private async Task OnTick()
        {
            Ped playerPed = Game.PlayerPed;
            if (playerPed.IsInVehicle() && playerPed.CurrentVehicle.Driver == playerPed)
            {
                currVehicle = playerPed.CurrentVehicle;
                float engineRevs = currVehicle.CurrentRPM;
                if (engineRevs > 0.89)
                {
                    isRedlining = true;
                }
                else
                {
                    isRedlining = false;
                }
            }

            if (Game.Player.IsDead)
            {
                isRedlining = false;
            }

            if (currVehicle != null)
            {
                if (currVehicle.IsDead)
                {
                    isRedlining = false;
                }
            }
        }

        private async Task EngineCounter()
        {
            if (isRedlining)
            {
                currTimer--;
                //Screen.DisplayHelpTextThisFrame(currTimer.ToString() + " " + currVehicle.EngineHealth);
                if (currTimer == 100)
                {
                    currVehicle.EngineHealth -= 950;
                }
                else if (currTimer == -500)
                {
                    currVehicle.EngineHealth -= 100;
                }
            }
            else
            {
                currTimer = 20000;
            }
        }
    }
}
