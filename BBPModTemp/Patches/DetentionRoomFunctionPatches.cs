using HarmonyLib;
using LegacyUIs.Main;
using System;

namespace LegacyUIs.Patches
{
    [HarmonyPatch(typeof(DetentionRoomFunction))]
    public class DetentionRoomFunctionPatches
    {
        [HarmonyPatch("Activate", new Type[] { typeof(float), typeof(EnvironmentController) })]
        [HarmonyPostfix]
        public static void ActivatePostfix(DetentionRoomFunction __instance)
        {
            if (BasePlugin.Instance.ConfigOldYTPsDisplay.Value)
            {
                DetentionTimerDisplay.Create(__instance);
            }
        }
    }
}
