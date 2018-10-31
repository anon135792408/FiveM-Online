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
        public static String motd = "Welcome to Adam's FiveM server!";

        public Spawning()
        {
            EventHandlers.Add("playerSpawned", new Action<Player>(OnPlayerSpawned));
            EventHandlers.Add("playerConnecting", new Action<Player>(OnPlayerConnecting));
            EventHandlers.Add("playerDropped", new Action<Player>(OnPlayerDropped));
            EventHandlers.Add("baseevents:onPlayerDied", new Action<Player, string, Vector3>(OnPlayerDied)); // Currently not working
        }

        private void OnPlayerSpawned([FromSource] Player source)
        {
            Debug.WriteLine("[GTAO]" + source.Name + " has connected to the server from " + source.EndPoint);
            TriggerClientEvent(source, "chatMessage", new[] { 255, 0, 0 }, motd);
            TriggerClientEvent("GTAO:showNotification", "~h~" + source.Name + " ~s~joined.");
            TriggerClientEvent(source, "GTAO:switchInLocalPlayer");
        }

        private void OnPlayerConnecting([FromSource] Player source)
        {
            TriggerClientEvent(source, "GTAO:switchOutLocalPlayer");
        }

        private void OnPlayerDropped([FromSource] Player source)
        {
            Debug.WriteLine("[GTAO]" + source.Name + " has left the server");
            TriggerClientEvent("GTAO:showNotification", "~h~" + source.Name + " ~s~left.");
        }

        private void OnPlayerDied([FromSource] Player source, string reason, Vector3 pos)
        {
            Debug.WriteLine("[GTAO]" + source.Name + " has died");
            TriggerClientEvent("GTAO:showNotification", "~h~" + source.Name + " ~s~died.");
            Debug.WriteLine("AlertPlayerDied");
        }
    }
}
