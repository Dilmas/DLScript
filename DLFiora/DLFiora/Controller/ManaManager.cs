using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Menu;
using DLFiora.Util.Misc;

namespace DLFiora.Controller
{
    public static class ManaManager
    {

        public static bool CanUseSpell(Menu menu , string proprety)
        {
            return Player.Instance.ManaPercent >= Misc.GetSliderValue(menu, proprety);
        }
    }
}
