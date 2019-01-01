using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace GTAOnline_FiveM
{
    public class GamePlayer : BaseScript
    {
        public static bool isCutsceneActive = false;

        public GamePlayer()
        {
            Tick += OnTick;
        }

        private async Task OnTick()
        {
            if(isCutsceneActive)
            {
                HideHudAndRadarThisFrame();
            }
        }
    }
}
