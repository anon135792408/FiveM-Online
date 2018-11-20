using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace GTAOnline_Fivem_Server
{
    class Impound : BaseScript
    {
        public Impound()
        {
            EventHandlers.Add("GTAO:serverSyncImpoundSpaces", new Action<List<dynamic>>(ServerSendImpoundSpaces));
        }

        private void ServerSendImpoundSpaces(List<dynamic> impList)
        {
            TriggerClientEvent("GTAO:clientSyncImpoundSpaces", impList);
        }
    }
}
