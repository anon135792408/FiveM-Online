using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace GTAOnline_FiveM {
    class SimeonMission : BaseScript{
        private bool missionActive = false;
        static Random rnd = new Random();
        Vehicle missionVehicle;

        public SimeonMission() {
            Tick += OnTick;    
        }

        private async Task OnTick() {
            await Delay(100);
            if (!missionActive && NetworkIsHost()) {
                TriggerMission();
                Tick += MissionTick;
            }
        }

        private async Task MissionTick() {
            await Delay(100);
            if (missionActive) {
                if (missionVehicle.IsDead) {
                    missionActive = false;
                }
            } 
        }

        private async void TriggerMission() {
            Tuple<Vector3, float> randPos = GetRandomPosition();
            missionVehicle = await World.CreateVehicle(GetRandomVehHash(), randPos.Item1, randPos.Item2);
            

        }

        private VehicleHash GetRandomVehHash() {
            return SimeonMissionData.wantedVehicles[rnd.Next(SimeonMissionData.wantedVehicles.Count)];
        }

        private Tuple<Vector3, float> GetRandomPosition() {
            return Tuple.Create(SimeonMissionData.vehicleLocations.ElementAt(rnd.Next(SimeonMissionData.vehicleLocations.Count)).Key, SimeonMissionData.vehicleLocations.ElementAt(rnd.Next(SimeonMissionData.vehicleLocations.Count)).Value);
        }
    }
}
