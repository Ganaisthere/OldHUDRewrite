using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OldHUDRewrite.Patches
{
    [HarmonyPatch(typeof(LocalizationManager))]
    public class LocalizationManagerPatches
    {
        public static List<string> prepareLocalizationPaths;

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void StartPostfix(LocalizationManager __instance)
        {
            foreach (var t in prepareLocalizationPaths)
            {
                if (File.Exists(t))
                {
                    LocalizationData localizationData = JsonUtility.FromJson<LocalizationData>(File.ReadAllText(t));
                    Dictionary<string, string> localizedText = __instance.AccessToolsFieldGetValue<Dictionary<string, string>>("localizedText");
                    for (int i = 0; i < localizationData.items.Length; i++)
                    {
                        if (localizedText.ContainsKey(localizationData.items[i].key))
                        {
                            localizedText.Remove(localizationData.items[i].key);
                        }
                        localizedText.Add(localizationData.items[i].key, localizationData.items[i].value);
                    }
                    __instance.AccessToolsFieldSetValue("localizedText", localizedText);
                }
                else
                {
                    Debug.LogError($"Cannot find file! - {t}");
                }
            }
        }
    }
}
