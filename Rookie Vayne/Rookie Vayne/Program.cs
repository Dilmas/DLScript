using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using EloBuddy;

namespace Rookie_Vayne
{
    class Program
    {
        public static Menu Menu,
            VayneCombo,
            Harass;

        static float Vayne1, Vayne2, Vayne3, Vayne4, Vayne5;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (!Player.Instance.ChampionName.ToLower().Contains("vayne"))
                return;
            Menu = MainMenu.AddMenu("RookieVayne", "Vayne");
            Game.OnUpdate += Game_OnTick;
            Obj_AI_Base.OnBasicAttack += Obj_AI_Base_OnBasicAttack;
            Obj_AI_Base.OnBuffGain += Obj_AI_Base_OnBuffGain;
            Loading.OnLoadingComplete += Game_OnStart;
            Drawing.OnDraw += Game_OnDraw;
            Game.OnUpdate += Game_OnUpdate;

        }

        private static void Game_OnStart(EventArgs args)
        {
            VayneCombo = Menu.AddSubMenu("Combo", "Combo");

            VayneCombo.Add("CQ", new CheckBox("Use Q", true));
            VayneCombo.Add("EO", new CheckBox("E Options"));
            VayneCombo.Add("EC", new CheckBox("Use E To Peel", true));
            VayneCombo.Add("ES", new CheckBox("E To Stun", true));
            VayneCombo.Add("CR", new CheckBox("Use R", true));
            VayneCombo.Add("BORK", new CheckBox("Use BORK", true));
            VayneCombo.Add("Bilge", new CheckBox("Use Bilge", true));
            VayneCombo.Add("RotSec", new CheckBox("ZZRot Condemn", true));
            VayneCombo.Add("CF", new CheckBox("Condemn Flash Combo", true));

            VayneCombo = Menu.AddSubMenu("Harass", "Harass");

            VayneCombo.Add("HQ", new CheckBox("Use Q", true));
            VayneCombo.Add("HEO", new CheckBox("E Options"));
            VayneCombo.Add("HEC", new CheckBox("Use E To Peel", true));
            VayneCombo.Add("HES", new CheckBox("Use E To Stun", true));

            VayneCombo = Menu.AddSubMenu("LaneClear", "LeneClear");

            VayneCombo.Add("LCQ", new CheckBox("Use Q", true));

            VayneCombo = Menu.AddSubMenu("JungleClear", "JungleClear");

            VayneCombo.Add("JCQ", new CheckBox("Use Q", true));
            VayneCombo.Add("JCE", new CheckBox("Use E", true));

            VayneCombo = Menu.AddSubMenu("KillSteal", "KillSteal");

            VayneCombo.Add("KSQ", new CheckBox("Use Q", true));
            VayneCombo.Add("KSW", new CheckBox("Use W", true));

            VayneCombo = Menu.AddSubMenu("Skin", "Skin");

            Menu.AddLabel("Under Development");

        }

        private static void Game_OnDraw(EventArgs args)
        {

        }

        private static void Game_OnUpdate(EventArgs args)
        {

        }

        private static void Game_OnTick(EventArgs args)
        {

        }

        static void VayneMenu()
        {
            if (Player.CanUseSpell(SpellSlot.Q) == SpellState.Ready && Game.Time > Vayne1 + Vayne3 + 0.025f && Game.Time < Vayne1 + (Vayne4 * 0.75f))
            {
                Player.CastSpell(SpellSlot.Q, Game.CursorPos);
                return;
            }
            var target = GetAATarget(Player.Instance.AttackRange + Player.Instance.BoundingRadius);
            if (target == null)
            {
                if (Game.Time > Vayne1 + Vayne3 + 0.025f && Game.Time > Vayne4 + 0.150f)
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                    Vayne4 = Game.Time;
                }
                return;
            }
            if (Game.Time > Vayne1 + Vayne2)
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                return;
            }
            if (Game.Time > Vayne1 + Vayne5 + 0.025f && Game.Time > Vayne4 + 0.150f)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                Vayne3 = Game.Time;
            }
        }

        static AttackableUnit GetAATarget(float range)
        {
            AttackableUnit t = null;
            float num = 10000;
            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                float hp = enemy.Health;
                if (enemy.IsValidTarget(range + enemy.BoundingRadius) && hp < num)
                {
                    num = hp;
                    t = enemy;
                }
            }
            return t;
        }

        static void Obj_AI_Base_OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                Vayne3 = sender.AttackCastDelay;
                Vayne4 = sender.AttackDelay;
                Vayne1 = Game.Time;
            }
        }

        static void Obj_AI_Base_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (sender.IsMe && args.Buff.Name == "")
            {
                Vayne1 = 0;
            }

        }

    }
}