using HarmonyLib;
using System;
using UnityEngine;

namespace OldHUDRewrite.Patches
{
    [HarmonyPatch(typeof(BaldiTV))]
    public class BaldiTVPatches
    {
        [HarmonyPatch("AnnounceEvent", new Type[] { typeof(SoundObject) })]
        [HarmonyPrefix]
        public static bool AnnounceEventPrefix()
        {
            if (BasePlugin.Instance.ConfigOldEventNotification.Value)
            {
                return false;
            }
            return true;
        }

        [HarmonyPatch("ShowLevelTimeWarning", new Type[] { typeof(EnvironmentController) })]
        [HarmonyPrefix]
        public static bool ShowLevelTimeWarningPrefix()
        {
            if (BasePlugin.Instance.ConfigOldEventNotification.Value)
            {
                return false;
            }
            return true;
        }

        [HarmonyPatch("Speak", new Type[] { typeof(SoundObject) })]
        [HarmonyPrefix]
        public static bool SpeakPrefix(SoundObject sound, BaldiTV __instance)
        {
            if (BasePlugin.Instance.ConfigOldEventNotification.Value)
            {
                AudioManager baldiTvAudioManager = __instance.AccessToolsFieldGetValue<AudioManager>("baldiTvAudioManager");
                baldiTvAudioManager.captionAnchor = new Vector2(0.5f, 0.225f);
                baldiTvAudioManager.captionPosition = new Vector3(0f, 0f, 0f);
                baldiTvAudioManager.PlaySingle(sound);
                return false;
            }
            return true;
        }
    }
}
