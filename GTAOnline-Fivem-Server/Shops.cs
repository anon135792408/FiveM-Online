using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace GTAOnline_Fivem_Server
{
    class Shops : BaseScript
    {
        public Shops()
        {
            EventHandlers.Add("GTAO:serverSyncShopPedList", new Action<List<object>>(SyncShopPedList));
        }

        private void SyncShopPedList(List<object> pedList)
        {
            TriggerClientEvent("GTAO:clientSyncShopPedList", pedList);
        }
    }
}
