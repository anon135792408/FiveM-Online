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
    class DynamicRoadWorks : BaseScript
    {
        Ped worker1;
        Ped worker2;

        bool pedsAdded = false;

        public DynamicRoadWorks()
        {
            Tick += OnTick;
        }

        private async Task OnTick()
        {
            await Delay(0);
            if (NetworkIsHost())
            {
                if (HasForceCleanupOccurred(18))
                {
                    RemovePeds();
                }

                if (GetClockHours() >= 8 || GetClockHours() <= 18)
                {
                    if (!pedsAdded)
                    {
                        AddPeds();
                        pedsAdded = true;
                    }
                }
                else
                {
                    if (pedsAdded)
                    {
                        RemovePeds();
                        pedsAdded = false;
                    }
                }
            }
        }

        private void RemovePeds()
        {
            worker1.Delete();
            worker2.Delete();
        }

        private async void AddPeds()
        {
            worker1 = await World.CreatePed(PedHash.Construct01SMY, new Vector3(765.43f, -1731.76f, 29.26f), 281.75f);
            worker2 = await World.CreatePed(PedHash.Construct02SMY, new Vector3(761.93f, -1732.79f, 29.41f), 309.14f);

            worker1.Task.PlayAnimation("amb@world_human_const_drill@male@drill@base", "base");
            worker2.Task.PlayAnimation("amb@world_human_clipboard@male@idle_a", "idle_a");
        }
    }
}
