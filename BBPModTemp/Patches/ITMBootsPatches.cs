using HarmonyLib;

namespace OldHUDRewrite.Patches
{
    [HarmonyPatch(typeof(ITM_Boots))]
    public class ITMBootsPatches
    {
        [HarmonyPatch("Use")]
        [HarmonyPostfix]
        public static void UsePostfix(PlayerManager pm, ITM_Boots __instance)
        {

        }
    }
}
