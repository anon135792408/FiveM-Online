using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace GTAOnline_FiveM {
    class PersonalVehicle {
        private VehicleHash modelHash;
        private Vector3 position;
        private float heading;
        private VehicleColor color;

        public PersonalVehicle() {
            modelHash = VehicleHash.Asterope;
            position = World.GetNextPositionOnStreet(new Vector2(0, 0), true);
            heading = 90.0f;
            color = VehicleColor.MetallicSilver;
        }

        public VehicleHash ModelHash { get => modelHash; set => modelHash = value; }
        public Vector3 Position { get => position; set => position = value; }
        public float Heading { get => heading; set => heading = value; }
        public VehicleColor Color { get => color; set => color = value; }
    }
}
