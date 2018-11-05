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
        private const int MISSION_REFRESH_TIME = 960000;
        private Vector3 SIMEON_MARKER_LOC = new Vector3(1204.73f, -3115.97f, 5.36f);
        private Vector3 SIMEON_MISSION_DROPOFF = new Vector3(0.0f, 0.0f, 0.0f);
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
            VehicleHash.Prairie
        };

        Dictionary<Vector3, float> vehicleLocations = new Dictionary<Vector3, float>
        {
            {new Vector3(-65.79f, -1315.56f, 28.99f), 89.56f }
        };

        public SimeonMissions()
        {
            EventHandlers.Add("GTAO:clientDisplaySimeonMarker", new Action(DisplaySimeonMarker));
            EventHandlers.Add("GTAO:clientClearSimeonMarker", new Action(ClearSimeonMarker));
            EventHandlers.Add("GTAO:clientDisplaySimeonMissionMessage", new Action(DisplaySimeonMissionMessage));
            EventHandlers.Add("GTAO:clientSyncMissionVehicle", new Action<dynamic>(SyncMissionVehicle));
            Tick += OnTick;
            Tick += MissionTick;
        }

        private async Task MissionTick()
        {
            if (isMissionActive)
            {
                if (Game.PlayerPed.CurrentVehicle == missionVehicle && Game.PlayerPed.IsInRangeOf(SIMEON_MISSION_DROPOFF, 2.0f))
                {
                    missionVehicle.IsHandbrakeForcedOn = true;
                    Screen.Fading.FadeOut(500);
                    while (Screen.Fading.IsFadingOut)
                    {
                        await Delay(0);
                    }
                    missionVehicle.Delete();
                    isMissionActive = false;
                    TriggerServerEvent("GTAO:serverClearSimeonMarker");
                    Screen.Fading.FadeIn(500);
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
                SetEntityAsMissionEntity(missionVehicle.Handle, false, false);
                TriggerServerEvent("GTAO:serverSyncMissionVehicle", missionVehicle);

                TriggerServerEvent("GTAO:serverDisplaySimeonMarker");
                TriggerServerEvent("GTAO:serverDisplaySimeonMissionMessage");
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

        private void DisplaySimeonMissionMessage()
        {
            SetNotificationTextEntry("STRING");
            AddTextComponentString("I'm in need of a " + missionVehicle.DisplayName + " for one of my loyal customers");
            SetNotificationMessageClanTag_2("CHAR_SIMEON", "CHAR_SIMEON", true, 7, "Simeon", "~c~Vehicle Asset", 15, "", 8, 0);
            DrawNotification(true, false);
        }

        private void SyncMissionVehicle(dynamic missionVehicle)
        {
            this.missionVehicle = missionVehicle;
            SetEntityAsMissionEntity(missionVehicle.Handle, false, false);
        }
    }
}
