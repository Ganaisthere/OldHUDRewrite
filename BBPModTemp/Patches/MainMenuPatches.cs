using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace LegacyUIs.Patches
{
    [HarmonyPatch(typeof(MainMenu))]
    public class MainMenuPatches
    {
        public static List<string> whiteRooms = new List<string>()
        {
            "ReflexOffice",
            "Closet",
            "Cafeteria",
            "Cafeteria_Hard",
            "Library",
            "Playground",
            "JohnnysStore",
            "SafeRoom",
            "TeleporterRoom"
        };

        [HarmonyPatch("Start"), HarmonyPrefix]
        public static void StartPrefix(MainMenu __instance)
        {
            "Bro This Useless.".ToString();
            /*if (BasePlugin.Instance.ConfigOldMapColor.Value)
            {
                foreach (RoomAsset asset in Resources.FindObjectsOfTypeAll<RoomAsset>())
                {
                    bool success = false;
                    BasePlugin.Log($"\'{asset.name}\'");
                    foreach (string i in whiteRooms)
                    {
                        if (asset.name.Contains(i))
                        {
                            asset.color = Color.white;
                            success = true;
                            break;
                        }
                    }
                    if (success)
                    {
                        continue;
                    }
                    else if (asset.name.Contains("Faculty"))
                    {
                        asset.color = Color.yellow;
                    }
                    else if (asset.name.Contains("Class"))
                    {
                        asset.color = Color.red;
                    }
                    else if (asset.name.Contains("Office"))
                    {
                        asset.color = Color.gray;
                    }
                    else if (asset.name.Contains("Hall"))
                    {
                        asset.color = Color.green;
                    }
                }
            }*/
        }
    }
}
