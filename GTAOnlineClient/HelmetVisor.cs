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
    class HelmetVisor : BaseScript
    {
        Ped playerPed;
        PedProp playerHelmet;

        Dictionary<int, int> helmetVisorsMale = new Dictionary<int, int>() //Helmet Visors {down, up}
        {
            {92, 93},
            {81, 82},
            {79, 80},
            {74, 75},
            {68, 83},
            {51, 69},
            {63, 73},
            {53, 71},
            {52, 70},
            {54, 72},
            {126, 127},
            {124, 125},
            {119, 120},
            {117, 118}
        };

        Dictionary<int, int> helmetVisorsFemale = new Dictionary<int, int>() //Helmet Visors {down, up}
        {
            {50, 68},
            {51, 69},
            {19, 67},
            {63, 72},
            {73, 74},
            {78, 79},
            {80, 81},
            {91, 92},
            {116, 117},
            {118, 119},
            {123, 124},
            {125, 126}
        };

        Dictionary<int, int> helmetVisors;

        public HelmetVisor()
        {
            Tick += OnTick;
            Tick += OnTickHelmetControl;
        }

        public async Task OnTick()
        {
            if (Game.PlayerPed != null)
            {
                 playerPed = Game.PlayerPed;
            }
            if (playerPed != null && playerPed.Model == PedHash.FreemodeMale01 || playerPed.Model == PedHash.FreemodeFemale01)
            {
                if (playerPed.Model == PedHash.FreemodeMale01)
                {
                    helmetVisors = helmetVisorsMale;
                }
                else if (playerPed.Model == PedHash.FreemodeFemale01)
                {
                    helmetVisors = helmetVisorsFemale;
                }
                else
                {
                    helmetVisors = null;
                }

                PedProp[] playerProps = playerPed.Style.GetAllProps();
                foreach (PedProp p in playerProps)
                {
                    if (p.ToString() == "Hats")
                    {
                        playerHelmet = p;
                    }
                }
            }
        }

        public async Task OnTickHelmetControl()
        {
            if (Game.IsControlJustPressed(0, Control.MultiplayerInfo))
            {
                if (playerHelmet != null && playerHelmet.HasAnyVariations)
                {
                    string animDict = "";
                    if (playerPed.IsOnFoot)
                    {
                        animDict = "anim@mp_helmets@on_foot";
                    }
                    else
                    {
                        animDict = "anim@mp_helmets@on_bike@sports";
                    }

                    for (int i = 0; i < helmetVisors.Count; i++)
                    {
                        var helmetIndex = helmetVisors.ElementAt(i);
                        if (helmetIndex.Key == playerHelmet.Index)
                        {
                            playerPed.Task.PlayAnimation(animDict, "visor_up");
                        }
                        else
                        {
                            playerPed.Task.PlayAnimation(animDict, "visor_down");
                        }
                        await Delay(500);
                        playerHelmet.SetVariation(helmetIndex.Value, playerHelmet.TextureIndex);
                    }
                }
            }
        }
    }
}
