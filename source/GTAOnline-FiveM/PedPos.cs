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
    class PedPos : BaseScript
    {
        public Vector3 Position;
        public float Heading;

        public PedPos()
        {
            Position = Vector3.Zero;
            Heading = 0.0f;
        }

        public PedPos(Vector3 position, float heading)
        {
            Position = position;
            Heading = heading;
        }
    }
}
