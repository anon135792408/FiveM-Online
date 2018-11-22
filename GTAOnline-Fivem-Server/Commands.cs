using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace GTAOnline_Fivem_Server
{
    class Commands : BaseScript
    {
        public Commands()
        {
            API.RegisterCommand("engine", new Action<Player, List<object>, string>((source, arguments, raw) =>
            {
                TriggerClientEvent(source.Handle, "GTAO:clientToggleVehicleEngine");
            }), false);
        }
    }
}
