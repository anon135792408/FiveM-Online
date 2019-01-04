using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using NativeUI;
using FiveM_Online_Client;

namespace GTAOnline_FiveM
{
    class InteractionMenu : BaseScript
    {
        private MenuPool _MenuPool = new MenuPool();
        private UIMenu interactionMenu;
        public UIMenu taxiMenu;

        public Vehicle playerTaxi;
        public Ped taxiDriver;
        public Vector3 Destination;

        Ped mugger;
        Ped target;

        public InteractionMenu()
        {
            Tick += OnTick;

            //Menu 
            interactionMenu = new UIMenu(Game.Player.Name, "~b~Interaction Menu", false)
            {
                ScaleWithSafezone = false,
                MouseControlsEnabled = false,
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false
            };

            interactionMenu.AddItem(new UIMenuListItem("Services", new List<dynamic> { "Call Mugger", "Call Taxi", "Suicide" }, 0));
            interactionMenu.RefreshIndex();

            _MenuPool.Add(interactionMenu);

            taxiMenu = new UIMenu("Taxi", "~b~Destinations", false)
            {
                ScaleWithSafezone = false,
                MouseControlsEnabled = false,
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false
            };

            taxiMenu.Clear();
            taxiMenu.AddItem(new UIMenuItem("Waypoint", "Waypoint"));
            taxiMenu.RefreshIndex();

            _MenuPool.Add(taxiMenu);

            interactionMenu.OnListSelect += (sender, item, index) =>
            {
                switch (index)
                {
                    case 0:
                        interactionMenu.Visible = false;
                        CallMugger();
                        break;
                    case 1:
                        interactionMenu.Visible = false;
                        InitTaxi();
                        break;
                    case 2:
                        interactionMenu.Visible = false;
                        Game.PlayerPed.Kill();
                        break;
                }
            };

            taxiMenu.OnItemSelect += (sender, item, index) =>
            {
                switch (index)
                {
                    case 0:
                        if (Game.IsWaypointActive)
                        {
                            //taxiDriver.Task.ClearAll();
                            Destination = World.GetNextPositionOnStreet(World.WaypointPosition);
                            taxiDriver.Task.DriveTo(playerTaxi, Destination, 5.0f, 30f, (int)DrivingStyle.Normal);
                        }
                        else
                        {
                            Screen.ShowNotification("You do not have a waypoint set.");
                        }
                        break;
                }
            };
        }

        private async Task OnTaxiTick()
        {
            if (Game.PlayerPed.IsInRangeOf(playerTaxi.Position, 7.0f) && Game.IsControlJustPressed(0, Control.Enter) && !Game.PlayerPed.IsInVehicle())
            {
                Game.PlayerPed.Task.EnterVehicle(playerTaxi, VehicleSeat.LeftRear, -1);
            }

            if (Game.PlayerPed.CurrentVehicle == playerTaxi)
            {
                if (Game.IsControlJustPressed(0, Control.Context))
                {
                    taxiMenu.Visible = !taxiMenu.Visible;
                }
            }

            if (playerTaxi.IsInRangeOf(Destination, 7.0f))
            {
                while (playerTaxi.WheelSpeed > 0f)
                {
                    await Delay(250);
                }

                while (Game.PlayerPed.IsInVehicle())
                {
                    Game.PlayerPed.Task.LeaveVehicle();
                    await Delay(250);
                }

                playerTaxi.AttachedBlip.Delete();

                Tick -= OnTaxiTick;
                Tick -= CheckTaxiStatus;

                playerTaxi.IsTaxiLightOn = false;
                playerTaxi.MarkAsNoLongerNeeded();
                taxiDriver.MarkAsNoLongerNeeded();    
                playerTaxi = null;
                taxiDriver = null;
                taxiMenu.Visible = false;
            }
        }

        private async void CallMugger()
        {
            DisplayOnscreenKeyboard(0, "FMMC_KEY_TIP8", "", "", "", "", "", 64);
            while (UpdateOnscreenKeyboard() == 0)
            {
                DisableAllControlActions(0);
                await Delay(100);
            }

            string result = GetOnscreenKeyboardResult();
            foreach (Player p in Players)
            {
                if (p.Name.ToUpper().Equals(result.ToUpper()))
                {
                    Debug.WriteLine("Player Found!");
                    mugger = await World.CreatePed(PedHash.FibMugger01, World.GetNextPositionOnSidewalk(p.Character.Position + new Vector3(30f, 30f, 0f)), 0f);
                    mugger.Weapons.Give(WeaponHash.Knife, 1, true, true);
                    target = p.Character;
                    Tick += InitiateMugger;
                    mugger.Task.FightAgainst(target);
                    break;
                }
            }
        }

        private async Task InitiateMugger()
        {
            await Delay(100);
            if (mugger.IsDead)
            {
                mugger.MarkAsNoLongerNeeded();
                Player p = new Player(NetworkGetPlayerIndexFromPed(target.Handle));
                Screen.ShowNotification("The mugger you called on " + p.Name + " has been killed.");

                target = null;
                mugger = null;
                Tick -= InitiateMugger;
            }
            else if (target.IsDead && target.GetKiller() == mugger)
            {
                mugger.Task.ClearAll();
                mugger.Task.WanderAround();
                mugger.MarkAsNoLongerNeeded();

                Player p = new Player(NetworkGetPlayerIndexFromPed(target.Handle));
                Screen.ShowNotification("The mugger you called on " + p.Name + " has successfully mugged the target.");

                target = null;
                mugger = null;
                Tick -= InitiateMugger;
            }
        }

        private async Task OnTick()
        {
            _MenuPool.ProcessMenus();
            while (UpdateOnscreenKeyboard() == 0)
            {
                DisableAllControlActions(0);
                await Delay(100);
            }
            if (Game.IsControlJustPressed(0, Control.MultiplayerInfo))
            {
                interactionMenu.Visible = !interactionMenu.Visible;
            }
        }

        private async Task CheckTaxiStatus()
        {
            if (playerTaxi.IsDead || taxiDriver.IsDead || taxiDriver.IsFleeing)
            {
                playerTaxi.AttachedBlip.Delete();
                playerTaxi.IsTaxiLightOn = false;
                Tick -= OnTaxiTick;
                Tick -= CheckTaxiStatus;
                playerTaxi = null;
                taxiDriver = null;
                taxiMenu.Visible = false;
            }
        }

        private async void InitTaxi()
        {
            if (playerTaxi == null)
            {
                playerTaxi = await World.CreateVehicle(VehicleHash.Taxi, World.GetNextPositionOnStreet(AdditionalMaths.GetAroundVector3(Game.PlayerPed.Position, 30, 50)));
                taxiDriver = await World.CreatePed(PedHash.Indian01AMM, playerTaxi.Position);
                playerTaxi.AttachBlip();
                playerTaxi.IsTaxiLightOn = true;

                taxiDriver.SetIntoVehicle(playerTaxi, VehicleSeat.Driver);
                taxiDriver.Task.DriveTo(playerTaxi, World.GetNextPositionOnStreet(Game.PlayerPed.Position), 5.0f, 30f, (int)DrivingStyle.Normal);

                await Delay(1500);

                Tick += OnTaxiTick;
                Tick += CheckTaxiStatus;

            }
            else
            {
                Screen.ShowNotification("You already have an active taxi.");
            }
        }
    }
}
