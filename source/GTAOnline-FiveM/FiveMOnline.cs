using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using GTAOnline_FiveM;

namespace FiveM_Online_Client
{
    class FiveMOnline
    {
        public static GamePlayer onlinePlayer;

        public FiveMOnline()
        {
            onlinePlayer = new GamePlayer();
        }
    }
}
