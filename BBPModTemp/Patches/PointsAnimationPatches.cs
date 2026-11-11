using HarmonyLib;
using LegacyUIs.Main;
using System;

namespace LegacyUIs.Patches
{
    [HarmonyPatch(typeof(PointsAnimation))]
    public class PointsAnimationPatches
    {
        [HarmonyPatch("ShowDisplay", new Type[] { typeof(bool) })]
        [HarmonyPrefix]
        public static bool ShowDisplayPrefix(bool val)
        {
            if (BasePlugin.Instance.ConfigOldYTPsDisplay.Value)
            {
                OldPointsAnimation.Instance.ShowDisplay(val);
                return false;
            }
            return true;
        }

        [HarmonyPatch("AddScore", new Type[] { typeof(int), typeof(int), typeof(int) })]
        [HarmonyPrefix]
        public static bool AddScorePrefix(int points)
        {
            if (BasePlugin.Instance.ConfigOldYTPsDisplay.Value)
            {
                OldPointsAnimation.Instance.AddScore(points);
                return false;
            }
            return true;
        }
    }
}
