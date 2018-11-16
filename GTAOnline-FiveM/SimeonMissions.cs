using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace GTAOnline_FiveM
{
    class SimeonMissions : BaseScript
    {
        private const int MISSION_REFRESH_TIME = 1200000;
        private Vector3 SIMEON_MARKER_LOC = new Vector3(1204.73f, -3115.97f, 5.36f);
        private Vector3 SIMEON_MISSION_DROPOFF = new Vector3(1204.75f, -3115.10f, 5.34f);
        bool isMissionActive = false;
        Vehicle missionVehicle;
        Blip simBlip;
        static Random rnd = new Random();

        List<VehicleHash> wantedVehicles = new List<VehicleHash>
        {
            VehicleHash.Blista,
            VehicleHash.Asterope,
            VehicleHash.Asea,
            VehicleHash.Baller,
            VehicleHash.Tailgater,
            VehicleHash.Oracle,
            VehicleHash.Oracle2,
            VehicleHash.Patriot,
            VehicleHash.Premier,
            VehicleHash.Penumbra,
            VehicleHash.Prairie,
            VehicleHash.Buccaneer,
            VehicleHash.Buccaneer2,
            VehicleHash.Buffalo,
            VehicleHash.Buffalo2,
            VehicleHash.Cavalcade,
            VehicleHash.Cavalcade2,
            VehicleHash.Dubsta
        };

        Dictionary<Vector3, float> vehicleLocations = new Dictionary<Vector3, float>
        {
            { new Vector3(-65.79f, -1315.56f, 28.99f), 89.56f },
            { new Vector3(-941.50f, -2166.14f, 29.88f), 174.88f },
            { new Vector3(399.58f, -2005.71f, 22.53f), 157.67f },
            { new Vector3(88.69f, -1967.63f, 20.08f), 320.41f },
            { new Vector3(-1039.76f, -2641.36f, 15.81f), 150.30f },
            { new Vector3(-308.25f, -951.01f, 30.27f), 70.43f },
            { new Vector3(-87.13f, 369.54f, 111.64f), 244.91f }
        };

        public SimeonMissions()
        {
            EventHandlers.Add("playerSpawned", new Action(OnPlayerSpawned));
            EventHandlers.Add("GTAO:clientDisplaySimeonMarker", new Action(DisplaySimeonMarker));
            EventHandlers.Add("GTAO:clientClearSimeonMarker", new Action(ClearSimeonMarker));
            EventHandlers.Add("GTAO:clientDisplaySimeonMissionMessage", new Action<string>(DisplaySimeonMissionMessage));
            EventHandlers.Add("GTAO:hostSyncSimMission", new Action<Player>(SyncSimeonMissionForPlayer));
            EventHandlers.Add("GTAO:clientReceiveMissionData", new Action<dynamic, dynamic>(ReceiveMissionData));
        }

        private void OnPlayerSpawned()
        {
            Tick += OnTick;
        }

        private void SyncSimeonMissionForPlayer(Player player) => TriggerServerEvent("GTAO:serverSendMissionData", player, isMissionActive, missionVehicle.Handle);

        private void SyncSimeonMissionForAll() => TriggerServerEvent("GTAO:serverSendMissionData", -1, isMissionActive, missionVehicle.Handle);

        private void ReceiveMissionData(dynamic isMissionActive, dynamic vHandle)
        {
            missionVehicle = new Vehicle(vHandle);
            missionVehicle.IsPersistent = true;

            Blip vehBlip = missionVehicle.AttachBlip();
            vehBlip.Sprite = BlipSprite.PersonalVehicleCar;
            vehBlip.Color = BlipColor.Yellow;
            DisplaySimeonMarker();
        }

        private async Task MissionTick()
        {
            if (isMissionActive)
            {
                if (Game.PlayerPed.CurrentVehicle == missionVehicle && Game.PlayerPed.IsInRangeOf(SIMEON_MISSION_DROPOFF, 5.0f))
                {
                    missionVehicle.IsHandbrakeForcedOn = true;
                    while (missionVehicle.Speed > 0.0f)
                    {
                        await Delay(0);
                    }
                    Screen.Fading.FadeOut(500);
                    while (Screen.Fading.IsFadingOut)
                    {
                        await Delay(0);
                    }

                    TriggerServerEvent("GTAO:serverClearSimeonMarker");
                    string simMessage = "The vehicle has been delivered to my associates. Thank you.";
                    TriggerServerEvent("GTAO:serverDisplaySimeonMissionMessage", simMessage);

                    Tick -= MissionTick;
                    missionVehicle.Delete();
                    isMissionActive = false;
                    await Delay(2000);
                    Screen.Fading.FadeIn(500);
                    return;
                }

                if (missionVehicle.IsDead && NetworkIsHost())
                {
                    string simMessage = "Unfortunately the opportunity has passed as the vehicle I wanted was destroyed.";
                    TriggerServerEvent("GTAO:serverDisplaySimeonMissionMessage", simMessage);
                    isMissionActive = false;
                    missionVehicle.AttachedBlip.Delete();
                    missionVehicle.IsPersistent = false;
                    TriggerServerEvent("GTAO:serverClearSimeonMarker");
                }

                if (Game.PlayerPed.CurrentVehicle == missionVehicle)
                {
                    Game.Player.WantedLevel = 2;
                }
            }
        }

        private async Task OnTick()
        {
            if (NetworkIsHost() && !isMissionActive)
            {
                isMissionActive = true;

                int index = rnd.Next(vehicleLocations.Count());

                missionVehicle = await World.CreateVehicle(wantedVehicles[rnd.Next(wantedVehicles.Count())], vehicleLocations.ElementAt(index).Key, vehicleLocations.ElementAt(index).Value);
                VehToNet(missionVehicle.Handle);
                missionVehicle.IsPersistent = true;

                bool isFirstCharVowel = "aeiouAEIOU".IndexOf(missionVehicle.LocalizedName.ToCharArray()[0]) >= 0;
                string simMessage = "I am in need of a vehicle for one of my loyal customers.";
                if (isFirstCharVowel)
                {
                    simMessage = "I'm in need of an " + missionVehicle.LocalizedName + " for one of my customers.";
                }
                else
                {
                    simMessage = "I'm in need of a " + missionVehicle.LocalizedName + " for one of my customers.";
                }

                TriggerServerEvent("GTAO:serverDisplaySimeonMissionMessage", simMessage);
                TriggerServerEvent("GTAO:serverDisplaySimeonMarker");
                SyncSimeonMissionForAll();

                Tick += MissionTick;
            }
            await Delay(MISSION_REFRESH_TIME);
        }

        private void DisplaySimeonMarker()
        {
            simBlip = World.CreateBlip(SIMEON_MARKER_LOC);
            simBlip.IsShortRange = false;
            simBlip.Sprite = BlipSprite.Solomon;
            simBlip.Name = "Simeon";
            simBlip.Color = BlipColor.Yellow;
        }

        private void ClearSimeonMarker()
        {
            simBlip.Delete();
        }

        private void DisplaySimeonMissionMessage(string msg)
        {
            SetNotificationTextEntry("STRING");
            AddTextComponentString(msg);
            SetNotificationMessageClanTag_2("CHAR_SIMEON", "CHAR_SIMEON", true, 7, "Simeon", "~c~Vehicle Asset", 15, "", 8, 0);
            DrawNotification(true, false);
        }
    }
}
