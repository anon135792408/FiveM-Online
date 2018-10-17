using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;

namespace GTAOnlineClient
{
    class Debug : BaseScript
    {
        public Debug()
        {
            EventHandlers.Add("fadeIn", new Action(FadeIn));
            EventHandlers.Add("switchIn", new Action(SwitchIn));
        }

        public void FadeIn()
        {
            Screen.Fading.FadeIn(500);
        }

        public void SwitchIn()
        {
            StartPlayerSwitch(Game.PlayerPed.Handle, Game.PlayerPed.Handle, 1, 1);
        }
    }
}
