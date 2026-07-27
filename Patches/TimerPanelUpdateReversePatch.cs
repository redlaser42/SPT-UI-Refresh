using EFT.UI.BattleTimer;
using HarmonyLib;

namespace UIRefresh.Patches
{
    internal class TimerPanelUpdateReversePatch
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(TimerPanel), "UpdateTimer")]

        public static void UpdateTimer(TimerPanel instance)
        {

        }

    }
}
