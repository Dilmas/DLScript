using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using Champion = DLFiora.Model.Champion;

namespace DLFiora
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += GameLoaded;
        }

        private static void GameLoaded(EventArgs args)
        {
            if (Player.Instance.ChampionName == "Fiora")
            {
                new Champion().Init();
            }
        }
    }
}