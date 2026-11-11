using HarmonyLib;

namespace LegacyUIs.Patches
{
    [HarmonyPatch(typeof(EnvironmentController))]
    public class EnvironmentControllerPatches
    {
        [HarmonyPatch("Awake"), HarmonyPrefix]
        public static void AwakePrefix(EnvironmentController __instance)
        {
            if (BasePlugin.Instance.ConfigOldEventNotification.Value)
            {
                SoundObject EventRing = BasePlugin.assetMan.Get<SoundObject>("EventRing");
                if (EventRing != null)
                {
                    __instance.AccessToolsFieldSetValue("audEventNotification", EventRing);
                }
            }
        }
    }
}
