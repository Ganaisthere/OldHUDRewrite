using HarmonyLib;

namespace OldHUDRewrite.Patches
{
    [HarmonyPatch(typeof(RandomEvent))]
    public class RandomEventPatches
    {
        [HarmonyPatch("Begin")]
        [HarmonyPrefix]
        public static void BeginPrefix(RandomEvent __instance)
        {
            if (BasePlugin.Instance.ConfigOldEventNotification.Value && OldEventNotification.Instance != null)
            {
                OldEventNotification.Instance.AnnounceEvent(__instance);
            }
        }
    }
}
