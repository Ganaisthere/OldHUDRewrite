using HarmonyLib;
using System;

namespace LegacyUIs.Patches
{
    [HarmonyPatch(typeof(Elevator))]
    public class ElevatorPatches
    {
        [HarmonyPatch("Initialize", new Type[] { typeof(EnvironmentController), typeof(ElevatorManager), typeof(IntVector2) }), HarmonyPostfix]
        public static void InitializePostfix(Elevator __instance)
        {
            MapIcon mapIcon = __instance.AccessToolsFieldGetValue<MapIcon>("mapIcon");
            OldMap oldMap = BasePlugin.Instance.ConfigOldMap.Value;
            if (mapIcon != null && (oldMap == OldMap.OnlyColor || oldMap == OldMap.ColorAndMapScreen))
            {
                mapIcon.gameObject.SetActive(false);
            }
        }
    }
}
