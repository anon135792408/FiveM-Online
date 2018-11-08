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
            EventHandlers.Add("playerSpawned", new Action<Vector3>(OnPlayerSpawned));
            EventHandlers.Add("GTAO:showNotification", new Action<String>(showNotification));
            EventHandlers.Add("GTAO:switchInLocalPlayer", new Action(SwitchInLocalPlayer));
            EventHandlers.Add("GTAO:switchOutLocalPlayer", new Action(SwitchOutLocalPlayer));
        }

        private async void OnPlayerSpawned([FromSource]Vector3 pos)
        {
            //SwitchOutLocalPlayer();
            await Delay(2000);
            //SwitchInLocalPlayer();
        }

        private void showNotification(String text)
        {
            Screen.ShowNotification(text);
        }

        private async void SwitchOutLocalPlayer()
        {
            DoScreenFadeOut(500);
            while (!Game.PlayerPed.Exists())
            {
                await Delay(0);
            }
            SwitchOutPlayer(PlayerPedId(), 1, 1);
        }

        private async void SwitchInLocalPlayer()
        {
            DoScreenFadeIn(500);
            while (IsPlayerSwitchInProgress())
            {
                await Delay(0);
            }
            SwitchInPlayer(PlayerPedId());
        }
    }
}
