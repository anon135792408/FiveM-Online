using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace GTAOnline_FiveM
{
    public class ImpSpaces : BaseScript
    {
        public static IDictionary<Vector3, float> ValidImpounds = new Dictionary<Vector3, float>()
        {
            { new Vector3(420.79f, -1638.99f, 28.79f), 88.19f }
        };
    }
}
