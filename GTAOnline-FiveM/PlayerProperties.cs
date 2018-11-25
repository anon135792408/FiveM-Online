using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;

namespace GTAOnline_FiveM {
    class PlayerProperties : BaseScript {
        public PlayerProperties() {
            EventHandlers.Add("GTAO:clientSetPlayerWanted", new Action<int>(SetPlayerWanted));
        }

        private void SetPlayerWanted(int wantedLevel) {
            Game.Player.WantedLevel = wantedLevel;
        }
    }
}
