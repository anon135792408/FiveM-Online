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
            PlayerList pl = new PlayerList();

            API.RegisterCommand("engine", new Action<int, List<object>, string>((source, arguments, raw) =>
            {
                Player p = pl[source];
                TriggerClientEvent(p, "GTAO:clientToggleVehicleEngine");
            }), false);
        }
    }
}
