using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using FiveM_Online_Client;

namespace GTAOnline_FiveM
{
    public class GamePlayer : BaseScript
    {
        public List<Apartment> ownedApartments;
        public long playerCash;

        public bool isCutsceneActive = false;
        public bool isInAnyVehicle = false;
        public bool isPlayerDead = false;

        public GamePlayer()
        {
            Tick += CutsceneCheck;
            Tick += StatusCheck;
        }

        public GamePlayer(long cash, List<Apartment> apartments)
        {
            playerCash = cash;
            ownedApartments = apartments;
        }

        private async Task StatusCheck()
        {
            if (IsPedDeadOrDying(PlayerPedId(), true))
            {
                isPlayerDead = true;
            }
            else
                isPlayerDead = false;

            if (IsPedInAnyVehicle(PlayerPedId(), true))
            {
                isInAnyVehicle = true;
            }
            else
                isInAnyVehicle = false;

            await Delay(500);
        }

        private async Task CutsceneCheck()
        {
            if(isCutsceneActive)
            {
                HideHudAndRadarThisFrame();
            }
        }
    }
}
