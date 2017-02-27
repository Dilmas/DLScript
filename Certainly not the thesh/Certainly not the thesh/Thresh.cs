using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication.ExtendedProtection;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;

namespace Certainly_not_the_thesh
{

    public static class Champion
    {
        public static float ExtraAaRange = 120f;

            public static AIHeroClient MyHero
            {
                get { return ObjectManager.Player; }
            }

        public static Menu Menu,
            DrawMenu,
            ComboMenu,
            HarassMenu,
            PredictionMenu;

        private static Spell.Skillshot Q, W, E;
        public static Spell.Active Q2, R;
        //LatMana
        public static List<AIHeroClient> Enemies = new List<AIHeroClient>(), Allies = new List<AIHeroClient>();
        static int QMana { get { return 80; } }
        static int WMana { get { return 50 * W.Level; } }
        static int EMana { get { return 60 * E.Level; } }
        static int RMana { get { return R.Level > 0 ? 100 : 0; } }

            //Menu
            static void Main(string[] args)
            {
                Loading.OnLoadingComplete += Game_OnStart;
                Drawing.OnDraw += Game_OnDraw;
                Game.OnUpdate += Game_OnUpdate;

                Loading.OnLoadingComplete += Loading_OnLoadingComplete;
            }

            private static void Loading_OnLoadingComplete(EventArgs args)
            {
                
            }
            private static void Game_OnUpdate(EventArgs args)
            {
                var alvo = TargetSelector.GetTarget(1000, DamageType.Physical);

                if (!alvo.IsValid()) return;

                if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
                {
                    if (Q.IsReady() && ComboMenu["comboQ"].Cast<CheckBox>().CurrentValue)
                    {
                        Q.Cast();
                    }
                }
            }

            private static void Game_OnDraw(EventArgs args)
            {
                if (!DrawMenu["drawDisable"].Cast<CheckBox>().CurrentValue)
                {
                }
            }

            private static void Game_OnStart(EventArgs args)
            {
                Chat.Print("Remember it's certainly not the Thresh");
                //Menu Updated 1/29/2017

                Q = new Spell.Skillshot(SpellSlot.Q, 1040, SkillShotType.Linear, (int)0.5f, (int?)1900f, 70);
                Q.AllowedCollisionCount = 0;
                Q2 = new Spell.Active(SpellSlot.Q, 9000);
                W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Circular, 250, int.MaxValue, 10);
                W.AllowedCollisionCount = int.MaxValue;
                E = new Spell.Skillshot(SpellSlot.E, 480, SkillShotType.Linear, (int)0.25f, int.MaxValue, 50);
                E.AllowedCollisionCount = int.MaxValue;
                R = new Spell.Active(SpellSlot.R, 350);

                Menu = MainMenu.AddMenu("Thresh", "Thresh");
                Menu.AddSeparator();

                DrawMenu = Menu.AddSubMenu("Draw", "Draws");
                DrawMenu.Add("QRange", new CheckBox("Q Range", true));
                DrawMenu.Add("WRange", new CheckBox("W Range"));
                DrawMenu.Add("ERange", new CheckBox("E Range"));
                DrawMenu.Add("RRange", new CheckBox("R Range"));
                DrawMenu.Add("Disable", new CheckBox("Disable all Draws", true));

                ComboMenu = Menu.AddSubMenu("Combo", "ThreshCombo");
                ComboMenu.Add("comboQ", new CheckBox("Use Q in combo", true));
                ComboMenu.Add("comboW", new CheckBox("Use W in combo", true));
                ComboMenu.Add("comboE", new CheckBox("Use E in combo", true));
                ComboMenu.Add("comboR", new CheckBox("Use R in combo", true));

                HarassMenu.Add("HarrasQ", new CheckBox("Use Q", true));

            }
        }
    }

    
