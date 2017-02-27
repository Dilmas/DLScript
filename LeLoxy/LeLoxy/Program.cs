using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeLoxy
{
    class Program
    {
        public static Spell.Targeted Q;
        public static Spell.Targeted QUltimate, RTargeted;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot WUltimate, EUltimate;
        public static Spell.Active WReturn, RActive, RReturn;
        public static Spell.Targeted Ignite;
        public static int PassiveStacks
        {
            get
            {
                int stacks = 0;
                if (_Player.HasBuff("Stack"))
                    stacks = _Player.GetBuff("Stack").Count;
                return stacks;
            }
        }
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
            Drawing.OnDraw += Game_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

       private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Leblanc)
            {
                return;
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {

        }

        private static void Game_OnDraw(EventArgs args)
        {

        }

        private static void Game_OnStart(EventArgs args)
        {
             Q = new Spell.Targeted(SpellSlot.Q, 700);

            W = new Spell.Skillshot(SpellSlot.W, 600, SkillShotType.Circular, 0, 1600, 260)
            {
                AllowedCollisionCount = -1
            };

            WReturn = new Spell.Active(SpellSlot.W) {CastDelay = 250};

            RReturn = new Spell.Active(SpellSlot.R);

            E = new Spell.Skillshot(SpellSlot.E, 950, SkillShotType.Linear, 250, 1750, 55)
            {
                AllowedCollisionCount = 0
            };

            QUltimate = new Spell.Targeted(SpellSlot.Q, 700);

            WUltimate = new Spell.Skillshot(SpellSlot.W, 600, SkillShotType.Circular, 0, 1600, 250)
            {
                AllowedCollisionCount = -1
            };

            EUltimate = new Spell.Skillshot(SpellSlot.E, 950, SkillShotType.Linear, 250, 1750, 55)
            {
                AllowedCollisionCount = 0
            };

            RActive = new Spell.Active(SpellSlot.R);

            RTargeted = new Spell.Targeted(SpellSlot.R, int.MaxValue);
        }
    }
}