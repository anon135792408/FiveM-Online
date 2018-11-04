﻿using System;
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
        bool isMissionActive = false;
        Vehicle missionVehicle;

        public SimeonMissions()
        {
            EventHandlers.Add("GTAO:clientDisplaySimeonMarker", new Action(DisplaySimeonMarker));
            Tick += OnTick;
        }

        private async Task OnTick()
        {
            if (NetworkIsHost() && isMissionActive)
            {
                TriggerServerEvent("GTAO:serverDisplaySimeonMarker");

                isMissionActive = true;
                missionVehicle = await World.CreateVehicle(VehicleHash.Blista, new Vector3(-65.79f, -1315.56f, 28.99f), 89.56f);
                missionVehicle.IsPersistent = true;
            }
            await Delay(MISSION_REFRESH_TIME);
        }

        private void DisplaySimeonMarker()
        {
            Blip simBlip = World.CreateBlip(SIMEON_MARKER_LOC);
            simBlip.IsShortRange = false;
            simBlip.Sprite = BlipSprite.Simeon;
            simBlip.Color = BlipColor.Yellow;
        }
    }
}
