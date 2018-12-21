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
            Tick += OnTick;
        }

        private async Task OnTick() {
            foreach (Player player in Players) {
                await Delay(200);
                if (player.Character.AttachedBlip != null && player.Handle != Game.Player.Handle) {
                    Vector3 localPos = Game.PlayerPed.Position;
                    Vector3 playerPos = player.Character.Position;

                    float magnitude;
                    Vector3.Distance(ref localPos, ref playerPos, out magnitude);

                    Blip b = player.Character.AttachedBlip;

                    if (!Game.IsPaused) {
                        if (magnitude <= 255) {
                            b.Alpha = 255 - (int)magnitude;
                        } else {
                            b.Alpha = 0;
                        }
                    } else {
                        b.Alpha = 255;
                    }
                }
            }
        }

        public void OnPlayerSpawned(int playerid) {
            foreach (Player player in Players) {
                if (player.Character.AttachedBlip == null && player.Handle != Game.Player.Handle) {
                    Blip b = player.Character.AttachBlip();
                    b.IsShortRange = false;
                    b.Color = BlipColor.White;
                    b.Name = player.Name;
                    int pHandle = player.Handle;

                    if (NetworkIsFriend(ref pHandle)) {
                        SetBlipFriend(b.Handle, true);
                    }
                }
            }
        }
    }
}
