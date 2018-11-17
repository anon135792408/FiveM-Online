using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace GTAOnline_FiveM
{
    /*
     * Credit where due: https://github.com/AppiChudilko/SpawnManager-FIveM/blob/master/Spawn.cs
     */
    public class Spawning : BaseScript
    {
        public Spawning()
        {
            doPlayerSpawn();
        }

        private async void doPlayerSpawn()
        {
            if (!NetworkIsPlayerActive(PlayerId()))
            {
                await Delay(0);
            }
            await SpawnPlayer("MP_M_FREEMODE_01", 0.916756f, 528.485f, 174.628f, 0f);
        }

        private static bool _spawnLock = false;

        public static void FreezePlayer(int playerId, bool freeze)
        {
            var ped = GetPlayerPed(playerId);

            SetPlayerControl(playerId, !freeze, 0);

            if (!freeze)
            {
                if (!IsEntityVisible(ped))
                    SetEntityVisible(ped, true, false);

                if (!IsPedInAnyVehicle(ped, true))
                    SetEntityCollision(ped, true, true);

                FreezeEntityPosition(ped, false);
                //SetCharNeverTargetted(ped, false)
                SetPlayerInvincible(playerId, false);
            }
            else
            {
                if (IsEntityVisible(ped))
                    SetEntityVisible(ped, false, false);

                SetEntityCollision(ped, false, true);
                FreezeEntityPosition(ped, true);
                //SetCharNeverTargetted(ped, true)
                SetPlayerInvincible(playerId, true);

                if (IsPedFatallyInjured(ped))
                    ClearPedTasksImmediately(ped);
            }
        }

        public static async Task SpawnPlayer(string skin, float x, float y, float z, float heading)
        {
            if (_spawnLock)
                return;

            _spawnLock = true;

            DoScreenFadeOut(500);

            while (IsScreenFadingOut())
            {
                await Delay(1);
            }

            FreezePlayer(PlayerId(), true);
            await Game.Player.ChangeModel(GetHashKey(skin));
            SetPedDefaultComponentVariation(GetPlayerPed(-1));
            RequestCollisionAtCoord(x, y, z);

            var ped = GetPlayerPed(-1);

            SetEntityCoordsNoOffset(ped, x, y, z, false, false, false);
            NetworkResurrectLocalPlayer(x, y, z, heading, true, true);
            ClearPedTasksImmediately(ped);
            RemoveAllPedWeapons(ped, false);
            ClearPlayerWantedLevel(PlayerId());

            while (HasCollisionLoadedAroundEntity(ped))
            {
                await Delay(1);
            }

            ShutdownLoadingScreen();
            DoScreenFadeIn(500);

            while (IsScreenFadingIn())
            {
                await Delay(1);
            }

            FreezePlayer(PlayerId(), false);

            //TriggerEvent("playerSpawned", PlayerId());

            _spawnLock = false;
        }
    }
}
