using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace GTAOnline_FiveM {
    class UserHandler : BaseScript {
        Player localPlayer;

        public UserHandler() {
            EventHandlers.Add("playerSpawned", new Action(OnPlayerSpawned));
        }

        public void OnPlayerSpawned() {
            localPlayer = new Player();
            localPlayer.Name = GetPlayerName(PlayerId());
        }
    }
}
