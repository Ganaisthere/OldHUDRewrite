using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LegacyUIs.Patches
{
    //Copyright (c) 2026 Blitzo228
    //Licensed under the CC BY-NC-ND 4.0 License
    [HarmonyPatch(typeof(Map))]
    public static class MapPatches
    {
        public static Material OpaqueMat { get; internal set; }
        public static Dictionary<RoomCategory, Color> roomColors = new Dictionary<RoomCategory, Color>()
        {
            {RoomCategory.Faculty, Color.yellow},
            {RoomCategory.Class, Color.red},
            {RoomCategory.Office, Color.gray},
            {RoomCategory.Hall, Color.green},
            {RoomCategory.Buffer, Color.white},
            {RoomCategory.Special, Color.white},
            {RoomCategory.Store, Color.white}
        };

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
            OldMap oldMap = BasePlugin.Instance.ConfigOldMap.Value;
            if (oldMap == OldMap.OnlyColor || oldMap == OldMap.ColorAndMapScreen)
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
            if (oldMap == OldMap.OnlyMapScreen || oldMap == OldMap.ColorAndMapScreen)
            {
                Transform overlayTransform = __instance.transform.Find("Overlay");
                overlayTransform.Find("ZoomButtons").gameObject.SetActive(false);
                overlayTransform.Find("Dpad").gameObject.SetActive(false);
                overlayTransform.Find("Timer").gameObject.SetActive(false);
                overlayTransform.Find("Markers").gameObject.SetActive(false);
                overlayTransform.Find("PadSprite").gameObject.GetComponent<Image>().sprite = Other.FindObjectOfTypeWithName<Sprite>("PadZoomSheet_6");
            }
        }

        [HarmonyPatch("Update"), HarmonyPostfix]
        public static void UpdatePostfix(Map __instance)
        {
            OldMap oldMap = BasePlugin.Instance.ConfigOldMap.Value;
            if (oldMap == OldMap.OnlyColor || oldMap == OldMap.ColorAndMapScreen)
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
            if (oldMap == OldMap.OnlyMapScreen || oldMap == OldMap.ColorAndMapScreen)
            {
                __instance.transform.Find("Grid").gameObject.SetActive(false);
            }
        }

        [HarmonyPatch("Find", new Type[] { typeof(int), typeof(int), typeof(int), typeof(RoomController) }), HarmonyPostfix]
        public static void FindPostfix(Map __instance, int posX, int posZ, RoomController room)
        {
            OldMap oldMap = BasePlugin.Instance.ConfigOldMap.Value;
            if (oldMap == OldMap.OnlyColor || oldMap == OldMap.ColorAndMapScreen)
            {
                MapTile tile = __instance.tiles[posX, posZ];
                if (tile != null)
                {
                    var renderer = tile.SpriteRenderer;
                    if (renderer != null)
                    {
                        bool success = false;
                        foreach (var i in roomColors)
                        {
                            if (room == null)
                            {
                                break;
                            }
                            else if (room.category == i.Key)
                            {
                                room.color = i.Value;
                                renderer.color = i.Value;
                                success = true;
                                break;
                            }
                        }
                        if (!success)
                        {
                            Color color = renderer.color;
                            color.a = 1f;
                            renderer.color = color;
                        }
                        renderer.sharedMaterial = OpaqueMat;
                        renderer.enabled = true;
                    }
                }
            }
        }

        [HarmonyPatch("AddArrow", new Type[] { typeof(Entity), typeof(Color) }), HarmonyPrefix]
        public static bool AddArrowPrefix(Entity target)
        {
            OldMap oldMap = BasePlugin.Instance.ConfigOldMap.Value;
            if ((oldMap == OldMap.OnlyColor || oldMap == OldMap.ColorAndMapScreen) && target.gameObject.TryGetComponent<PlayerManager>(out _))
            {
                return false;
            }
            return true;
        }
    }
}