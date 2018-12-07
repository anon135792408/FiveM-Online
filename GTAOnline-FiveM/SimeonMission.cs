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
        private Vector3 SIMEON_DROPOFF = new Vector3(1204.43f, -3116.04f, 5.54f);
        private bool missionActive = false;
        static Random rnd = new Random();
        Vehicle missionVehicle;
        Blip simBlip;

        public SimeonMission() {
            EventHandlers.Add("GTAO:DisplaySimeonMarkerForAll", new Action(DisplaySimeonMarker));
            EventHandlers.Add("GTAO:ClearSimeonMarkerForAll", new Action(ClearSimeonMarker));
            EventHandlers.Add("GTAO:EndMissionForAll", new Action(EndMission));
            EventHandlers.Add("GTAO:StartMissionForAll", new Action<int>(StartMission));
            EventHandlers.Add("GTAO:SimeonMissionFadeOutIn", new Action(SimeonMissionFadeOutIn));
            Tick += OnTick;    
        }

        private async Task OnTick() {
            await Delay(100);
            if (!missionActive && NetworkIsHost()) {
                Tuple<Vector3, float> randPos = GetRandomPosition();
                RequestCollisionAtCoord(randPos.Item1.X, randPos.Item1.Y, randPos.Item1.Z);
                missionVehicle = await World.CreateVehicle(GetRandomVehHash(), randPos.Item1, randPos.Item2);
                missionVehicle.PlaceOnGround();

                TriggerServerEvent("GTAO:DisplaySimeonMarkerForAll");
                TriggerServerEvent("GTAO:StartMissionForAll", NetworkGetNetworkIdFromEntity(missionVehicle.Handle));

                float x = 0.0f;
                float y = 0.0f;
                bool onScreen = GetScreenCoordFromWorldCoord(randPos.Item1.X, randPos.Item1.Y, randPos.Item1.Z, ref y, ref x);
                
                Debug.WriteLine(x + " " + y);
            }

            await Delay(15000);
        }


        private async Task MissionTick() {
            await Delay(100);
            if (missionActive) {
                if (missionVehicle.IsDead) {
                    missionActive = false;
                    TriggerServerEvent("GTAO:EndMissionForAll");
                }

                if (Game.PlayerPed.IsInRangeOf(SIMEON_DROPOFF, 7.0f) && missionVehicle.Driver == Game.PlayerPed) {

                    for (int i = 0; i < missionVehicle.Passengers.Length; i++) { // Seriously, am I missing something?
                        int pHandle = GetPlayerServerId(NetworkGetPlayerIndexFromPed(missionVehicle.Passengers[i].Handle));
                        Debug.WriteLine(Players[pHandle].Name);
                        TriggerServerEvent("GTAO:SimeonMissionFadeOutIn", pHandle);
                    }

                    while (missionVehicle.Passengers.Count() > 0) {
                        await Delay(0);
                    }

                    missionActive = false;
                    missionVehicle.Delete();
                }
            } else {
                TriggerServerEvent("GTAO:EndMissionForAll");
            }
        }

        private async void SimeonMissionFadeOutIn() {
            Screen.Fading.FadeOut(500);
            while (Screen.Fading.IsFadingOut) {
                await Delay(0);
            }
            Game.PlayerPed.Task.LeaveVehicle();
            await Delay(1750);

            Screen.Fading.FadeIn(500);
        }

        private void EndMission() {
            TriggerServerEvent("GTAO:ClearSimeonMarkerForAll");
            missionActive = false;
            missionVehicle = null;
            Tick -= MissionTick;
        }

        private void StartMission(int netid) {
            missionVehicle = new Vehicle(NetworkGetEntityFromNetworkId(netid));
            SetEntityAsMissionEntity(missionVehicle.Handle, true, true);
            missionActive = true;
            Tick += MissionTick;
            AttachBlipToMissionEntity();
        }

        private void AttachBlipToMissionEntity() {
            Blip b = missionVehicle.AttachBlip();
            b.Sprite = BlipSprite.PersonalVehicleCar;
            b.Color = BlipColor.Yellow;
            b.Name = "Simeon's Wanted Asset";
        }

        private VehicleHash GetRandomVehHash() {
            return SimeonMissionData.wantedVehicles[rnd.Next(SimeonMissionData.wantedVehicles.Count)];
        }

        private void DisplaySimeonMarker() {
            simBlip = World.CreateBlip(SIMEON_DROPOFF);
            simBlip.Alpha = 255;
            simBlip.Sprite = BlipSprite.Solomon;
            simBlip.Color = BlipColor.Yellow;
            simBlip.Name = "Simeon Drop Off";
            simBlip.IsShortRange = false;
        }

        private void ClearSimeonMarker() {
            simBlip.Alpha = 0;
            simBlip.Color = BlipColor.Yellow;
            simBlip.IsShortRange = false;
        }

        private Tuple<Vector3, float> GetRandomPosition() {
            return Tuple.Create(SimeonMissionData.vehicleLocations.ElementAt(rnd.Next(SimeonMissionData.vehicleLocations.Count)).Key, SimeonMissionData.vehicleLocations.ElementAt(rnd.Next(SimeonMissionData.vehicleLocations.Count)).Value);
        }

        private void DrawNotification(String message)
        {
            SetNotificationMessage_2("CHAR_SIMEON", 1, true, 1, true, "Simeon", "Vehicle Asset"); //Potentially buggy, don't have the required integers for picName2 and iconType
            DrawNotification_2(false, true);
        }
    }
}
