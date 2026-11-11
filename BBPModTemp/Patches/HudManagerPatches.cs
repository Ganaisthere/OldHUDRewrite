using HarmonyLib;
using LegacyUIs.Main;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LegacyUIs.Patches
{
    [HarmonyPatch(typeof(HudManager))]
    public class HudManagerPatches
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void AwakePostfix(HudManager __instance)
        {
            if (BasePlugin.Instance.ConfigOldItemSlots.Value)
            {
                __instance.inventory.transform.localScale = Vector3.zero;
                GameObject gameObject = new GameObject("OldItemSlots");
                gameObject.transform.SetParent(__instance.transform);
                gameObject.AddComponent<OldItemSlots>();

                TMP_Text itemTitle = __instance.AccessToolsFieldGetValue<TMP_Text>("itemTitle");
                itemTitle.font = Other.FindObjectOfTypeWithName<TMP_FontAsset>("COMIC_24_Pro");
                itemTitle.fontSize = 18f;
                itemTitle.rectTransform.anchoredPosition3D = new Vector3(-8f, -76f, 0f);
            }
            else if (BasePlugin.Instance.ConfigBiggerItemText.Value)
            {
                TMP_Text itemTitle = __instance.AccessToolsFieldGetValue<TMP_Text>("itemTitle");
                itemTitle.font = Other.FindObjectOfTypeWithName<TMP_FontAsset>("COMIC_24_Pro");
                itemTitle.fontSize = 24f;
            }
            if (BasePlugin.Instance.ConfigRemoveGauge.Value)
            {
                __instance.gaugeManager.gameObject.transform.localScale = Vector3.zero;
            }
            StaminometerStyle style = BasePlugin.Instance.ConfigStaminometerStyle.Value;
            if (style != StaminometerStyle.Vanilla)
            {
                RectTransform staminaNeedle = __instance.AccessToolsFieldGetValue<RectTransform>("staminaNeedle");
                Transform staminometer = staminaNeedle.parent;
                Transform staminoOverlay = staminometer.Find("Overlay");
                Transform staminoBackground = staminometer.Find("Background");
                if (style == StaminometerStyle.Classic || style == StaminometerStyle.PlusV01)
                {
                    staminaNeedle.localScale = Vector3.zero;
                    staminoOverlay.localScale = Vector3.zero;
                    staminoBackground.localScale = Vector3.zero;
                }
                else
                {
                    staminaNeedle.localScale = Vector3.zero;
                }
                if (!__instance.gameObject.TryGetComponent<OldStaminometer>(out _))
                {
                    __instance.gameObject.AddComponent<OldStaminometer>();
                }
            }
            if (BasePlugin.Instance.ConfigOldEventNotification.Value && !__instance.gameObject.TryGetComponent<OldEventNotification>(out _))
            {
                __instance.gameObject.AddComponent<OldEventNotification>();
            }
            if (BasePlugin.Instance.ConfigOldYTPsDisplay.Value && !__instance.gameObject.TryGetComponent<OldPointsAnimation>(out _))
            {
                __instance.gameObject.AddComponent<OldPointsAnimation>();
            }
            if (BasePlugin.Instance.ConfigRemoveNotebookIcon.Value)
            {
                __instance.AccessToolsFieldGetValue<GameObject[]>("notebookDisplay")[0].transform.localScale = Vector3.zero;
                TMP_Text text = __instance.AccessToolsFieldGetValue<TMP_Text[]>("textBox")[0];
                text.rectTransform.anchoredPosition3D = new Vector3(4f, text.rectTransform.anchoredPosition.y, 0f);
            }
        }

        [HarmonyPatch("UpdateNotebookText", new Type[] { typeof(int), typeof(string), typeof(bool) })]
        [HarmonyPostfix]
        public static void UpdateNotebookTextPostfix(int textVal, string text, HudManager __instance)
        {
            if (!BasePlugin.Instance.ConfigOldNotebooksText.Value || text.Contains(Singleton<LocalizationManager>.Instance.GetLocalizedText("Hud_Notebooks")))
            {
                return;
            }
            TMP_Text[] textBox = __instance.AccessToolsFieldGetValue<TMP_Text[]>("textBox");
            if (textVal < textBox.Length)
            {
                textBox[textVal].text = text + " " + Singleton<LocalizationManager>.Instance.GetLocalizedText("Hud_Notebooks");
            }
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void UpdatePostfix(HudManager __instance)
        {
            if (BasePlugin.Instance.ConfigNoItemSliderAnimation.Value)
            {
                Image[] itemIcon = __instance.inventory.AccessToolsFieldGetValue<Image[]>("itemIcon");
                Image[] itemSliderImage = __instance.inventory.AccessToolsFieldGetValue<Image[]>("itemSliderImage");
                foreach (var i in itemIcon)
                {
                    i.enabled = true;
                }
                foreach (var i in itemSliderImage)
                {
                    i.enabled = false;
                }
            }
        }

        [HarmonyPatch("UpdateReticle", new Type[] { typeof(bool) })]
        [HarmonyPrefix]
        public static bool UpdateReticlePrefix(HudManager __instance)
        {
            if (BasePlugin.Instance.ConfigNoHandOnlyReticle.Value)
            {
                __instance.AccessToolsFieldGetValue<Image>("reticle").sprite = __instance.AccessToolsFieldGetValue<Sprite>("retOff");
                return false;
            }
            return true;
        }
    }
}
