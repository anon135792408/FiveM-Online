using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;

namespace GTAOnlineClient
{
    class Spawning : BaseScript
    {
        Ped playerPed = Game.PlayerPed;
        Player player = Game.Player;

        bool playerJustDead = false;
        public Spawning()
        {
            StartAudioScene("MP_LEADERBOARD_SCENE");
            Tick += OnTick;
            EventHandlers.Add("playerSpawned", new Action<Vector3>(OnPlayerSpawned));
        }

        public async Task OnTick()
        {
            if (player.IsDead && !playerJustDead)
            {
                RequestScriptAudioBank("MP_WASTED", false);
                playerJustDead = true;

                var scaleform = RequestScaleformMovie("MP_BIG_MESSAGE_FREEMODE");
                while (!HasScaleformMovieLoaded(scaleform))
                {
                    await Delay(0);
                }
                PushScaleformMovieFunction(scaleform, "SHOW_SHARD_WASTED_MP_MESSAGE");
                BeginTextComponent("STRING");
                AddTextComponentString("~r~wasted");
                EndTextComponent();
                PopScaleformMovieFunctionVoid();

                Screen.Effects.Start(ScreenEffect.DeathFailOut, 0, false);
                ShakeGameplayCam("DEATH_FAIL_IN_EFFECT_SHAKE", 1.0f);
                PlaySoundFrontend(-1, "MP_Flash", "WastedSounds", true);
                while (playerPed.IsDead)
                {
                    DrawScaleformMovieFullscreen(scaleform, 255, 255, 255, 255, 255);
                    await Delay(0);
                }
            }
        }

        private async void OnPlayerSpawned([FromSource]Vector3 pos)
        {
            Screen.Effects.Stop();
            playerJustDead = false;
            playerPed = Game.PlayerPed;
            StartPlayerSwitch(playerPed.Handle, playerPed.Handle, 1, 1);
            while (GetPlayerSwitchState() != 3)
            {
                await Delay(0);
            }
            Screen.Fading.FadeIn(500);
            StopAudioScene("MP_LEADERBOARD_SCENE");
        }
    }
}
