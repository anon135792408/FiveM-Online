using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace GTAOnline_FiveM {
    class ImpoundSpace {
        public ImpoundSpace(Vector3 coords, float heading, bool isTaken, int playerHandle, int vehicleHandle) {
            Coords = coords;
            Heading = heading;
            IsTaken = isTaken;
            PlayerHandle = playerHandle;
            VehicleHandle = vehicleHandle;
        }

        public Vector3 Coords { get; set; }

        public float Heading { get; set; }

        public bool IsTaken { get; set; }

        public int PlayerHandle { get; set; }

        public int VehicleHandle { get; set; }
    }
}
