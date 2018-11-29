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
        private Vehicle veh;
        private Vector3 lastPos;
        private float heading;

        public PersonalVehicle(Vehicle veh, Vector3 lastPos, float heading) {
            this.veh = veh;
            this.lastPos = lastPos;
            this.heading = heading;
        }

        public Vehicle Veh {
            get { return veh; }
        }

        public Vector3 LastPosition {
            get { return lastPos; }
            set { lastPos = value; }
        }

        public float Heading {
            get { return heading; }
            set { heading = value; }
        }
    }
}
