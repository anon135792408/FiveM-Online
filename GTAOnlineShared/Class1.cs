using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace GTAOnlineShared
{
    public class Class1
    {
        private static bool testingSharedStuff = true;

        public static string getBool()
        {
            Debug.WriteLine(testingSharedStuff.ToString());
            return testingSharedStuff.ToString();
        }

        public static void setBool(bool testingSharedStuffNew)
        {
            testingSharedStuff = testingSharedStuffNew;
        }
    }
}
