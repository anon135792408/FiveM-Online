using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace GTAOnline_FiveM {
    class Christmas : BaseScript {
        /*
         * Credit to TomGrobbe's snowball script: https://github.com/TomGrobbe/Snowballs/blob/master/snowballs/client.lua
         * */

        bool loaded = false;

        public Christmas() {
            Tick += OnTick;
        }

        private async Task OnTick() {
            DateTime dt = DateTime.Now;

            if (dt.Day >= 14 && dt.Day <= 26 && dt.Month == 12) {
                SetWeatherTypeNowPersist("XMAS");
                await Delay(0);
                if (IsNextWeatherType("XMAS")) {
                    N_0xc54a08c85ae4d410(3.0f); // Sets ice effect on water

                    SetForceVehicleTrails(true);
                    SetForcePedFootstepsTracks(true);

                    if (!loaded) {
                        RequestScriptAudioBank("ICE_FOOTSTEPS", false);
                        RequestScriptAudioBank("SNOW_FOOTSTEPS", false);
                        RequestNamedPtfxAsset("core_snow");
                        while (!HasNamedPtfxAssetLoaded("core_snow")) {
                            await Delay(0);
                        }
                        UseParticleFxAssetNextCall("core_snow");
                        loaded = true;
                    }

                    RequestAnimDict("anim@mp_snowball");
                    if (IsControlJustReleased(0, 119) && !IsPedInAnyVehicle(GetPlayerPed(-1), true) && !IsPlayerFreeAiming(PlayerId()) && !IsPedSwimming(PlayerPedId()) && !IsPedSwimmingUnderWater(PlayerPedId()) && !IsPedRagdoll(PlayerPedId()) && !IsPedFalling(PlayerPedId()) && !IsPedRunning(PlayerPedId()) && !IsPedSprinting(PlayerPedId()) && GetInteriorFromEntity(PlayerPedId()) == 0 && !IsPedShooting(PlayerPedId()) && !IsPedUsingAnyScenario(PlayerPedId()) && !IsPedInCover(PlayerPedId(), false)) {
                        TaskPlayAnim(PlayerPedId(), "anim@mp_snowball", "pickup_snowball", 8.0f, -1, -1, 0, 1, false, false, false);
                        await Delay(1950);
                        GiveWeaponToPed(GetPlayerPed(-1), uint.Parse(GetHashKey("WEAPON_SNOWBALL").ToString()), 2, false, true);
                    }

                } else {
                    if (loaded) {
                        N_0xc54a08c85ae4d410(0.0f);
                    }
                    loaded = false;
                    RemoveNamedPtfxAsset("core_snow");
                    ReleaseNamedScriptAudioBank("ICE_FOOTSTEPS");
                    ReleaseNamedScriptAudioBank("SNOW_FOOTSTEPS");
                    SetForceVehicleTrails(false);
                    SetForcePedFootstepsTracks(false);
                }

                if (GetSelectedPedWeapon(PlayerPedId()) == GetHashKey("WEAPON_SNOWBALL")) {
                    SetPlayerWeaponDamageModifier(PlayerId(), 0.0f);
                }
            }
        }
    }
}
