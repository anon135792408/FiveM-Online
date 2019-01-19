using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using NativeUI;

namespace FiveM_Online_Client
{
    class Apartments : BaseScript
    {
        private bool isNearApartment = false;
        private Apartment closestApt;

        private Apartment[] apartments = new Apartment[]
        {
            new Apartment(new Vector3(-819f, -989f, 13.6f), Vector3.Zero, Vector3.Zero, 10000),
        };

        public Apartments()
        {
            Tick += RenderBuyCheckpoints;
            Tick += BuyPromptCheck;
        }

        private async Task RenderBuyCheckpoints()
        {
            foreach (Apartment apt in apartments)
            {
                if (!apt.IsOwnedByPlayer(Game.Player))
                {
                    if (apt.PurchasePosition.DistanceToSquared(Game.PlayerPed.Position) <= 200f)
                    {
                        World.DrawMarker(MarkerType.VerticalCylinder, apt.PurchasePosition + new Vector3(0f, 0f, -1f), Vector3.Zero, Vector3.Zero, Vector3.One, System.Drawing.Color.FromArgb(180, 66, 134, 244));

                        if (apt.PurchasePosition.DistanceToSquared(Game.PlayerPed.Position) <= 5f)
                        {
                            isNearApartment = true;
                            closestApt = apt;
                        }
                        else
                        {
                            isNearApartment = false;
                        }
                    }
                }
            }
        }

        private async Task BuyPromptCheck()
        {
            if (isNearApartment)
            {
                Screen.DisplayHelpTextThisFrame("Press E to purchase this property for $" + closestApt.ApartmentPrice.ToString());
            }
        }
    }
}
