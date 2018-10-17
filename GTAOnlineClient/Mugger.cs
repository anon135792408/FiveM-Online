using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace GTAOnlineClient
{
    class Mugger : BaseScript
    {
        Ped target;
        Ped muggerPed;

        public Mugger()
        {
            EventHandlers.Add("SetMuggerOnPlayerPed", new Action<int>(SetMuggerOnPlayerPed));
        }

        private async void SetMuggerOnPlayerPed(int targetId)
        {
            Player player = new PlayerList()[targetId];
            target = player.Character;
            TriggerEvent("chatMessage", "MUGGER:", new[] { 0, 0, 200 }, "Mugger called on player.");
            muggerPed = await World.CreatePed(PedHash.FibMugger01, World.GetNextPositionOnSidewalk(Game.PlayerPed.Position + new Vector3(50, 50, 1)));
            muggerPed.Weapons.Give(WeaponHash.Knife, 1, true, true);
            muggerPed.Task.FightAgainst(target);
            Tick += OnTick;
        }

        private async Task OnTick()
        {
            CheckPlayerStatus();
            CheckMuggerStatus();
        }

        private void CheckPlayerStatus()
        {
            if (target.IsDead)
            {
                muggerPed.Task.ClearAll();
                muggerPed.Task.WanderAround();
                TriggerEvent("chatMessage", "MUGGER:", new[] { 0, 0, 200 }, "Mugger successfully mugged target.");
                Tick -= OnTick;
            }
        }

        private void CheckMuggerStatus()
        {
            if (muggerPed.IsDead)
            {
                TriggerEvent("chatMessage", "MUGGER:", new[] { 0, 0, 200 }, "Mugger set on player has been killed.");
                Tick -= OnTick;
            }
        }
    }
}
