using HarmonyLib;
using LegacyUIs.Main;

namespace LegacyUIs.Patches
{
    [HarmonyPatch(typeof(ITM_Nametag))]
    public class ITMNametagPatches
    {
        [HarmonyPatch("Use")]
        [HarmonyPostfix]
        public static void UsePostfix(PlayerManager pm, ITM_Nametag __instance)
        {
            if (BasePlugin.Instance.ConfigItemUsingAnimations.Value)
            {
                NametagAnimation.Create(pm.playerNumber, __instance);
            }
        }
    }
}
