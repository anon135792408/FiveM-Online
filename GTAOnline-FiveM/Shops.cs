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
        Ped npc;
        public Shops()
        {
            Tick += OnTick;
            CreatePeds();
        }

        private async Task OnTick()
        {
            
        }

        private async void CreatePeds()
        {
            if (NetworkIsHost())
            {
                npc = await World.CreatePed(PedHash.ShopLowSFY, new Vector3(73.88f, -1392.80f, 29.39f), 263.72f);
                Tick += CheckPedStatus;
            }
        }

        private async Task CheckPedStatus()
        {
            if (npc.IsDead)
            {
                npc.IsPersistent = false;
                npc = null;
            }
        }
    }
}
