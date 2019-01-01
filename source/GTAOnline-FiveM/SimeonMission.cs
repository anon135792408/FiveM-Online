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
        private bool isVehicleDead = false;

        public SimeonMission() {
            EventHandlers.Add("GTAO:DisplaySimeonMarkerForAll", new Action(DisplaySimeonMarker));
            EventHandlers.Add("GTAO:ClearSimeonMarkerForAll", new Action(ClearSimeonMarker));
            EventHandlers.Add("GTAO:EndMissionForAll", new Action(EndMission));
            EventHandlers.Add("GTAO:StartMissionForAll", new Action<int>(StartMission));
            EventHandlers.Add("GTAO:SimeonMissionFadeOutIn", new Action(SimeonMissionFadeOutIn));
            Tick += OnTick;    
        }

        private async Task OnTick() {
            await Delay(15000);

            if (!missionActive && NetworkIsHost()) {
                Tuple<Vector3, float> randPos = GetRandomPosition();
                RequestCollisionAtCoord(randPos.Item1.X, randPos.Item1.Y, randPos.Item1.Z);
                missionVehicle = await World.CreateVehicle(GetRandomVehHash(), randPos.Item1, randPos.Item2);
                await Delay(2000);
                missionVehicle.PlaceOnGround();
                missionVehicle.LockStatus = VehicleLockStatus.CanBeBrokenInto;
                missionVehicle.IsAlarmSet = true;
                missionVehicle.IsPersistent = true;

                SetVehicleHasBeenOwnedByPlayer(missionVehicle.Handle, true);
                var net_id = NetworkGetNetworkIdFromEntity(missionVehicle.Handle);
                SetNetworkIdCanMigrate(net_id, true);
                SetNetworkIdExistsOnAllMachines(net_id, true);
                NetworkFadeOutEntity(missionVehicle.Handle, true, false);
                await Delay(5000);
                TriggerServerEvent("GTAO:StartMissionForAll", net_id);
                TriggerServerEvent("GTAO:DisplaySimeonMarkerForAll");
                missionActive = true;
                NetworkFadeInEntity(missionVehicle.Handle, true);
            }
        }

        private async Task MissionTick() {
            await Delay(500);
            if (missionActive) {
                if ((missionVehicle.IsDead || (missionVehicle.IsUpsideDown && missionVehicle.Driver == null)) && NetworkIsHost()) {
                    TriggerServerEvent("GTAO:EndMissionForAll");
                    TriggerServerEvent("GTAO:ClearSimeonMarkerForAll");
                }

                if (Game.PlayerPed.IsInRangeOf(SIMEON_DROPOFF, 7.0f) && missionVehicle.Driver == Game.PlayerPed) {
                    //missionVehicle.MaxSpeed = 0;
                    SetVehicleHalt(missionVehicle.Handle, 3.0f, 1, false);
                    while (!missionVehicle.IsStopped) {
                        await Delay(100);
                    }

                    TriggerServerEvent("GTAO:SimeonMissionFadeOutIn", Game.Player.ServerId);

                    for (int i = 0; i < missionVehicle.Passengers.Count(); i++) {
                        int pHandle = GetPlayerServerId(NetworkGetPlayerIndexFromPed(missionVehicle.Passengers[i].Handle));
                        TriggerServerEvent("GTAO:SimeonMissionFadeOutIn", pHandle);
                    }

                    while (missionVehicle.Passengers.Count() > 0) {
                        await Delay(100);
                    }

                    TriggerServerEvent("GTAO:EndMissionForAll");
                    TriggerServerEvent("GTAO:ClearSimeonMarkerForAll");
                    PlayMissionCompleteAudio("FRANKLIN_BIG_01");
                }
            }
        }

        private async Task VehicleStatusCheck() {
            isVehicleDead = missionVehicle.IsDead;
            await Delay(500);
        }

        private async void SimeonMissionFadeOutIn() {
            Screen.Fading.FadeOut(500);
            while (Screen.Fading.IsFadingOut) {
                await Delay(100);
            }
            Game.PlayerPed.Task.LeaveVehicle();
            while (Game.PlayerPed.IsSittingInVehicle()) {
                await Delay(100);
            }
            BigMessageThread.MessageInstance.ShowMissionPassedMessage("Mission Passed", 5000);
            Game.PlayerPed.PositionNoOffset = World.GetNextPositionOnSidewalk(new Vector2(1199.64f, -3065.44f));
            await Delay(1750);

            Screen.Fading.FadeIn(500);
        }

        private void EndMission() {
            if (NetworkIsHost() && missionVehicle != null) {
                Delay(1250);
                NetworkFadeOutEntity(missionVehicle.Handle, true, false);
            }
            missionActive = false;
            missionVehicle.AttachedBlip.Delete();
            missionVehicle.MarkAsNoLongerNeeded();
            missionVehicle.IsPersistent = false;
            SetAggressiveHorns(false);
            Tick -= MissionTick;
        }

        private void StartMission(int net_id) {
            NetworkRequestControlOfNetworkId(net_id);
            missionVehicle = new Vehicle(NetworkGetEntityFromNetworkId(net_id));
            missionActive = true;
            SetAggressiveHorns(true);
            AttachBlipToMissionEntity();

            Tick += VehicleStatusCheck;
            Tick += MissionTick;

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
            Debug.WriteLine("Attaching Blip to Mission vehicle");
            Blip b = missionVehicle.AttachBlip();
            b.IsShortRange = false;
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
                await Delay(100);
            }
            Debug.WriteLine("Passing message: " + message);
            SetNotificationTextEntry("STRING");
            AddTextComponentString(message);
            SetNotificationMessage("CHAR_SIMEON", "CHAR_SIMEON", true, 1, "Simeon", "Vehicle Asset");
            DrawNotification(false, true);
        }
    }
}
