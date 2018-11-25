using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace GTAOnline_FiveM {
    class MirrorParkParty : BaseScript {
        int iLocal_0 = 0;

        public MirrorParkParty() {
            Tick += OnTick;
        }

        private async Task OnTick() {
            await Delay(0);
            if (NetworkIsHost()) {
                if (HasForceCleanupOccurred(18)) {
                    RemoveIpl();
                }

                if (!IsWorldPointWithinBrainActivationRange()) {
                    iLocal_0 = 3;
                }

                switch (iLocal_0) {
                    case 0:
                        if (GetClockHours() >= 22 || GetClockHours() <= 4) {
                            iLocal_0 = 1;
                        } else {
                            RemoveIpl();
                        }
                        break;
                    case 1:
                        if (!IsIplActive("ID2_21_G_Night")) {
                            RequestIpl("ID2_21_G_Night");
                            iLocal_0 = 2;
                        }
                        break;
                    case 2: break;
                    case 3:
                        if (!IsNewLoadSceneActive() && !IsPlayerSwitchInProgress()) {
                            RemoveIpl();
                        }
                        break;
                }
            }
        }

        private void RemoveIpl() {
            if (IsIplActive("ID2_21_G_Night")) {
                CitizenFX.Core.Native.API.RemoveIpl("ID2_21_G_Night");
            }
        }
    }
}
