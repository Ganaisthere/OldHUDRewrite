using HarmonyLib;
using UnityEngine;

namespace LegacyUIs.Patches
{
    //Copyright (c) 2026 Blitzo228
    //Licensed under the CC BY-NC-ND 4.0 License
    [HarmonyPatch(typeof(MapTile))]
    public class MapTilePatches
    {
        [HarmonyPatch("Reveal"), HarmonyPostfix]
        public static void RevealPostfix(MapTile __instance)
        {
            OldMap oldMap = BasePlugin.Instance.ConfigOldMap.Value;
            if (oldMap == OldMap.OnlyColor || oldMap == OldMap.ColorAndMapScreen)
            {
                SpriteRenderer renderer = __instance.SpriteRenderer;
                if (renderer != null)
                {
                    Color color = renderer.color;
                    color.a = 1f;
                    renderer.color = color;

                    if (MapPatches.OpaqueMat != null)
                    {
                        renderer.sharedMaterial = MapPatches.OpaqueMat;
                    }
                    renderer.enabled = true;
                }
            }
        }
    }
}
