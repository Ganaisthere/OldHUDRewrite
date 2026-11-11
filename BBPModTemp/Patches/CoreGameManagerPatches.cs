using HarmonyLib;
using LegacyUIs.Main;
using System;
using UnityEngine;

namespace LegacyUIs.Patches
{
    [HarmonyPatch(typeof(CoreGameManager))]
    public class CoreGameManagerPatches
    {
        [HarmonyPatch("Pause", new Type[] { typeof(bool) }), HarmonyPrefix]
        public static bool ActivatePrefix(bool openScreen, CoreGameManager __instance)
        {
            if (!BasePlugin.Instance.ConfigFGPDPauseScreen.Value)
            {
                return true;
            }
            if (__instance.disablePause || Singleton<GlobalCam>.Instance.TransitionActive)
            {
                return false;
            }
            if (__instance.gameObject.GetComponentInChildren<FGPDPauseScreen>(true) == null)
            {
                GameObject gameObject = new GameObject("FGPDPauseScreen");
                gameObject.transform.SetParent(__instance.transform);
                FGPDPauseScreen pauseScreen = gameObject.AddComponent<FGPDPauseScreen>();
                FGPDPauseScreen.Instance = pauseScreen;
            }
            if (!__instance.AccessToolsFieldGetValue<bool>("paused"))
            {
                __instance.AccessToolsFieldSetValue("previousScale", Time.timeScale);
                Time.timeScale = 0f;
                AudioListener.pause = true;
                __instance.AccessToolsFieldSetValue("paused", true);
                Singleton<InputManager>.Instance.ActivateActionSet("Interface");
                if (openScreen)
                {
                    if (Singleton<PlayerFileManager>.Instance.authenticMode)
                    {
                        __instance.authenticScreen.gameObject.SetActive(false);
                    }
                    FGPDPauseScreen.Instance.gameObject.SetActive(true);
                    Singleton<CursorManager>.Instance.LockCursor();
                    FGPDPauseScreen.Instance.text.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(Singleton<BaseGameManager>.Instance.InPitstop() ? "FGPD_Men_Pause_SaveAvailable" : "FGPD_Men_Pause");
                }
            }
            else
            {
                Time.timeScale = __instance.AccessToolsFieldGetValue<float>("previousScale");
                AudioListener.pause = false;
                __instance.AccessToolsFieldSetValue("paused", false);
                Singleton<InputManager>.Instance.ActivateActionSet("InGame");
                Singleton<InputManager>.Instance.StopFrame();
                if (__instance.AccessToolsFieldGetValue<bool>("mapOpen"))
                {
                    __instance.GetPlayer(0).ec.map.TurnOff();
                    __instance.CloseMap();
                }
                if (Singleton<PlayerFileManager>.Instance.authenticMode)
                {
                    __instance.authenticScreen.gameObject.SetActive(true);
                }
                FGPDPauseScreen.Instance.gameObject.SetActive(false);
                Singleton<CursorManager>.Instance.LockCursor();
            }
            Singleton<MusicManager>.Instance.PauseMidi(__instance.AccessToolsFieldGetValue<bool>("paused"));
            return false;
        }
    }
}
