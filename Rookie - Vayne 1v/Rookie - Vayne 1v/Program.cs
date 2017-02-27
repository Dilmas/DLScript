using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace Rookie___Vayne_1v
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Vayne") return;

            MenuManager.Initialize();
            Chat.OnInput += ChatInputComing;
            MenuManager.Modes.Gosu.UnSetGod();
            new Brain().Initialize();
        }

        private static void ChatInputComing(ChatInputEventArgs args)
        {
            if (args.Input == "#IDDQD" && !MenuManager.IDDQD)
            {
                args.Input = "";
                Chat.Print("Good luck Load Complete.");
                MenuManager.IDDQD = true;
                MenuManager.Modes.Gosu.SetGod();
            }
            else if (args.Input == "#IDDQD" && MenuManager.IDDQD)
            {
                args.Input = "";
            }
        }
    }
}
