using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace GTAOnline_Fivem_Server
{
    class SimeonMissions : BaseScript
    {
        public SimeonMissions()
        {
            EventHandlers.Add("GTAO:serverDisplaySimeonMarker", new Action(DisplaySimeonMarker));
            EventHandlers.Add("GTAO:serverClearSimeonMarker", new Action(DisplaySimeonMarker));
            EventHandlers.Add("GTAO:serverDisplaySimeonMissionMessage", new Action(DisplaySimeonMissionMessage));
            EventHandlers.Add("GTAO:serverSyncMissionVehicle", new Action<dynamic>(SyncMissionVehicle));
        }

        private void DisplaySimeonMarker()
        {
            TriggerClientEvent("GTAO:clientDisplaySimeonMarker");
        }

        private void SyncMissionVehicle(dynamic missionVehicle)
        {
            TriggerClientEvent("GTAO:clientSyncMissionVehicle", missionVehicle);
        }

        private void DisplaySimeonMissionMessage()
        {
            TriggerClientEvent("GTAO:clientDisplaySimeonMissionMessage");
        }

        private void ClearSimeonMarker()
        {
            TriggerClientEvent("GTAO:clientClearSimeonMissionMessage");
        }
    }
}
