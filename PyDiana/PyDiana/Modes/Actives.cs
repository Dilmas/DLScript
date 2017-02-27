using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using PyDiana;

namespace PyDiana.Modes
{

    class Actives
    {
        private static AIHeroClient Player { get { return ObjectManager.Player; } }
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }
        public static Menu Menu, PyCombo, PyHarass, KillSteal, Lane, Jungle;

        private static void Game_OnTick(EventArgs args)
        {
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Combo();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    Harass();
                    break;
                case Orbwalker.ActiveModes.LaneClear:
                    LaneClear();
                    break;
                case Orbwalker.ActiveModes.JungleClear:
                    JungleClear();
                    break;
                case Orbwalker.ActiveModes.LastHit:
                    break;
                case Orbwalker.ActiveModes.None:
                    break;
            }
            
        }

    private static void Combo()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (target == null)
                return;
            if (PyCombo["UseQ"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, Q.Range) && Q.IsReady())
                {

                    if (Q.GetPrediction(target).HitChance >= HitChance.High)
                    {
                        Q.Cast(Q.GetPrediction(target).CastPosition);
                    }


                }
            }
            if (PyCombo["UseW"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, W.Range) && W.IsReady())
                {

                    W.Cast();



                }
            }
            if (PyCombo["UseE"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, E.Range) && E.IsReady())
                {

                    E.Cast();


                }
            }
            if (PyCombo["UseR"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, R.Range) && !Q.IsReady()) 
                {

                    R.Cast(target);



                }
            }


        }
        private static void Harass()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (target == null)
                return;
            if (PyHarass["UseQ"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, Q.Range) && Q.IsReady())
                {

                    if (Q.GetPrediction(target).HitChance >= HitChance.High)
                    {
                        Q.Cast(Q.GetPrediction(target).CastPosition);
                    }


                }
            }
            if (PyHarass["UseW"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(Player, W.Range) && W.IsReady())
                {

                    W.Cast();



                }
            }



        }

        private static void LaneClear()
        {
            if (Lane["Qclearmana"].Cast<Slider>().CurrentValue <= Player.ManaPercent)
            {
                var minion1 = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Q.Range));

                Q.Cast(minion1);

            }
            if (Lane["Wclearmana"].Cast<Slider>().CurrentValue <= Player.ManaPercent)
            {
                var minion1 = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(W.Range));

                W.Cast();

            }

        }
        private static void JungleClear()
        {

            if (Lane["Qclearjg"].Cast<CheckBox>().CurrentValue)
            {
                var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Position, Q.Range);
                Q.Cast(monster.First());
            }
            if (Lane["Wclearjg"].Cast<CheckBox>().CurrentValue)
            {
                var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Position, W.Range);
                W.Cast();
            }
            if (Lane["Rclearjg"].Cast<CheckBox>().CurrentValue)
            {
                var monster = EntityManager.MinionsAndMonsters.Monsters.FirstOrDefault(m => m.IsValidTarget(R.Range));
                R.Cast(monster);
            }

        }
    }
}

    

