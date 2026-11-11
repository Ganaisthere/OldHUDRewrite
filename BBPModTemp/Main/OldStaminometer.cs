using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace LegacyUIs.Main
{
    public class OldStaminometer : MonoBehaviour
    {
        public static OldStaminometer Instance { get; internal set; }

        public RawImage redBar, greenBar;
        public Image niddle;
        public TextMeshProUGUI youNeedRestText;

        public void Awake()
        {
            StaminometerStyle style = BasePlugin.Instance.ConfigStaminometerStyle.Value;
            if (style == StaminometerStyle.Classic || style == StaminometerStyle.PlusV01 || style == StaminometerStyle.PlusV02)
            {
                redBar = new GameObject("OSM-RedBar").AddComponent<RawImage>();
                redBar.rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
                redBar.rectTransform.pivot = new Vector2(0f, 0f);
                redBar.rectTransform.localScale = Vector3.one;
                redBar.color = Color.red;

                greenBar = new GameObject("OSM-GreenBar").AddComponent<RawImage>();
                greenBar.rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
                greenBar.rectTransform.pivot = new Vector2(0f, 0f);
                greenBar.rectTransform.localScale = Vector3.one;
                greenBar.color = new Color(0.298f, 1f, 0f, 1f);

                if (style == StaminometerStyle.Classic)
                {
                    redBar.transform.SetParent(transform, false);
                    redBar.rectTransform.sizeDelta = new Vector2(160f, 11f);
                    redBar.rectTransform.anchorMax = new Vector2(0.5f, 1f);
                    redBar.rectTransform.anchorMin = new Vector2(0.5f, 1f);
                    redBar.rectTransform.anchoredPosition3D = new Vector3(-80f, -20f, 0f);
                    redBar.rectTransform.SetSiblingIndex(1);

                    greenBar.transform.SetParent(transform, false);
                    greenBar.rectTransform.sizeDelta = new Vector2(160f, 11f);
                    greenBar.rectTransform.anchorMax = new Vector2(0.5f, 1f);
                    greenBar.rectTransform.anchorMin = new Vector2(0.5f, 1f);
                    greenBar.rectTransform.anchoredPosition3D = new Vector3(-80f, -20f, 0f);
                    greenBar.rectTransform.SetSiblingIndex(2);

                    youNeedRestText = new GameObject("OSM-RedBar").AddComponent<TextMeshProUGUI>();
                    youNeedRestText.transform.SetParent(transform, false);
                    youNeedRestText.rectTransform.sizeDelta = new Vector2(256f, 64f);
                    youNeedRestText.rectTransform.anchoredPosition = new Vector2(0f, -40f);
                    youNeedRestText.rectTransform.anchorMax = new Vector2(0.5f, 1f);
                    youNeedRestText.rectTransform.anchorMin = new Vector2(0.5f, 1f);
                    youNeedRestText.fontSize = 24;
                    youNeedRestText.alignment = TextAlignmentOptions.Center;
                    youNeedRestText.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("Hud_YouNeedRest");
                    youNeedRestText.color = Color.red;
                    youNeedRestText.gameObject.SetActive(false);
                }
                else
                {
                    redBar.transform.SetParent(gameObject.GetComponent<HudManager>().AccessToolsFieldGetValue<RectTransform>("staminaNeedle").parent, false);
                    redBar.rectTransform.SetSiblingIndex(1);

                    greenBar.transform.SetParent(redBar.transform.parent, false);
                    greenBar.rectTransform.SetSiblingIndex(2);

                    if (style == StaminometerStyle.PlusV01)
                    {
                        redBar.rectTransform.sizeDelta = new Vector2(160f, 11f);
                        redBar.rectTransform.anchoredPosition3D = new Vector3(2f, 2f, 0f);

                        greenBar.rectTransform.sizeDelta = new Vector2(160f, 11f);
                        greenBar.rectTransform.anchoredPosition3D = new Vector3(2f, 2f, 0f);
                    }
                    else
                    {
                        redBar.rectTransform.sizeDelta = new Vector2(140, 22f);
                        redBar.rectTransform.anchoredPosition3D = new Vector3(25f, 21f, 0f);

                        greenBar.rectTransform.sizeDelta = new Vector2(140, 22f);
                        greenBar.rectTransform.anchoredPosition3D = new Vector3(25f, 21f, 0f);
                    }
                }
            }
            else if (style == StaminometerStyle.VanillaNoAnimation)
            {
                niddle = new GameObject("OSM-Niddle").AddComponent<Image>();
                niddle.transform.SetParent(gameObject.GetComponent<HudManager>().AccessToolsFieldGetValue<RectTransform>("staminaNeedle").parent, false);
                niddle.rectTransform.SetSiblingIndex(2);
                niddle.transform.localScale = Vector3.one;
                niddle.rectTransform.anchoredPosition3D = new Vector3(25f, 28f, 0f);
                niddle.rectTransform.sizeDelta = new Vector2(8f, 32f);
                niddle.sprite = Other.FindObjectOfTypeWithName<Sprite>("StaminometerSheet_Needle");
            }
        }

        public void Update()
        {
            PlayerMovement movement = Singleton<CoreGameManager>.Instance.GetPlayer(0).plm;
            float edit = movement.stamina / movement.StaminaMax;
            if (greenBar != null)
            {
                greenBar.transform.localScale = new Vector3(edit > 1f ? 1f : edit, 1f, 1f);
            }
            if (youNeedRestText != null)
            {
                youNeedRestText.gameObject.SetActive(true);
                if (movement.stamina > 0)
                {
                    youNeedRestText.gameObject.SetActive(false);
                }
            }
            if (niddle != null)
            {
                float edit2 = math.round(edit > 1f ? 133f + 10f * (edit - 1f) : edit * 133f);
                niddle.rectTransform.anchoredPosition3D = new Vector3(25f + edit2, 28f, 0f);
            }
        }
    }
}
