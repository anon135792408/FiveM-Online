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
        public static List<VehicleHash> wantedVehicles = new List<VehicleHash> {
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
            VehicleHash.Dubsta,
            VehicleHash.Stratum,
            VehicleHash.Sultan,
            VehicleHash.Sentinel,
            VehicleHash.Gauntlet
        };

        public static Dictionary<Vector3, float> vehicleLocations = new Dictionary<Vector3, float> {
            { new Vector3(40.73f, -703.12f, 43.66f), 158.03f },
            { new Vector3(36.99f, -712.57f, 44.04f), 160.37f }
        };
    }
}
