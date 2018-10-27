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
            EventHandlers.Add("GTAO:AlertPlayerJoined", new Action<String>(AlertPlayerJoined));
            EventHandlers.Add("GTAO:AlertPlayerLeft", new Action<String>(AlertPlayerLeft));
        }

        private void AlertPlayerJoined(String playerName)
        {
            Screen.ShowNotification("~h~" + playerName + " ~s~joined.");
        }

        private void AlertPlayerLeft(String playerName)
        {
            Screen.ShowNotification("~h~" + playerName + " ~s~left.");
        }
    }
}
