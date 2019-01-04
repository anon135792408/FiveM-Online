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
    public class AdditionalMaths : BaseScript
    {
        public static Random rnd = new Random();

        public static int GetRandomOperand()// 0 for +ve, 1 for -ve
        {
            return rnd.Next(0, 1);
        }

        public static Vector3 GetAroundVector3(Vector3 v, int minDistance, int maxDistance)
        {
            Vector3 result = Vector3.Zero;

            float ranX;
            float ranY;

            int ranOperand = GetRandomOperand();
            int ranOperandLayer2 = GetRandomOperand();

            ranX = v.X + rnd.Next(minDistance, maxDistance);
            ranY = v.Y + rnd.Next(minDistance, maxDistance);

            switch (ranOperand)
            {
                case 0:
                    if (ranOperandLayer2 == 1)
                    {
                        result = v + new Vector3(ranX, -ranY, v.Z);
                    }
                    else
                    {
                        result = v + new Vector3(ranX, ranY, v.Z);
                    }
                    break;
                case 1:
                    if (ranOperandLayer2 == 1)
                    {
                        result = v + new Vector3(-ranX, -ranY, v.Z);
                    }
                    else
                    {
                        result = v + new Vector3(-ranX, ranY, v.Z);
                    }
                    break;
            }

            return result;
        }
    }
}
