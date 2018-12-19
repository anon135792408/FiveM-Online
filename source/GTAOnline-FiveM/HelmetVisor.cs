using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace GTAOnline_FiveM {
    class HelmetVisor : BaseScript {
        public HelmetVisor() {
            Tick += OnTick;
        }

        private async Task OnTick() {
            if (Game.IsControlJustPressed(0, Control.VehicleFlyRollLeftOnly)) {
                int component = GetPedPropIndex(Game.PlayerPed.Handle, 0);
                int texture = GetPedPropTextureIndex(Game.PlayerPed.Handle, 0);
                int compHash = GetHashNameForProp(Game.PlayerPed.Handle, 0, component, texture);

                if (N_0xd40aac51e8e4c663(compHash) > 0) {
                    int newHelmet = 0;
                    string animName = "visor_up";
                    string animDict = "anim@mp_helmets@on_foot";
                    if ((uint)Game.PlayerPed.Model.Hash == (uint)PedHash.FreemodeFemale01) {
                        switch (component) {
                            case 49:
                                newHelmet = 67;
                                animName = "visor_up";
                                break;
                            case 50:
                                newHelmet = 68;
                                animName = "visor_up";
                                break;
                            case 51:
                                newHelmet = 69;
                                animName = "visor_up";
                                break;
                            case 52:
                                newHelmet = 70;
                                animName = "visor_up";
                                break;
                            case 62:
                                newHelmet = 71;
                                animName = "visor_up";
                                break;
                            case 66:
                                newHelmet = 81;
                                animName = "visor_down";
                                break;
                            case 67:
                                newHelmet = 49;
                                animName = "visor_down";
                                break;
                            case 68:
                                newHelmet = 50;
                                animName = "visor_down";
                                break;
                            case 69:
                                newHelmet = 51;
                                animName = "visor_down";
                                break;
                            case 70:
                                newHelmet = 52;
                                animName = "visor_down";
                                break;
                            case 71:
                                newHelmet = 62;
                                animName = "visor_down";
                                break;
                            case 72:
                                newHelmet = 73;
                                animName = "visor_up";
                                break;
                            case 73:
                                newHelmet = 72;
                                animName = "visor_down";
                                break;
                            case 77:
                                newHelmet = 78;
                                animName = "visor_up";
                                break;
                            case 78:
                                newHelmet = 77;
                                animName = "visor_down";
                                break;
                            case 79:
                                newHelmet = 80;
                                animName = "visor_up";
                                break;
                            case 80:
                                newHelmet = 79;
                                animName = "visor_down";
                                break;
                            case 81:
                                newHelmet = 66;
                                animName = "visor_up";
                                break;
                            case 90:
                                newHelmet = 91;
                                animName = "visor_up";
                                break;
                            case 91:
                                newHelmet = 90;
                                animName = "visor_down";
                                break;
                            case 115:
                                newHelmet = 116;
                                animName = "goggles_up";
                                break;
                            case 116:
                                newHelmet = 115;
                                animName = "goggles_down";
                                break;
                            case 117:
                                newHelmet = 118;
                                animName = "goggles_up";
                                break;
                            case 118:
                                newHelmet = 117;
                                animName = "goggles_down";
                                break;
                            case 122:
                                newHelmet = 123;
                                animName = "visor_up";
                                break;
                            case 123:
                                newHelmet = 122;
                                animName = "visor_down";
                                break;
                            case 124:
                                newHelmet = 125;
                                animName = "visor_up";
                                break;
                            case 125:
                                newHelmet = 124;
                                animName = "visor_down";
                                break;
                        }
                    } else if ((uint)Game.PlayerPed.Model.Hash == (uint)PedHash.FreemodeMale01) {
                        switch (component) {
                            case 50:
                                newHelmet = 68;
                                animName = "visor_up";
                                break;
                            case 51:
                                newHelmet = 69;
                                animName = "visor_up";
                                break;
                            case 52:
                                newHelmet = 70;
                                animName = "visor_up";
                                break;
                            case 53:
                                newHelmet = 71;
                                animName = "visor_up";
                                break;
                            case 62:
                                newHelmet = 72;
                                animName = "visor_up";
                                break;
                            case 67:
                                newHelmet = 82;
                                animName = "visor_down";
                                break;
                            case 68:
                                newHelmet = 50;
                                animName = "visor_down";
                                break;
                            case 69:
                                newHelmet = 51;
                                animName = "visor_down";
                                break;
                            case 70:
                                newHelmet = 52;
                                animName = "visor_down";
                                break;
                            case 71:
                                newHelmet = 53;
                                animName = "visor_down";
                                break;
                            case 72:
                                newHelmet = 62;
                                animName = "visor_down";
                                break;
                            case 73:
                                newHelmet = 74;
                                animName = "visor_up";
                                break;
                            case 74:
                                newHelmet = 73;
                                animName = "visor_down";
                                break;
                            case 78:
                                newHelmet = 79;
                                animName = "visor_up";
                                break;
                            case 79:
                                newHelmet = 78;
                                animName = "visor_down";
                                break;
                            case 80:
                                newHelmet = 81;
                                animName = "visor_up";
                                break;
                            case 81:
                                newHelmet = 80;
                                animName = "visor_down";
                                break;
                            case 82:
                                newHelmet = 67;
                                animName = "visor_up";
                                break;
                            case 91:
                                newHelmet = 92;
                                animName = "visor_up";
                                break;
                            case 92:
                                newHelmet = 91;
                                animName = "visor_down";
                                break;
                            case 116:
                                newHelmet = 117;
                                animName = "goggles_up";
                                break;
                            case 117:
                                newHelmet = 116;
                                animName = "goggles_down";
                                break;
                            case 118:
                                newHelmet = 119;
                                animName = "goggles_up";
                                break;
                            case 119:
                                newHelmet = 118;
                                animName = "goggles_down";
                                break;
                            case 123:
                                newHelmet = 124;
                                animName = "visor_up";
                                break;
                            case 124:
                                newHelmet = 123;
                                animName = "visor_down";
                                break;
                            case 125:
                                newHelmet = 126;
                                animName = "visor_up";
                                break;
                            case 126:
                                newHelmet = 125;
                                animName = "visor_down";
                                break;
                        }
                    } else {
                        return;
                    }
                    ClearPedTasks(Game.PlayerPed.Handle);
                    TaskPlayAnim(Game.PlayerPed.Handle, animDict, animName, 8.0f, 1.0f, -1, 48, 0.0f, false, false, false);
                    Debug.WriteLine(GetAnimDuration(animDict, animName).ToString());
                    
                    while(GetEntityAnimCurrentTime(Game.PlayerPed.Handle, animDict, animName) < 0.5f) {
                        Debug.WriteLine(GetEntityAnimCurrentTime(Game.PlayerPed.Handle, animDict, animName).ToString());
                        await Delay(0);
                    }
                    SetPedPropIndex(Game.PlayerPed.Handle, 0, newHelmet, texture, true);
                    ClearPedTasks(Game.PlayerPed.Handle);
                    RemoveAnimDict(animDict);
                }
            }
        }
    }
}
