using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace GTAOnline_Fivem_Server
{
    public class Spawning : BaseScript
    {
        public static string motd = "Welcome to Adam's FiveM server!";

        public Spawning()
        {
            EventHandlers.Add("playerConnecting", new Action<Player, string, CallbackDelegate>(OnPlayerConnecting));
        }

        private void OnPlayerConnecting([FromSource] Player player, string playerName, CallbackDelegate kickCallback)
        {
            TriggerClientEvent(player, "GTAO:displayNotification", motd);
        }
    }
}
