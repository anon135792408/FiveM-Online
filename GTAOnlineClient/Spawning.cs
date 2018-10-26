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
        Ped deathPed;

        public Spawning()
        {
            StartAudioScene("MP_LEADERBOARD_SCENE");
            Tick += OnTick;
            EventHandlers.Add("playerSpawned", new Action<Vector3>(OnPlayerSpawned));
        }

        public async Task OnTick()
        {
            if (player.IsDead)
            {
                deathPed = await World.CreatePed(PedHash.MovPrem01SFY, new Vector3(0, -10, 0));
                deathPed.IsPositionFrozen = true;

                RequestScriptAudioBank("MP_WASTED", false);

                var scaleform = RequestScaleformMovie("MP_BIG_MESSAGE_FREEMODE");
                while (!HasScaleformMovieLoaded(scaleform))
                {
                    await Delay(0);
                }
                PushScaleformMovieFunction(scaleform, "SHOW_SHARD_WASTED_MP_MESSAGE");
                
                BeginTextComponent("STRING");
                if (player.Character.GetKiller() != null)
                {
                    Ped killer = new Ped(GetPedKiller(player.Character.Handle));
                    if (killer != null)
                    {
                        int pedType = GetPedType(killer.Handle);
                        if (pedType == 6 || pedType == 27)
                        {
                            AddTextComponentString("~b~busted");
                        }
                        else
                        {
                            AddTextComponentString("~r~wasted");
                        }
                    }
                }
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
                Tick -= OnTick;
            }
        }

        private async void OnPlayerSpawned([FromSource]Vector3 pos)
        {
            Tick += OnTick;
            Screen.Effects.Stop();
            playerPed = Game.PlayerPed;
            StartPlayerSwitch(deathPed.Handle, playerPed.Handle, 1, 1);
            deathPed.Delete();
            while (GetPlayerSwitchState() != 3) //Don't fade the player in until the switch is in progress
            {
                await Delay(0);
            }
            Screen.Fading.FadeIn(500);
            StopAudioScene("MP_LEADERBOARD_SCENE");
        }
    }
}
