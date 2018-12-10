using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using CitizenFX.Core.Native;

namespace GTAOnline_FiveM {
    class PhoneCamera : BaseScript {

        bool isPhoneActive = false;

        public PhoneCamera() {
            DestroyMobilePhone();
            Tick += OnTick;
        }

        public async Task OnTick() {
            await Delay(100);
            
            if (IsControlJustPressed(0, 27)) {
                CreateMobilePhone(0);
                CellCamActivate(true, true);
                isPhoneActive = true;
            }

            if (IsControlJustPressed(0,177) && isPhoneActive) {
                DestroyMobilePhone();
                isPhoneActive = false;
                CellCamActivate(false, false);
            }

            if (IsControlJustPressed(0, 176) && isPhoneActive) {
                Function.Call((Hash)0xa67c35c56eb1bd9d);
                if (Function.Call<bool>((Hash)0x0d6ca79eeebd8ca3) && Function.Call<bool>((Hash)0x3dec726c25a11bac, -1)) {
                    Function.Call((Hash)0xd801cc02177fa3f1);
                }
            }

            if (isPhoneActive) {
                HideHudComponentThisFrame(7);
                HideHudComponentThisFrame(8);
                HideHudComponentThisFrame(9);
                HideHudComponentThisFrame(6);
                HideHudComponentThisFrame(19);
                HideHudAndRadarThisFrame();
            }

            if (Game.PlayerPed.IsDead) {
                isPhoneActive = false;
                DestroyMobilePhone();
            }
        }
    }
}
