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
    public class Spawning : BaseScript
    {
        Vector3 spawnPoint = new Vector3(360.39f, -585.02f, 28.82f);

        public Spawning()
        {
            EventHandlers.Add("GTAO:displayNotification", new Action<string>(DisplayNotification));
            EventHandlers.Add("playerSpawned", new Action(OnPlayerSpawned));
            Tick += OnTick;
            Tick += ManagerTick;
        }

        private async Task OnTick()
        {
            while (!Game.PlayerPed.Exists())
            {
                Screen.Fading.FadeOut(0);
                await Delay(0);
            }
        }

        private async Task ManagerTick()
        {
            await Delay(50);

            var playerPed = GetPlayerPed(-1);

            if (playerPed != -1)
            {
                if (NetworkIsPlayerActive(PlayerId()))
                {
                    if (Game.PlayerPed.IsDead)
                    {
                        await Delay(2500);
                        SpawnPlayer(spawnPoint);
                    }
                }
            }
        }

        bool spawnLock = false;

        private async void SpawnPlayer(Vector3 spawnPoint)
        {
            if (spawnLock)
            {
                return;
            }

            spawnLock = true;

            DoScreenFadeOut(500);

            while (IsScreenFadedOut())
            {
                await Delay(0);
            }

            FreezePlayer(PlayerId(), true);

            RequestCollisionAtCoord(spawnPoint.X, spawnPoint.Y, spawnPoint.Z);

            var playerPed = GetPlayerPed(-1);

            SetEntityCoordsNoOffset(playerPed, spawnPoint.X, spawnPoint.Y, spawnPoint.Z, true, true, false);
            NetworkResurrectLocalPlayer(spawnPoint.X, spawnPoint.Y, spawnPoint.Z, 0.0f, true, false);
            ClearPedTasksImmediately(playerPed);
            RemoveAllPedWeapons(playerPed, true);
            ClearPlayerWantedLevel(PlayerId());

            while (!HasCollisionLoadedAroundEntity(playerPed))
            {
                await Delay(0);
            }

            ShutdownLoadingScreen();

            DoScreenFadeIn(500);

            while (IsScreenFadingIn())
            {
                await Delay(0);
            }

            FreezePlayer(PlayerId(), false);

            TriggerEvent("playerSpawned");

            spawnLock = false;
        }

        private async void OnPlayerSpawned()
        {
            Tick -= OnTick;
            StartPlayerSwitch(PlayerPedId(), PlayerPedId(), 1024, 1);

            while (GetPlayerSwitchState() != 5)
            {
                await Delay(0);
            }
            Screen.Fading.FadeIn(1000);
        }

        private async void DisplayNotification(string msg)
        {
            while(!Game.PlayerPed.Exists())
            {
                await Delay(0);
            }
            Screen.ShowNotification(msg);
        }

        private void LoadScene(float x, float y, float z)
        {
            NewLoadSceneStart(x, y, z, 0.0f, 0.0f, 0.0f, 20.0f, 0);

            while (IsNewLoadSceneActive())
            {
                NetworkUpdateLoadScene();
            }
        }

        private void FreezePlayer(int player, bool freeze)
        {
            SetPlayerControl(player, !freeze, 0);

            var playerPed = GetPlayerPed(player);

            if (!freeze)
            {
                if (!IsEntityVisible(playerPed))
                {
                    SetEntityVisible(playerPed, true, true);
                }

                if (!IsPedInAnyVehicle(playerPed, true))
                {
                    SetEntityCollision(playerPed, true, true);
                }

                FreezeEntityPosition(playerPed, false);
                SetPlayerInvincible(player, false);
            }
            else
            {
                if (IsEntityVisible(playerPed))
                {
                    SetEntityVisible(playerPed, false, true);
                }

                SetEntityCollision(playerPed, false, true);
                FreezeEntityPosition(playerPed, true);
                SetPlayerInvincible(player, true);

                if (!IsPedFatallyInjured(playerPed))
                {
                    ClearPedTasksImmediately(playerPed);
                }
            }
        }
    }
}
