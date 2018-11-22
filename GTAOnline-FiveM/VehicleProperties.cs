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
    class VehicleProperties : BaseScript
    {
        public VehicleProperties()
        {
            EventHandlers.Add("GTAO:clientToggleVehicleEngine", new Action(TogglePlayerEngine));
        }

        private void TogglePlayerEngine()
        {
            if (Game.PlayerPed.CurrentVehicle != null && Game.PlayerPed.CurrentVehicle.Driver == Game.PlayerPed)
            {
                Game.PlayerPed.CurrentVehicle.IsEngineRunning = !Game.PlayerPed.CurrentVehicle.IsEngineRunning;
            }
        }
    }
}
