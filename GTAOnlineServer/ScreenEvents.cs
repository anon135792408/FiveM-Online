using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace GTAOnlineShared
{
    class ScreenEvents : BaseScript
    {
        public ScreenEvents()
        {
            EventHandlers.Add("ScreenFadeForPlayers", new Action<Player[], int, int>(ScreenFadeForPlayers));
        }

        private void ScreenFadeForPlayers(Player[] players, int time, int fadeType)
        {
            foreach (Player p in players)
            {
                TriggerClientEvent(p, "ScreenFade", time, fadeType);
            }
        }

    }
}
