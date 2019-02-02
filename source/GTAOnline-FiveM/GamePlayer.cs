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
        public static List<Apartment> ownedApartments;
        public long playerCash;

        public static bool isCutsceneActive = false;
        public static bool isInAnyVehicle = false;
        public static bool isPlayerDead = false;

        public GamePlayer(long cash, List<Apartment> ownedApartments)
        {
            Tick += CutsceneCheck;
            Tick += StatusCheck;
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
