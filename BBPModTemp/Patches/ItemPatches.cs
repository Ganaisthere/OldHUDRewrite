using HarmonyLib;
using UnityEngine;

namespace OldHUDRewrite.Patches
{
    [HarmonyPatch(typeof(ItemManager))]
    public class ItemPatches
    {
        [HarmonyPatch("UpdateSelect")]
        [HarmonyPrefix]
        public static void UpdateSelectPostfix(ItemManager __instance)
        {
            bool one = BasePlugin.Instance.ConfigBiggerItemIcon.Value;
            bool two = BasePlugin.Instance.ConfigFGPDNothingItemIcon.Value;
            if (!one && !two)
            {
                return;
            }
            for (int i = 0; i <= __instance.maxItem; i++)
            {
                Sprite sprite = Other.FindObjectOfTypeWithName<Sprite>("Transparent");
                if (__instance.items[i].itemType == Items.None)
                {
                    if (two)
                    {
                        sprite = BasePlugin.assetMan.Get<Sprite>("PlaceHolder");
                    }
                }
                else if (BasePlugin.Instance.ConfigBiggerItemIcon.Value)
                {
                    sprite = __instance.items[i].itemSpriteLarge;
                }
                else
                {
                    sprite = __instance.items[i].itemSpriteSmall;
                }
                Singleton<CoreGameManager>.Instance.GetHud(__instance.pm.playerNumber).UpdateItemIcon(i, sprite);
            }
        }
    }
}
