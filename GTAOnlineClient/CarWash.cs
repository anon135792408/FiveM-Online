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
    class CarWash : BaseScript
    {
        //Constants
        const float UNIVERSAL_SIZE = 2.25f;

        //Variables
        Vector3 playerPos = Game.PlayerPed.Position;
        WashStation currLocation = null;
        WashStation lastLocation = null;
        Vehicle playerVeh = null;
        bool isBeingWashed = false;


        static WashStation[] CarWashes =
        {
            new WashStation {
                Name = "Hands On Car Wash",
                Entrance = new Vector3(52.38f,-1392.05f,28.89f),
                Heading = 89.7f,
                WashPos = new Vector3(23.84f,-1391.84f,28.83f),
                Exit = new Vector3(-5.71f,-1392.00f,28.83f),
            },
        };

        static VehicleClass[] AcceptedVehicles =
        {
            VehicleClass.Compacts,
            VehicleClass.Coupes,
            VehicleClass.Emergency,
            VehicleClass.Motorcycles,
            VehicleClass.Muscle,
            VehicleClass.OffRoad,
            VehicleClass.Sedans,
            VehicleClass.Sports,
            VehicleClass.SportsClassics,
            VehicleClass.Super,
            VehicleClass.SUVs,
            VehicleClass.Vans
        };

        public CarWash()
        {
            Tick += OnTick;
            Tick += WashInitiater;
            foreach (WashStation i in CarWashes)
            {
                Blip intBlip = World.CreateBlip(i.Entrance);
                intBlip.Sprite = BlipSprite.CarWash;
                intBlip.IsShortRange = true;
                intBlip.Name = i.Name;
            }
        }

        private async Task WashInitiater()
        {
            if (currLocation != null)
            {
                if (Game.PlayerPed.IsInVehicle())
                {
                    Screen.DisplayHelpTextThisFrame("Press ~b~~h~E~h~~w~ to wash your vehicle at ~b~~h~" + currLocation.Name + "~h~~w~.");
                    if (Game.IsControlJustPressed(0, Control.Context))
                    {
                        playerVeh = Game.PlayerPed.CurrentVehicle;
                        if (IsVehicleAppropriate())
                        {
                            if (playerVeh.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed)
                            {
                                InitiateCarWash(currLocation, playerVeh);
                            }
                            else
                            {
                                Screen.DisplayHelpTextThisFrame("You must be the driver of this vehicle.");
                                await Delay(2500);
                            }
                        }
                        else
                        {
                            Screen.DisplayHelpTextThisFrame("This vehicle cannot be washed.");
                        }
                    }
                }
                else
                {
                    Screen.DisplayHelpTextThisFrame("You must be in a vehicle to utilise this Car Wash.");
                }
            }
        }

        private async void InitiateCarWash(WashStation cWash, Vehicle veh)
        {
            lastLocation = currLocation;
            veh.Position = lastLocation.WashPos;
        }

        public bool IsVehicleAppropriate()
        {
            foreach (VehicleClass v in AcceptedVehicles)
            {
                if (v == playerVeh.ClassType)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task OnTick()
        {
            foreach (WashStation i in CarWashes)
            {
                if (!isBeingWashed)
                {
                    if (Game.PlayerPed.IsInRangeOf(i.Entrance, UNIVERSAL_SIZE))
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

    class WashStation : BaseScript
    {
        public string Name;
        public Vector3 Entrance;
        public float Heading;
        public Vector3 WashPos;
        public Vector3 Exit;
    }
}
