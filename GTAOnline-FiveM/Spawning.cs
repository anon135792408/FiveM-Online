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
