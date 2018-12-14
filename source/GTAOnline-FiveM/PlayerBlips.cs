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
    class PlayerBlips : BaseScript {

        public PlayerBlips() {
            EventHandlers.Add("playerSpawned", new Action<int>(OnPlayerSpawned));
        }

        public void OnPlayerSpawned(int playerid) {
            foreach (Player player in Players) {
                if (player.Character.AttachedBlip == null && player.Handle != Game.Player.Handle) {
                    Blip b = player.Character.AttachBlip();
                    b.IsShortRange = false;
                    b.Sprite = BlipSprite.Player;
                    b.Color = BlipColor.White;
                    b.Name = player.Name;
                }
            }
        }
    }
}
