using HarmonyLib;
using LegacyUIs.Main;

namespace LegacyUIs.Patches
{
    [HarmonyPatch(typeof(ITM_Boots))]
    public class ITMBootsPatches
    {
        [HarmonyPatch("Use")]
        [HarmonyPostfix]
        public static void UsePostfix(PlayerManager pm, ITM_Boots __instance)
        {
            if (BasePlugin.Instance.ConfigItemUsingAnimations.Value)
            {
                BootsAnimation.Create(pm.playerNumber, __instance);
            }
        }
    }

}
