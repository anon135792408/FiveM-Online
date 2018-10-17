using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
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
            SetManualShutdownLoadingScreenNui(true);
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
                Screen.Effects.Start(ScreenEffect.DeathFailMpIn, 0, false);
                PlaySoundFrontend(-1, "MP_Flash", "WastedSounds", true);
            }
        }

        private async void OnPlayerSpawned([FromSource]Vector3 pos)
        {
            playerJustDead = false;
            Screen.Effects.Stop();
            playerPed = Game.PlayerPed;
            StartPlayerSwitch(playerPed.Handle, playerPed.Handle, 1, 1);
            await Delay(3150);
            ShutdownLoadingScreenNui();
            Screen.Fading.FadeIn(500);
            StopAudioScene("MP_LEADERBOARD_SCENE");
        }
    }
}
