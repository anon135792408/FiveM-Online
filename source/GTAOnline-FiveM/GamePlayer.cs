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
        }

        public GamePlayer(long cash, List<Apartment> apartments)
        {
            playerCash = cash;
            ownedApartments = apartments;
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
