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
            Tick += OnTick;
            EventHandlers.Add("playerConnecting", new Action<Player>(OnPlayerSpawned));
            EventHandlers.Add("playerDropped", new Action<Player>(OnPlayerDropped));
        }

        private async Task OnTick()
        {
            await Delay(0);
        }

        private void OnPlayerSpawned([FromSource] Player source)
        {
            Debug.WriteLine(source.Name + " has connected to the server from " + source.EndPoint);
            TriggerClientEvent(source, "chatMessage", new[] { 255, 0, 0 }, motd);
            TriggerClientEvent("GTAO:AlertPlayerJoined", source.Name);
        }

        private void OnPlayerDropped([FromSource] Player source)
        {
            Debug.WriteLine(source.Name + " has left the server");
            TriggerClientEvent(source, "chatMessage", new[] { 255, 0, 0 }, motd);
            TriggerClientEvent("GTAO:AlertPlayerLeft", source.Name);
        }
    }
}
