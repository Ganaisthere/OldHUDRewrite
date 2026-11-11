using HarmonyLib;
using LegacyUIs.Main;
using UnityEngine;

namespace LegacyUIs.Patches
{
    [HarmonyPatch(typeof(ItemManager))]
    public class ItemManagerPatches
    {
        [HarmonyPatch("UpdateSelect")]
        [HarmonyPrefix]
        public static void UpdateSelectPostfix(ItemManager __instance)
        {
            if (OldItemSlots.Instance != null)
            {
                return;
            }
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
                else if (one)
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
