using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace GTAOnlineShared
{
    class ScreenEvents : BaseScript
    {
        public ScreenEvents()
        {
            EventHandlers.Add("ScreenFade", new Action<int, int>(ScreenFade));
        }

    }
}
