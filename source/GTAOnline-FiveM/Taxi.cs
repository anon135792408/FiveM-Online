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
    class Taxi : BaseScript {
        private MenuPool _MenuPool = new MenuPool();
        private UIMenu taxiMenu;

        private Vehicle playerTaxi;
        private Ped taxiDriver;
        private Vector3 Destination;

        public Taxi() {
            _MenuPool.Add(taxiMenu);
        }
        int i = 0;
        public async void CallTaxi() {
            taxiMenu = new UIMenu("Taxi", "~b~Destinations", false) {
                ScaleWithSafezone = false,
                MouseControlsEnabled = false,
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false
            };

            if (playerTaxi == null) {
                playerTaxi = await World.CreateVehicle(VehicleHash.Taxi, World.GetNextPositionOnStreet(GetAroundVector3(Game.PlayerPed.Position, 50f)));
                taxiDriver = await World.CreatePed(PedHash.Indian01AMM, playerTaxi.Position);
                taxiDriver.SetIntoVehicle(playerTaxi, VehicleSeat.Driver);
                playerTaxi.AttachBlip();
                taxiDriver.Task.DriveTo(playerTaxi, Game.PlayerPed.Position + (World.GetNextPositionOnSidewalk(Game.PlayerPed.Position) - World.GetNextPositionOnStreet(Game.PlayerPed.Position)), 5.0f, 30f, (int)DrivingStyle.Normal);
                Tick += OnTick;
                taxiMenu.Clear();
                taxiMenu.AddItem(new UIMenuItem("Waypoint", "Waypoint"));
                taxiMenu.RefreshIndex();
                taxiMenu.OnListSelect += (sender, item, index) => {
                    switch (index) {
                        case 0:
                            if (Game.IsWaypointActive) {
                                Destination = World.WaypointPosition;
                                taxiDriver.Task.DriveTo(playerTaxi, Destination + (World.GetNextPositionOnSidewalk(Destination) - World.GetNextPositionOnStreet(Destination)), 5.0f, 30f, (int)DrivingStyle.Normal);
                            }
                            break;
                    }
                };
            } else {
                Screen.ShowNotification("You already have an active taxi.");
            }
        }

        public async Task OnTick() {
            await Delay(0);
            /*if (Game.PlayerPed.IsInRangeOf(playerTaxi.Position, 7.0f)) {
                Debug.WriteLine("IsInRange");
                Game.PlayerPed.Task.ClearAllImmediately();
                Game.PlayerPed.Task.EnterVehicle(playerTaxi, VehicleSeat.Passenger, -1);
            }*/

            Debug.WriteLine("yes");

            if (Game.PlayerPed.CurrentVehicle == playerTaxi) {
                _MenuPool.ProcessMenus();
                if (Game.IsControlJustPressed(0, Control.Context)) {
                    taxiMenu.Visible = !taxiMenu.Visible;
                }
            }
        }

        public Vector3 GetAroundVector3(Vector3 v, float distance) {
            float variation = distance * 2;
            Random rnd = new Random();

            float bx = rnd.Next((int)distance, (int)variation) - distance;
            float by = rnd.Next((int)distance, (int)variation) - distance;

            return new Vector3(v.X + bx, v.Y + by, v.Z);
        }
    }
}
