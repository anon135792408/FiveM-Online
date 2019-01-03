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
            new PedPos(new Vector3(24.47f, -1347.39f, 29.5f), 264.72f),
            new PedPos(new Vector3(-46.88f, -1757.84f, 29.42f), 48.98f),
            new PedPos(new Vector3(1134.21f, -982.44f, 46.42f), 280.57f),
            new PedPos(new Vector3(-706.08f, -913.53f, 19.22f), 91.31f),
            new PedPos(new Vector3(-1221.92f, -908.35f, 12.33f), 30.66f)
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
                await Delay(500);
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
                    await Delay(500);
                    if (p.IsDead)
                    {
                        CashieerList.Remove(p);
                        SpawnCashieers();
                        break;
                    }
                }
            }
            await Delay(60000);
        }
    }
}
