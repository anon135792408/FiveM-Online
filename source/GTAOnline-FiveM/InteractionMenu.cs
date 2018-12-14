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
    class InteractionMenu : BaseScript {
        private MenuPool _MenuPool = new MenuPool();
        private UIMenu interactionMenu;

        Ped mugger;
        Ped target;

        public InteractionMenu() {
            Tick += OnTick;

            //Menu 
            interactionMenu = new UIMenu(Game.Player.Name, "~b~Interaction Menu", false) {
                ScaleWithSafezone = false,
                MouseControlsEnabled = false,
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false
            };
            
            interactionMenu.AddItem(new UIMenuListItem("Services", new List<dynamic> { "Call Mugger", "Taxi", "Suicide"}, 0));
            interactionMenu.RefreshIndex();

            _MenuPool.Add(interactionMenu);

            interactionMenu.OnListSelect += (sender, item, index) => {
                switch (index) {
                    case 0:
                        interactionMenu.Visible = false;
                        CallMugger();
                        break;
                    case 1:
                        interactionMenu.Visible = false;
                        break;
                    case 2:
                        interactionMenu.Visible = false;
                        Game.PlayerPed.Kill();
                        break;
                }
            };
        }

        private async void CallMugger() {
            DisplayOnscreenKeyboard(0, "FMMC_KEY_TIP8", "", "", "", "", "", 64);
            while (UpdateOnscreenKeyboard() == 0) {
                DisableAllControlActions(0);
                await Delay(0);
            }

            string result = GetOnscreenKeyboardResult();
            foreach (Player p in Players) {
                if (p.Name.ToUpper().Equals(result.ToUpper())) {
                    Debug.WriteLine("Player Found!");
                    mugger = await World.CreatePed(PedHash.FibMugger01, World.GetNextPositionOnSidewalk(p.Character.Position + new Vector3(30f, 30f, 0f)), 0f);
                    mugger.Weapons.Give(WeaponHash.Knife, 1, true, true);
                    target = p.Character;
                    Tick += InitiateMugger;
                    break;
                }
            }
        }
        
        private async Task InitiateMugger() {
            mugger.Task.FightAgainst(target);
            if (mugger.IsDead) {
                mugger.MarkAsNoLongerNeeded();
                Player p = new Player(NetworkGetPlayerIndexFromPed(target.Handle));
                Screen.ShowNotification("The mugger you called on " + p.Name + " has been killed.");
                Tick -= InitiateMugger;
            } else if (target.IsDead) {
                mugger.Task.ClearAll();
                mugger.Task.WanderAround();
                mugger.MarkAsNoLongerNeeded();
                Player p = new Player(NetworkGetPlayerIndexFromPed(target.Handle));
                Screen.ShowNotification("The mugger you called on " + p.Name + " has successfully mugged the target.");
            }
        }

        private async Task OnTick() {
            _MenuPool.ProcessMenus();
            while (UpdateOnscreenKeyboard() == 0) {
                DisableAllControlActions(0);
                await Delay(0);
            }
            if (Game.IsControlJustPressed(0, Control.MultiplayerInfo)) {
                interactionMenu.Visible = !interactionMenu.Visible;
            }
        }
    }
}
