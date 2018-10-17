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

        Dictionary<int, int> helmetVisors = new Dictionary<int, int>() //Helmet Visors {down, up}
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
                            await Delay(500);
                            playerHelmet.SetVariation(helmetIndex.Value, playerHelmet.TextureIndex);
                        }
                        else if (helmetIndex.Value == playerHelmet.Index)
                        {
                            playerPed.Task.PlayAnimation(animDict, "visor_down");
                            await Delay(500);
                            playerHelmet.SetVariation(helmetIndex.Key, playerHelmet.TextureIndex);
                        }
                    }
                }
            }
        }
    }
}
