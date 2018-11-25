using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace GTAOnline_Fivem_Server {
    class PlayerProperties : BaseScript {
        public PlayerProperties() {
            EventHandlers.Add("GTAO:serverSetPlayerWanted", new Action<Player, int>(SetPlayerWanted));
        }

        private void SetPlayerWanted(Player player, int wantedLevel) {
            TriggerClientEvent(player, "GTAO:clientSetPlayerWanted", wantedLevel);
        }
    }
}
