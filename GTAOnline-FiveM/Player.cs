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
        private string name;
        private long cash;
        private List<PersonalVehicle> personalVehicles;

        public Player() {
            name = "NoName";
            cash = 25000;
            personalVehicles = new List<PersonalVehicle>();
        }

        public string Name { get => name; set => name = value; }
        public long Cash { get => cash; set => cash = value; }
        public List<PersonalVehicle> PersonalVehicles { get => personalVehicles; set => personalVehicles = value; }
    }
}
