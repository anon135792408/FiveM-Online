using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace GTAOnline_FiveM {
    class Impound : BaseScript {
        IList<ImpoundSpace> ImpoundSpaces = new List<ImpoundSpace>();

        bool tickInit = false;

        public Impound() {
            EventHandlers.Add("GTAO:clientSyncImpoundSpaces", new Action<IList<ImpoundSpace>>(ReceiveImpoundListData));
            ImpoundSpaces = ParseImpoundSpaces();
            Tick += OnTick;
        }

        private IList<ImpoundSpace> ParseImpoundSpaces() {
            IList<ImpoundSpace> result = new List<ImpoundSpace>();
            foreach (KeyValuePair<Vector3, float> entry in ImpSpaces.ValidImpounds) {
                result.Add(new ImpoundSpace(entry.Key, entry.Value, false, -1, -1));
            }
            return result;
        }

        private async Task OnTick() {
            if (IsPedFatallyInjured(PlayerPedId()) && Game.Player.WantedLevel > 0 && Game.PlayerPed.LastVehicle != null) {
                int playerVeh = GetPlayersLastVehicle();

                while (IsPedFatallyInjured(PlayerPedId())) {
                    await Delay(500);
                    Debug.WriteLine("Awaiting player resurrection");
                }

                if (IsVehicleImpoundable(playerVeh)) {
                    if (SetEntityCoordsToFirstFreeImpoundSpace(playerVeh)) {
                        while (IsPlayerSwitchInProgress()) {
                            await Delay(0);
                        }
                        Screen.DisplayHelpTextThisFrame("Your vehicle has been impounded.");
                    }
                }
            }

            if (NetworkIsHost()) {
                if (ImpoundSpaces.Count() > 0 && !tickInit) {
                    Tick += MonitorImpound;
                    tickInit = true;
                } else {
                    Tick -= MonitorImpound;
                    tickInit = false;
                }
            }
        }

        private async Task MonitorImpound() {
            await Delay(0);
            foreach (ImpoundSpace imp in ImpoundSpaces) {
                await Delay(1000);
                if (imp.VehicleHandle != -1) {
                    Vehicle impVeh = new Vehicle(imp.VehicleHandle);
                    if (GetDistanceBetweenCoords(impVeh.Position.X, impVeh.Position.Y, impVeh.Position.Z, imp.Coords.X, imp.Coords.Y, imp.Coords.Z, true) > 7.0f) {
                        imp.PlayerHandle = -1;
                        imp.VehicleHandle = -1;
                    }
                    Debug.WriteLine("Impound Space " + imp.PlayerHandle);
                }
            }
        }

        private void ReceiveImpoundListData(IList<ImpoundSpace> impList) {
            this.ImpoundSpaces.Clear();
            this.ImpoundSpaces = new List<ImpoundSpace>(impList);
        }

        private bool SetEntityCoordsToFirstFreeImpoundSpace(int entity) {
            foreach (ImpoundSpace imp in ImpoundSpaces) {
                if (!imp.IsTaken) {
                    NetworkFadeOutEntity(entity, true, false);
                    SetEntityCoords(entity, imp.Coords.X, imp.Coords.Y, imp.Coords.Z, true, true, true, false);
                    SetEntityHeading(entity, imp.Heading);
                    SetEntityAsMissionEntity(entity, true, true);

                    imp.IsTaken = true;
                    imp.VehicleHandle = entity;
                    imp.PlayerHandle = PlayerId();

                    NetworkFadeInEntity(entity, false);

                    TriggerServerEvent("GTAO:serverSyncImpoundSpaces", ImpoundSpaces);

                    return true;
                }
            }
            return false;
        }

        private bool IsVehicleImpoundable(int vHandle) {
            Vehicle v = new Vehicle(vHandle);
            if (v.Occupants.Count() == 0 && !v.IsDead) {
                return true;
            }
            return false;
        }
    }
}
