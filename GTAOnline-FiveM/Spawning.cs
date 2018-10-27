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
            EventHandlers.Add("GTAO:showNotification", new Action<String>(showNotification));
        }

        private void showNotification(String text)
        {
            Screen.ShowNotification(text);
        }

    }
}
