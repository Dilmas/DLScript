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
using PyDiana.Modes;

namespace PyDiana
{
    class Program
    {
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }
        public static Menu Menu, PyCombo, PyHarass, KillSteal, Lane, Jungle;

        //Remind me to update this
        public static Spell.Skillshot Q;
        //Remind me to update this
        public static Spell.Active W;
        //Remind me to update this
        public static Spell.Active E;
        //Remind me to update this
        public static Spell.Targeted R, R2;
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Linear);
            {
                Q.AllowedCollisionCount = int.MaxValue;

            }
            W = new Spell.Active(SpellSlot.W, 250);
            E = new Spell.Active(SpellSlot.E, 350);
            R = new Spell.Targeted(SpellSlot.R, 825);

            Menu = MainMenu.AddMenu("Diana", "Diana 2.0");

            PyCombo = Menu.AddSubMenu("Combo");
            //combo
            PyCombo.Add("comboQ", new CheckBox("Use Q ", true));
            PyCombo.Add("comboW", new CheckBox("Use W ", true));
            PyCombo.Add("comboE", new CheckBox("Use E ", true));
            PyCombo.Add("comboR", new CheckBox("Use R", true));

            PyHarass = Menu.AddSubMenu("Harass");
            //Harass
            PyHarass.Add("Q", new CheckBox("Use Q"));
            PyHarass.Add("W", new CheckBox("Use W"));
            PyHarass.Add("E", new CheckBox("Use E", false));
            PyHarass.Add("ManaPercent", new Slider("Minimum Mana Percent", 25));

            KillSteal = Menu.AddSubMenu("KillSteal");
            //KS
            KillSteal.Add("Q", new CheckBox("Use Q"));
            KillSteal.Add("W", new CheckBox("Use W"));
            KillSteal.Add("R", new CheckBox("Use R"));

            Lane = Menu.AddSubMenu("LaneClear");
            //LaneClear
            Lane.Add("LaneClear.Q", new Slider("Use Q >= {0}", 4, 0, 10));
            Lane.Add("LaneClear.W", new Slider("Use W >= {0}", 3, 0, 10));
            Lane.Add("LaneClear.ManaPercent", new Slider("Minimum Mana Percent", 50));

            Jungle = Menu.AddSubMenu("Jungleclear");
            //Jungleclear
            Jungle.Add("JungleClear.Q", new CheckBox("Use Q"));
            Jungle.Add("JungleClear.W", new CheckBox("Use W"));
            Jungle.Add("JungleClear.ManaPercent", new Slider("Minimum Mana Percent", 20));
        }
    }
}