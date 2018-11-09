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
            EventHandlers.Add("GTAO:serverDisplaySimeonMissionMessage", new Action<dynamic>(DisplaySimeonMissionMessage));
            EventHandlers.Add("GTAO:serverSendMissionData", new Action<dynamic, dynamic, dynamic>(SendMissionData));
            EventHandlers.Add("playerConnecting", new Action<Player, string, CallbackDelegate>(OnPlayerConnecting));
        }

        private void OnPlayerConnecting([FromSource] Player player, string playerName, CallbackDelegate kickCallback)
        {
            TriggerClientEvent("GTAO:hostSyncSimMission", player);
        }

        private void DisplaySimeonMarker()
        {
            TriggerClientEvent("GTAO:clientDisplaySimeonMarker");
        }

        private void SendMissionData(dynamic pid, dynamic isMissionActive, dynamic vHandle)
        {
            if (pid == -1) // Send a pid value of -1 if you want to trigger this event for every player.
            {
                TriggerClientEvent("GTAO:clientReceiveMissionData", isMissionActive, vHandle);
            }
            else
            {
                TriggerClientEvent(pid, "GTAO:clientReceiveMissionData", isMissionActive, vHandle);
            }
        }

        private void DisplaySimeonMissionMessage(dynamic msg)
        {
            TriggerClientEvent("GTAO:clientDisplaySimeonMissionMessage", msg);
        }

        private void ClearSimeonMarker()
        {
            TriggerClientEvent("GTAO:clientClearSimeonMissionMessage");
        }
    }
}
