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
    class ScreenEvents : BaseScript
    {
        public ScreenEvents()
        {
            EventHandlers.Add("ScreenFade", new Action<int, int>(ScreenFade));
        }

        public void ScreenFade(int time, int fadeType) //fadeTypes = [0] For fading in, [1] For fading out
        {
            switch (fadeType)
            {
                case 0:
                    Screen.Fading.FadeIn(time);
                    break;
                case 1:
                    Screen.Fading.FadeOut(time);
                    break;
            }
        }
    }
}
