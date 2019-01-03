using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using NativeUI;

namespace GTAOnline_FiveM
{
    class ConvenienceStore : BaseScript
    {
        List<Vector3> blipCoordinates = new List<Vector3>();

        public ConvenienceStore()
        {
            SetupBlipCoordinates();
        }

        void SetupBlipCoordinates()
        {
            blipCoordinates.Add(new Vector3(-50f, -1753f, 29f));
            blipCoordinates.Add(new Vector3(29f, -1345f, 29f));
            blipCoordinates.Add(new Vector3(1136f, -981f, 46f));
            blipCoordinates.Add(new Vector3(1158f, -322f, 69f));
            blipCoordinates.Add(new Vector3(2555f, 385f, 108f));
            blipCoordinates.Add(new Vector3(377f, 327f, 103f));
            blipCoordinates.Add(new Vector3(-711f, -911f, 19f));
            blipCoordinates.Add(new Vector3(-1224f, -906f, 12f));
            blipCoordinates.Add(new Vector3(-1488f, -380f, 40f));
            blipCoordinates.Add(new Vector3(-1825f, 791f, 138f));
            blipCoordinates.Add(new Vector3(-2969f, 390f, 15f));
            blipCoordinates.Add(new Vector3(-3041f, 588f, 7f));
            blipCoordinates.Add(new Vector3(-3244f, 1004f, 12f));
            blipCoordinates.Add(new Vector3(544f, 2668f, 42f));
            blipCoordinates.Add(new Vector3(2678f, 3284f, 55f));
            blipCoordinates.Add(new Vector3(1963f, 3744f, 32f));
            blipCoordinates.Add(new Vector3(1393f, 3603f, 34f));
            blipCoordinates.Add(new Vector3(1702f, 4927f, 42f));
            blipCoordinates.Add(new Vector3(1733f, 6415f, 35f));

            foreach (Vector3 coordinate in blipCoordinates)
            {
                Blip b = World.CreateBlip(coordinate);
                b.Sprite = BlipSprite.Store;
                b.Name = "Store";
                b.IsShortRange = true;
            }
        }

    }
}