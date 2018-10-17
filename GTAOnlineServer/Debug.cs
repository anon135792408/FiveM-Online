using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace GTAOnlineServer
{
    class Debug : BaseScript
    {
        private bool debuggingMode = false;

        public Debug()
        {
            if (debuggingMode)
            {
                API.RegisterCommand("fadein", new Action<int, List<object>, string>((source, arguments, raw) =>
                {
                    TriggerClientEvent(new PlayerList()[source], "fadeIn");
                }), false);

                API.RegisterCommand("switchin", new Action<int, List<object>, string>((source, arguments, raw) =>
                {
                    TriggerClientEvent(new PlayerList()[source], "switchIn");
                }), false);
            }
        }
    }
}
