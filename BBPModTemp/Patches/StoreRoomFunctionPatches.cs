using HarmonyLib;
using System;

namespace OldHUDRewrite.Patches
{
    [HarmonyPatch(typeof(StoreRoomFunction))]
    public class StoreRoomFunctionPatches
    {
        [HarmonyPatch("OnPlayerStay", new Type[] { typeof(PlayerManager) })]
        [HarmonyPostfix]
        public static void OnPlayerStayPostfix()
        {
            if (BasePlugin.Instance.ConfigOldYTPsDisplay.Value && OldPointsAnimation.Instance != null)
            {
                OldPointsAnimation.Instance.ShowDisplay(true);
            }
        }

        [HarmonyPatch("OnPlayerExit", new Type[] { typeof(PlayerManager) })]
        [HarmonyPostfix]
        public static void OnPlayerExitPostfix()
        {
            if (BasePlugin.Instance.ConfigOldYTPsDisplay.Value && OldPointsAnimation.Instance != null)
            {
                OldPointsAnimation.Instance.ShowDisplay(false);
            }
        }
    }
}
