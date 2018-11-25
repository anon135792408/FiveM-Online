using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace GTAOnline_FiveM {
    class VehicleProperties : BaseScript {
        public VehicleProperties() {
            EventHandlers.Add("GTAO:clientToggleLocalPlayerVehicleEngine", new Action(ToggleLocalPlayerVehicleEngine));
        }

        private void ToggleLocalPlayerVehicleEngine() {
            if (Game.PlayerPed.CurrentVehicle != null && Game.PlayerPed.CurrentVehicle.Driver == Game.PlayerPed) {
                Vehicle v = Game.PlayerPed.CurrentVehicle;
                v.IsEngineRunning = !v.IsEngineRunning;
            }
        }
    }
}
