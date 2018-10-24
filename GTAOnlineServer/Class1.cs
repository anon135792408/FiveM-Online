using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace GTAOnlineServer
{
    public class Class1 : BaseScript
    {
        public Class1()
        {
            Tick += OnTick;
        }

        public async Task OnTick()
        {
            await Delay(1000);
            string bValue = GTAOnlineShared.Class1.getBool();
        }
    }
}
