﻿using System.Collections.Generic;
using System.Linq;
using DLFiora.Helpers;
using DLFiora.Model;
using DLFiora.Model.Enum;
using DLFiora.Model;
using EloBuddy;
using EloBuddy.SDK;
using DLFiora.Util.Misc;

namespace DLFiora.Controller.Modes
{
    public sealed class LaneClear : ModeBase
    {

        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            var q = PluginModel.Q;
            var w = PluginModel.W;

            var minionTarget = EntityManager.MinionsAndMonsters.EnemyMinions.Aggregate((curMin, x) => (curMin == null || x.HealthPercent < curMin.Health ? x : curMin));

            if(minionTarget == null || !minionTarget.IsValidTarget()) return;

            if (q.IsReady() && Misc.IsChecked(PluginModel.LaneClearMenu, "lcQ") && ManaManager.CanUseSpell(PluginModel.LaneClearMenu, "lcMana") && q.IsInRange(minionTarget))
            {
                q.Cast(minionTarget);
            }

            if (w.IsReady() && Misc.IsChecked(PluginModel.LaneClearMenu, "lcW") && ManaManager.CanUseSpell(PluginModel.LaneClearMenu, "lcMana") && w.IsInRange(minionTarget) && !Orbwalker.CanAutoAttack && !Player.Instance.IsInAutoAttackRange(minionTarget))
            {
                w.Cast(minionTarget);
            }
        }
    }
}
