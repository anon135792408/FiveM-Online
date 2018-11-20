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
    class Impound : BaseScript
    {
        List<dynamic> ImpoundSpaces = new List<dynamic>();
        
        public Impound()
        {
            EventHandlers.Add("GTAO:clientSyncImpoundSpaces", new Action<List<dynamic>>(ReceiveImpoundListData));
            ImpoundSpaces.Add(new ImpoundSpace(new Vector3(420.79f, -1638.99f, 28.79f), 88.19f, false, -1, -1));
            Tick += OnTick;
        }

        private async Task OnTick()
        {
            await Delay(0);
            if (IsPedFatallyInjured(PlayerPedId()) && Game.Player.WantedLevel > 0 && Game.PlayerPed.LastVehicle != null)
            {
                while (IsPedFatallyInjured(PlayerPedId()))
                {
                    await Delay(500);
                    Debug.WriteLine("Awaiting player resurrection");
                }

                int playerVeh = GetPlayersLastVehicle();
                if (IsVehicleImpoundable(playerVeh))
                {
                    if (SetEntityCoordsToFirstFreeImpoundSpace(playerVeh))
                    {
                        while (IsPlayerSwitchInProgress())
                        {
                            await Delay(0);
                        }
                        Screen.DisplayHelpTextThisFrame("Your vehicle has been impounded.");
                    }
                }
            }
        }

        private void ReceiveImpoundListData(List<dynamic> impList)
        {
            this.ImpoundSpaces = null;
            this.ImpoundSpaces = new List<dynamic>(impList);
        }

        private bool SetEntityCoordsToFirstFreeImpoundSpace(int entity)
        {
            foreach(ImpoundSpace imp in ImpoundSpaces)
            {
                if (!imp.IsTaken)
                {
                    NetworkFadeOutEntity(entity, true, false);
                    SetEntityCoords(entity, imp.Coords.X, imp.Coords.Y, imp.Coords.Z, true, true, true, false);
                    SetEntityHeading(entity, imp.Heading);

                    imp.IsTaken = true;
                    imp.VehicleHandle = entity;
                    imp.PlayerHandle = PlayerId();

                    NetworkFadeInEntity(entity, false);
                    return true;
                }
            }
            return false;
        }

        private bool IsVehicleImpoundable(int vHandle)
        {
            Vehicle v = new Vehicle(vHandle);
            if (v.Occupants.Count() == 0 && !v.IsDead)
            {
                return true;
            }
            return false;
        }
    }
}
