using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using NativeUI;

namespace FiveM_Online_Client
{
    class Cashieers : BaseScript
    {
        public PedPos[] CashieerPositions = new PedPos[]
        {
            new PedPos(new Vector3(24.47f, -1347.39f, 29.5f), 264.72f)
        };

        public List<Ped> CashieerList = new List<Ped>();

        public Cashieers()
        {
            if (NetworkIsHost())
            {
                SpawnCashieers();
            }
            Tick += StatusCheck;
        }

        public async void SpawnCashieers()
        {
            int closestPed = -1;
            foreach (PedPos pedPos in CashieerPositions)
            {
                GetClosestPed(pedPos.Position.X, pedPos.Position.Y, pedPos.Position.Z, 1f, true, true, ref closestPed, true, true, -1);

                if (DoesEntityExist(closestPed))
                {
                    if (IsPedDeadOrDying(closestPed, true))
                        CashieerList.Add(await World.CreatePed(PedHash.ShopKeep01, pedPos.Position, pedPos.Heading));
                }
                else
                {
                    CashieerList.Add(await World.CreatePed(PedHash.ShopKeep01, pedPos.Position, pedPos.Heading));
                }
            }
        }

        public async Task StatusCheck()
        {
            if (NetworkIsHost())
            {
                foreach (Ped p in CashieerList)
                {
                    if (p.IsDead)
                    {
                        CashieerList.Remove(p);
                        SpawnCashieers();
                    }
                }
            }
            await Delay(60000);
        }
    }
}
