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
    public class Spawning : BaseScript
    {
        public Spawning()
        {
            EventHandlers.Add("GTAO:displayNotification", new Action<string>(DisplayNotification));
            EventHandlers.Add("playerSpawned", new Action(OnPlayerSpawned));
            Tick += OnTick;
        }

        private async Task OnTick()
        {
            while (!Game.PlayerPed.Exists())
            {
                Screen.Fading.FadeOut(0);
                await Delay(0);
            }
        }

        private async void OnPlayerSpawned()
        {
            Tick -= OnTick;
            StartPlayerSwitch(PlayerPedId(), PlayerPedId(), 1024, 2);

            while (GetPlayerSwitchState() != 3)
            {
                await Delay(0);
            }
            Screen.Fading.FadeIn(1000);
        }

        private async void DisplayNotification(string msg)
        {
            while(!Game.PlayerPed.Exists())
            {
                await Delay(0);
            }
            Screen.ShowNotification(msg);
        }
    }
}
