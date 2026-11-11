using HarmonyLib;
using System;
using UnityEngine;

namespace LegacyUIs.Patches
{
    //Copyright (c) 2026 Blitzo228
    //Licensed under the CC BY-NC-ND 4.0 License
    [HarmonyPatch(typeof(Map))]
    public static class MapPatches
    {
        public static Material OpaqueMat { get; internal set; }

        public static void CreateOpaqueMaterial()
        {
            if (OpaqueMat == null)
            {
                Shader shader = Shader.Find("Sprites/Default");

                OpaqueMat = new Material(shader);

                OpaqueMat.SetOverrideTag("RenderType", "Opaque");
                OpaqueMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                OpaqueMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                OpaqueMat.SetInt("_ZWrite", 1);
                OpaqueMat.DisableKeyword("_ALPHATEST_ON");
                OpaqueMat.DisableKeyword("_ALPHABLEND_ON");
                OpaqueMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                OpaqueMat.renderQueue = -1;
            }
        }

        [HarmonyPatch("Initialize", new Type[] { typeof(EnvironmentController), typeof(IntVector2) }), HarmonyPostfix]
        public static void InitializePostfix(Map __instance)
        {
            if (BasePlugin.Instance.ConfigOldMapColor.Value)
            {
                CreateOpaqueMaterial();

                for (int i = 0; i < __instance.size.x; i++)
                {
                    for (int j = 0; j < __instance.size.z; j++)
                    {
                        MapTile tile = __instance.tiles[i, j];
                        if (tile != null)
                        {
                            var renderer = tile.SpriteRenderer;
                            if (renderer != null)
                            {
                                Color color = renderer.color;
                                color.a = 1f;

                                renderer.sharedMaterial = OpaqueMat;
                                renderer.color = color;

                                if (__instance.foundTiles[i, j])
                                {
                                    renderer.enabled = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch("Update"), HarmonyPostfix]
        public static void Update_Postfix(Map __instance)
        {
            if (BasePlugin.Instance.ConfigOldMapColor.Value)
            {
                for (int i = 0; i < __instance.size.x; i++)
                {
                    for (int j = 0; j < __instance.size.z; j++)
                    {
                        MapTile tile = __instance.tiles[i, j];
                        if (tile != null)
                        {
                            var renderer = tile.SpriteRenderer;
                            if (renderer != null)
                            {
                                Color color = renderer.color;
                                if (color.a < 1f)
                                {
                                    color.a = 1f;
                                    renderer.color = color;
                                }

                                if (renderer.sharedMaterial != OpaqueMat)
                                {
                                    Color currentColor = renderer.color;
                                    renderer.sharedMaterial = OpaqueMat;
                                    renderer.color = currentColor;
                                }

                                if (__instance.foundTiles[i, j])
                                {
                                    renderer.enabled = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch("Find", new Type[] { typeof(int), typeof(int), typeof(int), typeof(RoomController) }), HarmonyPostfix]
        public static void Find_Postfix(Map __instance, int posX, int posZ)
        {
            if (BasePlugin.Instance.ConfigOldMapColor.Value)
            {
                MapTile tile = __instance.tiles[posX, posZ];
                if (tile != null)
                {
                    var renderer = tile.SpriteRenderer;
                    if (renderer != null)
                    {
                        Color color = renderer.color;
                        color.a = 1f;
                        renderer.color = color;
                        renderer.sharedMaterial = OpaqueMat;
                        renderer.enabled = true;
                    }
                }
            }
        }

        [HarmonyPatch("AddArrow", new Type[] { typeof(Entity), typeof(Color) }), HarmonyPrefix]
        public static bool AddArrowPrefix(Entity target)
        {
            if (BasePlugin.Instance.ConfigOldMapColor.Value && target.gameObject.TryGetComponent<PlayerManager>(out _))
            {
                return false;
            }
            return true;
        }
    }
}