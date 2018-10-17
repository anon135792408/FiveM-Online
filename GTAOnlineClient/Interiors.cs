using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace GTAOnlineServer
{
    class Interiors : BaseScript
    {
        float drawDistanceMag = 50.0f;
        bool isPlayerInBuilding = false;
        Vector3 playerPos = Game.PlayerPed.Position;
        Interior currLocation = null;
        Interior lastLocation = null;

        static Interior[] intLocations =
        {
            new Interior {Name = "Bahamas Mamas", Entrance = new Vector3(-1388.82f,-586.23f,30.22f), Exit = new Vector3(-1387.40f,-588.55f,30.32f), Size = 1.0f},
            new Interior {Name = "FIB Top Floor", Entrance = new Vector3(136.19f, -761.85f, 45.75f), Exit = new Vector3(136.19f, -761.85f, 242.15f), Size = 1.0f},
            new Interior {Name = "Weazel Plaza Apartment 56", Entrance = new Vector3(-907.14f,-452.02f,39.41f), Exit = new Vector3(-890.62f,-436.67f,121.61f), Size = 1.0f},
            new Interior {Name = "Weazel Plaza Apartment 41", Entrance = new Vector3(-909.60f,-446.45f,39.61f), Exit = new Vector3(-890.63f,-452.76f,95.46f), Size = 1.0f},
            new Interior {Name = "Maze Bank Office", Entrance = new Vector3(-66.78f,-802.39f,44.23f), Exit = new Vector3(-77.25f,-828.54f,243.39f), Size = 1.0f},
            new Interior {Name = "0112 South Rockford Drive", Entrance = new Vector3(-810.43f,-979.03f,13.22f), Exit = new Vector3(266.12f,-1007.43f,-101.01f), Size = 1.0f},
            new Interior {Name = "2044 North Conker Avenue", Entrance = new Vector3(346.43f,440.67f,147.72f), Exit = new Vector3(341.65f,437.77f,149.39f), Size = 1.0f},
        };

        public Interiors()
        {
            Tick += OnTick;
            Tick += EntrancePromptTick;
            Tick += DeathCheck;
            foreach (Interior i in intLocations)
            {
                Blip intBlip = World.CreateBlip(i.Entrance);
                intBlip.Sprite = BlipSprite.Garage2;
                intBlip.IsShortRange = true;
                intBlip.Name = i.Name;
            }
        }

        private async Task DeathCheck()
        {
            if (Game.PlayerPed.IsDead)
            {
                currLocation = null;
            }
        }

        private async Task OnTick()
        {
            foreach (Interior i in intLocations)
            {
                if (Game.PlayerPed.IsInRangeOf(i.Exit, drawDistanceMag))
                {
                    World.DrawMarker(MarkerType.VerticalCylinder, i.Exit + new Vector3(0, 0, -1.0001f), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), System.Drawing.Color.FromArgb(255, 255, 255));
                }
                if (Game.PlayerPed.IsInRangeOf(i.Entrance, drawDistanceMag))
                {
                    World.DrawMarker(MarkerType.VerticalCylinder, i.Entrance + new Vector3(0, 0, -1.0001f), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1), System.Drawing.Color.FromArgb(255, 255, 255));
                    if (!isPlayerInBuilding)
                    {
                        if (Game.PlayerPed.IsInRangeOf(i.Entrance, i.Size))
                        {
                            currLocation = i;
                        }
                        else
                        {
                            currLocation = null;
                        }
                    }
                }
            }
        }

        private async Task EntrancePromptTick()
        {
            if (currLocation != null && !isPlayerInBuilding)
            {
                Screen.DisplayHelpTextThisFrame("Press ~b~~h~E~h~~w~ to enter ~b~~h~ " + currLocation.Name + "~h~~w~");
                if (Game.IsControlJustPressed(0, Control.Context))
                {
                    lastLocation = currLocation;
                    TeleportPlayerIntoBuilding(lastLocation);
                }
            }
            if (lastLocation != null && isPlayerInBuilding && Game.PlayerPed.IsInRangeOf(lastLocation.Exit, lastLocation.Size))
            {
                Screen.DisplayHelpTextThisFrame("Press ~b~~h~E~h~~w~ to exit ~b~~h~ " + lastLocation.Name + "~h~~w~");
                if (Game.IsControlJustPressed(0, Control.Context))
                {
                    TeleportPlayerOutOfBuilding(lastLocation);
                    lastLocation = null;
                }
            }
        }
        
        private async void TeleportPlayerIntoBuilding(Interior i)
        {
            Screen.Fading.FadeOut(500);
            await Delay(1000);
            Screen.Fading.FadeIn(500);
            Game.PlayerPed.Position = i.Exit;
            isPlayerInBuilding = true;
        }

        private async void TeleportPlayerOutOfBuilding(Interior i)
        {
            Screen.Fading.FadeOut(500);
            await Delay(1000);
            Screen.Fading.FadeIn(500);
            Game.PlayerPed.Position = i.Entrance;
            isPlayerInBuilding = false;
            currLocation = null;
        }
    }

    class Interior : BaseScript
    {
        public string Name;
        public Vector3 Entrance;
        public Vector3 Exit;
        public float Size;
    }
}
