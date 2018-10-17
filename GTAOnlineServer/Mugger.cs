using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace GTAOnlineClient
{
    class Mugger : BaseScript
    {
        public Mugger()
        {
            API.RegisterCommand("mug", new Action<int, List<object>, string>((source, arguments, raw) =>
            {
                int targetId;
                for (var i = 0; i < 32; i++)
                {
                    if (arguments.Count > 0)
                    {
                        if (API.GetPlayerName(i.ToString()) == arguments[0].ToString())
                        {
                            targetId = i;
                            TriggerClientEvent(new PlayerList()[targetId], "SetMuggerOnPlayerPed", targetId);
                        }
                    }
                }
            }), false);
        }
    }
}
