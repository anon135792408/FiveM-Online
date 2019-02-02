using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using GTAOnline_FiveM;
using static CitizenFX.Core.Native.API;

namespace FiveM_Online_Client
{
    public class Apartment
    {
        private Vector3 purchasePosition;
        private Vector3 entrancePosition;
        private Vector3 exitPosition;
        private readonly long apartmentPrice;

        public Apartment()
        {
            purchasePosition = Vector3.Zero;
            entrancePosition = Vector3.Zero;
            exitPosition = Vector3.Zero;
            apartmentPrice = 0;
        }

        public Apartment(Vector3 purchasePosition, Vector3 entrancePosition, Vector3 exitPosition, long apartmentPrice)
        {
            this.purchasePosition = purchasePosition;
            this.entrancePosition = entrancePosition;
            this.exitPosition = exitPosition;
            this.apartmentPrice = apartmentPrice;
        }

        public bool IsOwnedByPlayer(Player p)
        {
            foreach (Apartment apt in FiveMOnline.onlinePlayer.ownedApartments)
            {
                if (apt.Equals(this))
                {
                    return true;
                }
            }
            return false;
        }

        public bool PurchaseApartment()
        {
            if (FiveMOnline.onlinePlayer.playerCash >= this.apartmentPrice)
            {
                FiveMOnline.onlinePlayer.ownedApartments.Add(this);
                return true;
            }
            return false;
        }

        public Vector3 PurchasePosition
        {
            get { return purchasePosition; }
            set { purchasePosition = value; }
        }

        public long ApartmentPrice
        {
            get { return apartmentPrice; }
        }
    }
}
