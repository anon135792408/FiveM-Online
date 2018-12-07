﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace GTAOnline_Fivem_Server {
    class SimeonMission : BaseScript{
        public SimeonMission() {
            EventHandlers.Add("GTAO:DisplaySimeonMarkerForAll", new Action(DisplaySimeonMarkerForAll));
            EventHandlers.Add("GTAO:ClearSimeonMarkerForAll", new Action(ClearSimeonMarkerForAll));
            EventHandlers.Add("GTAO:EndMissionForAll", new Action(EndMissionForAll));
            EventHandlers.Add("GTAO:StartMissionForAll", new Action<int>(StartMissionForAll));
            EventHandlers.Add("GTAO:SimeonMissionFadeOutIn", new Action<int>(SimeonMissionFadeOutIn));
        }

        public void DisplaySimeonMarkerForAll() {
            Debug.WriteLine("Invoking DisplaySimeonMissionForAll on Clientside...");
            TriggerClientEvent("GTAO:DisplaySimeonMarkerForAll");
        }

        public void ClearSimeonMarkerForAll() {
            Debug.WriteLine("Invoking ClearSimeonMarkerForAll on Clientside...");
            TriggerClientEvent("GTAO:ClearSimeonMarkerForAll");
        }

        public void EndMissionForAll() {
            Debug.WriteLine("Invoking EndMissionForAll on Clientside...");
            TriggerClientEvent("GTAO:EndMissionForAll");
        }

        public void StartMissionForAll(int netid) {
            Debug.WriteLine("Invoking StartMissionForAll on Clientside...");
            TriggerClientEvent("GTAO:StartMissionForAll", netid);
        }

        public void SimeonMissionFadeOutIn(int netid) {
            Debug.WriteLine("Invoking SimeonMissionFadeOutIn on Clientside ID " + netid.ToString() + "...");
            TriggerClientEvent(Players[netid], "GTAO:SimeonMissionFadeOutIn");
        }
    }
}