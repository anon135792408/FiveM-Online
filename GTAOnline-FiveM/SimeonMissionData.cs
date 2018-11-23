using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace GTAOnline_FiveM
{
    public class SimeonMissionData
    {
        public static List<VehicleHash> wantedVehicles = new List<VehicleHash>
        {
            VehicleHash.Blista,
            VehicleHash.Asterope,
            VehicleHash.Asea,
            VehicleHash.Baller,
            VehicleHash.Tailgater,
            VehicleHash.Oracle,
            VehicleHash.Oracle2,
            VehicleHash.Patriot,
            VehicleHash.Premier,
            VehicleHash.Penumbra,
            VehicleHash.Prairie,
            VehicleHash.Buccaneer,
            VehicleHash.Buccaneer2,
            VehicleHash.Buffalo,
            VehicleHash.Buffalo2,
            VehicleHash.Cavalcade,
            VehicleHash.Cavalcade2,
            VehicleHash.Dubsta
        };

        public static Dictionary<Vector3, float> vehicleLocations = new Dictionary<Vector3, float>
        {
            { new Vector3(-65.79f, -1315.56f, 28.99f), 89.56f },
            { new Vector3(-941.50f, -2166.14f, 29.88f), 174.88f },
            { new Vector3(399.58f, -2005.71f, 22.53f), 157.67f },
            { new Vector3(88.69f, -1967.63f, 20.08f), 320.41f },
            { new Vector3(-1039.76f, -2641.36f, 15.81f), 150.30f },
            { new Vector3(-308.25f, -951.01f, 30.27f), 70.43f },
            { new Vector3(-87.13f, 369.54f, 111.64f), 244.91f }
        };
    }
}
