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
    class Shops : BaseScript
    {
        IList<dynamic> shopPeds = new List<dynamic>();
        bool firstTick = true;

        public Shops()
        {
            EventHandlers.Add("GTAO:clientSyncShopPedList", new Action<List<dynamic>>(SyncShopPedList));
            Tick += OnTick;
            CreatePeds();
        }

        private async Task OnTick()
        {
            CreatePeds();
        }

        private async void SyncShopPedList(List<dynamic> shopPeds)
        {
            this.shopPeds = shopPeds;
            Debug.WriteLine("Received PedList count: " + shopPeds.Count());
        }

        private async void CreatePeds()
        {
            if (NetworkIsHost() && firstTick && shopPeds.Count < 1)
            {
                while (!Game.PlayerPed.Exists())
                {
                    await Delay(0);
                }
                Ped x = await World.CreatePed(PedHash.ShopLowSFY, new Vector3(73.88f, -1392.80f, 29.39f), 263.72f);
                shopPeds.Add(x.Handle);
                Tick += CheckPedStatus;
                firstTick = false;
            }
        }

        private async Task CheckPedStatus()
        {
            Ped p;
            if (shopPeds.Count > 0)
            {
                for (int i = 0; i < shopPeds.Count; i++)
                {
                    await Delay(100);
                    p = new Ped(shopPeds[i]);
                    if (p.IsDead)
                    {
                        p.IsPersistent = true;
                        shopPeds.RemoveAt(i);

                        TriggerServerEvent("GTAO:serverSyncShopPedList", shopPeds);
                    }
                    p = null;
                }
            }
        }
    }
}
