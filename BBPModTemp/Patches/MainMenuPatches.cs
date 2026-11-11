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

            if (BasePlugin.Instance.ConfigOldMapColor.Value)
            {
                foreach (RoomAsset asset in Resources.FindObjectsOfTypeAll<RoomAsset>())
                {
                    bool success = false;

                    foreach (string i in whiteRooms)
                    {
                        if (asset.name.Contains(i) && asset.name.StartsWith(i.Substring(0, 1)))
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
                }
            }
        }
    }
}
