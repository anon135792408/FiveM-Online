using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace GTAOnline_FiveM {
    class Player {
        private Vector3 lastPos;
        private float heading;

        private long money;
        private List<PersonalVehicle> personalVehicles;
        private long xp;

        public Player() {
            lastPos = new Vector3(30.18f, -723.04f, 44.19f);
            heading = 248.17f;
            money = 5000;
            personalVehicles = new List<PersonalVehicle>();
            xp = 0;
        }

        public Vector3 LastPosition {
            get { return lastPos; }
            set { lastPos = value; }
        }

        public float Heading {
            get { return heading; }
            set { heading = value; }
        }

        public long Money {
            get { return money; }
            set { money = value; }
        }

        public long Xp {
            get { return xp; }
            set { xp = value; }
        }

        public List<PersonalVehicle> GetPlayerPersonalVehicles() {
            return personalVehicles;
        }

        public void AddPersonalVehicle(Vehicle v) {
            PersonalVehicle tempVeh = new PersonalVehicle(v, v.Position, v.Heading);
            personalVehicles.Add(tempVeh);
        }

        public void DeletePersonalVehicle(PersonalVehicle pv) {
            personalVehicles.Remove(pv);
        }

        public void SavePlayerData() {
            //Code here
        }
    }
}
