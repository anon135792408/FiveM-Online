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
            EventHandlers.Add("GTAO:serverSyncShopPedList", new Action<IList<dynamic>>(SyncShopPedList));
        }

        private void SyncShopPedList(IList<dynamic> shopPeds)
        {
            TriggerClientEvent("GTAO:clientSyncShopPedList", shopPeds);
            Debug.WriteLine("Server PedList count: " + shopPeds.Count());
        }
    }
}
