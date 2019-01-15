using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using FiveM_Online_Client;

namespace GTAOnline_FiveM
{
    /*
     * Credit where due: https://github.com/AppiChudilko/SpawnManager-FIveM/blob/master/Spawn.cs
     */
    public class Spawning : BaseScript
    {
        public List<string> uNames = new List<string>();
        public string userIdentifier;
        Vector3 lastPos;

        public Spawning()
        {
            EventHandlers.Add("GTAO:SyncSQLPlayerList", new Action<List<string>>(SyncSQLPlayerList));
            EventHandlers.Add("GTAO:SyncPlayerIdentifier", new Action<dynamic>(SyncPlayerIdentifier));
            EventHandlers.Add("GTAO:SyncPlayerLastPos", new Action<dynamic>(SyncPlayerLastPos));
            DoPlayerSpawn();
            Tick += OnTick;
        }

        private void SyncSQLPlayerList(List<string> list)
        {
            uNames = new List<string>(list);
        }

        private void SyncPlayerIdentifier(dynamic identifier)
        {
            userIdentifier = identifier;
        }

        private void SyncPlayerLastPos(dynamic pos)
        {
            lastPos = pos;
        }

        private async void DoPlayerSpawn()
        {
            if (!NetworkIsPlayerActive(PlayerId()))
            {
                await Delay(100);
            }

            TriggerServerEvent("GTAO:RetrieveSQLPlayerList", Game.Player.Handle);

            TriggerServerEvent("GTAO:RetrievePlayerIdentifier", Game.Player.Handle);

            await Delay(2500);

            if (uNames.Count > 0)
            {
                Debug.WriteLine("Over Zero!");
                foreach (string str in uNames)
                {
                    Debug.WriteLine(str);

                    if (str.Equals(Game.Player.Name))
                    {
                        
                        Debug.WriteLine("User exists! Retrieving Data...");
                        TriggerServerEvent("GTAO:RetrievePlayerLastPos", PlayerId());
                        await SpawnPlayer("MP_M_FREEMODE_01", lastPos.X, lastPos.Y, lastPos.Z, 0.0f);
                    }
                    else
                    {
                        await SpawnPlayer("MP_M_FREEMODE_01", 30.18f, -723.04f, 44.19f, 248.17f);
                        TriggerServerEvent("GTAO:SavePlayerData", userIdentifier, GetPlayerName(PlayerId()));
                    }
                }
            }
            else
            {
                Debug.WriteLine("Below Zero!");
                await SpawnPlayer("MP_M_FREEMODE_01", 30.18f, -723.04f, 44.19f, 248.17f);
                TriggerServerEvent("GTAO:SavePlayerData", userIdentifier, GetPlayerName(PlayerId()));
            }
        }

        private static bool _spawnLock = false;

        public static void FreezePlayer(int playerId, bool freeze)
        {
            var ped = GetPlayerPed(playerId);

            SetPlayerControl(playerId, !freeze, 0);

            if (!freeze)
            {
                if (!IsEntityVisible(ped))
                    SetEntityVisible(ped, true, true);

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

        private async Task OnTick()
        {
            if (IsPedFatallyInjured(PlayerPedId()))
            {
                await Delay(3400);
                await SpawnPlayer("MP_M_FREEMODE_01", 30.18f, -723.04f, 44.19f, 248.17f);
            }

            if (IsPlayerSwitchInProgress())
            {
                HideHudAndRadarThisFrame();
            }
        }

        public static async Task SpawnPlayer(string skin, float x, float y, float z, float heading)
        {
            if (_spawnLock)
                return;

            _spawnLock = true;

            Screen.Fading.FadeOut(500);

            Debug.WriteLine("Before While fading out");
            while (Screen.Fading.IsFadingOut)
            {
                await Delay(1);
            }

            if (!IsPlayerSwitchInProgress())
            {
                SwitchOutPlayer(PlayerPedId(), 0, 1);
            }

            FreezePlayer(PlayerId(), true);
            await Game.Player.ChangeModel(GetHashKey(skin));
            SetPedDefaultComponentVariation(GetPlayerPed(-1));
            RequestCollisionAtCoord(x, y, z);

            var ped = Game.PlayerPed.Handle;

            SetEntityCoordsNoOffset(ped, x, y, z, false, false, false);
            NetworkResurrectLocalPlayer(x, y, z, heading, true, true);
            NewLoadSceneStart(x, y, z, 0.0f, 0.0f, 0.0f, 20.0f, 0);
            NetworkUpdateLoadScene();
            ClearPedTasksImmediately(ped);
            RemoveAllPedWeapons(ped, false);
            ClearPlayerWantedLevel(PlayerId());

            while (!HasCollisionLoadedAroundEntity(ped))
            {
                await Delay(1);
            }

            if (GetPlayerSwitchState() != 8)
            {
                await Delay(100);
            }

            SwitchInPlayer(PlayerPedId());

            ShutdownLoadingScreen();
            Screen.Fading.FadeIn(500);

            while (Screen.Fading.IsFadingIn)
            {
                await Delay(1);
            }

            while (IsPlayerSwitchInProgress())
            {
                await Delay(100);
            }

            FreezePlayer(PlayerId(), false);

            TriggerEvent("playerSpawned", PlayerId());
            SetEntityCoordsNoOffset(ped, x, y, z, false, false, false);
            _spawnLock = false;

            SetCanAttackFriendly(PlayerPedId(), true, false);

        }
    }
}
