using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace GTAOnlineClient
{
    class Impound : BaseScript
    {
        Vehicle playerVeh = null;
        bool isBeingImpounded = false;

        Dictionary<Vector3, float> impoundSpaces = new Dictionary<Vector3, float>() // Parking Pos + Vehicle Heading
        {
            {new Vector3(400.83f,-1648.53f,28.82f), 139.71f},
        };

        public Impound()
        {
            Tick += OnTick;
            EventHandlers.Add("playerSpawned", new Action<Vector3>(OnPlayerSpawned));
        }

        private async Task OnTick()
        {
            if (!Game.Player.IsDead)
            {
                playerVeh = Game.PlayerPed.LastVehicle;
            }

            if (Game.Player.IsDead && Game.Player.WantedLevel > 0 && !isBeingImpounded)
            {
                isBeingImpounded = true;
                if (playerVeh.Occupants.Count() < 1 || playerVeh.Driver == Game.PlayerPed)
                {
                    ImpoundPlayerLastVehicle();
                }
            }
        }

        public async void ImpoundPlayerLastVehicle()
        {
            playerVeh = Game.PlayerPed.LastVehicle;
            Random rnd = new Random();
            var rndSpace = impoundSpaces.ElementAt(rnd.Next(impoundSpaces.Count));
            while (playerVeh.Occupants.Count() > 0)
            {
                await Delay(0);
            }
            playerVeh.Position = rndSpace.Key;
            playerVeh.Heading = rndSpace.Value;
            playerVeh.IsEngineRunning = false;
        }

        private async void OnPlayerSpawned([FromSource]Vector3 pos)
        {
            if (isBeingImpounded)
            {
                Screen.DisplayHelpTextThisFrame("Your vehicle has been impounded.");
                isBeingImpounded = false;
                await Delay(2500);
            }
        }
    }
}
