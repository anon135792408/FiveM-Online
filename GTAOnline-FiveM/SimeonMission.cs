using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using NativeUI;

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
                VehToNet(missionVehicle.Handle);
                missionVehicle.PlaceOnGround();
                missionVehicle.LockStatus = VehicleLockStatus.CanBeBrokenIntoPersist;
                missionVehicle.IsAlarmSet = true;

                TriggerServerEvent("GTAO:DisplaySimeonMarkerForAll");
                TriggerServerEvent("GTAO:StartMissionForAll", NetworkGetNetworkIdFromEntity(missionVehicle.Handle));
            }

            await Delay(15000);
        }


        private async Task MissionTick() {
            await Delay(100);
            if (missionActive) {
                if (missionVehicle.IsDead && NetworkIsHost()) {
                    TriggerServerEvent("GTAO:EndMissionForAll");
                    TriggerServerEvent("GTAO:ClearSimeonMarkerForAll");
                }

                if (Game.PlayerPed.IsInRangeOf(SIMEON_DROPOFF, 7.0f) && missionVehicle.Driver == Game.PlayerPed) {
                    TriggerServerEvent("GTAO:SimeonMissionFadeOutIn", Game.Player.ServerId);

                    for (int i = 0; i < missionVehicle.Passengers.Count(); i++) {
                        int pHandle = GetPlayerServerId(NetworkGetPlayerIndexFromPed(missionVehicle.Passengers[i].Handle));
                        TriggerServerEvent("GTAO:SimeonMissionFadeOutIn", pHandle);
                    }

                    missionVehicle.MaxSpeed = 0;
                    while (!missionVehicle.IsStopped) {
                        await Delay(0);
                    }

                    while (missionVehicle.Passengers.Count() > 0) {
                        await Delay(0);
                    }

                    TriggerServerEvent("GTAO:EndMissionForAll");
                    TriggerServerEvent("GTAO:ClearSimeonMarkerForAll");
                    PlayMissionCompleteAudio("FRANKLIN_BIG_01");
                }
            }
        }

        private async void SimeonMissionFadeOutIn() {
            BigMessageThread.MessageInstance.ShowMissionPassedMessage("Mission Passed", 5000);
            Screen.Fading.FadeOut(500);
            while (Screen.Fading.IsFadingOut) {
                await Delay(0);
            }
            Game.PlayerPed.Task.LeaveVehicle();
            while (Game.PlayerPed.IsSittingInVehicle()) {
                await Delay(0);
            }
            Game.PlayerPed.PositionNoOffset = World.GetNextPositionOnSidewalk(new Vector2(1199.64f, -3065.44f));
            await Delay(1750);

            Screen.Fading.FadeIn(500);
        }

        private void EndMission() {
            NetworkFadeOutEntity(missionVehicle.Handle, true, false);
            missionActive = false;
            missionVehicle.AttachedBlip.Delete();
            missionVehicle.MarkAsNoLongerNeeded();
            missionVehicle.IsPersistent = false;
            SetAggressiveHorns(false);
            Tick -= MissionTick;
        }

        private void StartMission(int netid) {
            missionVehicle = new Vehicle(NetworkGetEntityFromNetworkId(netid));
            SetEntityAsMissionEntity(missionVehicle.Handle, true, true);
            missionActive = true;
            SetAggressiveHorns(true);
            Tick += MissionTick;
            AttachBlipToMissionEntity();

            string locName = missionVehicle.LocalizedName;
            Debug.WriteLine(locName);
            if ("aeiouAEIOU".Contains(locName[0]))
            {
                DrawSimeonNotification("An " + locName + " has been spotted in " + World.GetStreetName(missionVehicle.Position) + ", go and pick it up for me.");
            } else
            {
                DrawSimeonNotification("A " + locName + " has been spotted in " + World.GetStreetName(missionVehicle.Position) + ", go and pick it up for me.");
            }
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
            simBlip.Delete();
        }

        private Tuple<Vector3, float> GetRandomPosition() {
            int index = rnd.Next(SimeonMissionData.vehicleLocations.Count);
            return Tuple.Create(SimeonMissionData.vehicleLocations.ElementAt(index).Key, SimeonMissionData.vehicleLocations.ElementAt(index).Value);
        }

        private async void DrawSimeonNotification(string message)
        {
            while (!NetworkIsPlayerActive(PlayerId()) || IsPlayerSwitchInProgress()) {
                await Delay(0);
            }
            Debug.WriteLine("Passing message: " + message);
            SetNotificationTextEntry("STRING");
            AddTextComponentString(message);
            SetNotificationMessage("CHAR_SIMEON", "CHAR_SIMEON", true, 1, "Simeon", "Vehicle Asset");
            DrawNotification(false, true);
        }
    }
}
