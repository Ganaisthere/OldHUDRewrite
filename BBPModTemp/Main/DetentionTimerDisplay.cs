using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LegacyUIs.Main
{
    public class DetentionTimerDisplay : MonoBehaviour
    {
        public DetentionRoomFunction function;
        public TextMeshProUGUI text;
        public bool ready = false;
        public float DetentionRoomTimer()
        {
            if (function == null)
            {
                return 0f;
            }
            return function.AccessToolsFieldGetValue<float>("time");
        }
        public string Format()
        {
            return string.Format(Singleton<LocalizationManager>.Instance.GetLocalizedText("Hud_Detention"), Mathf.CeilToInt(DetentionRoomTimer()).ToString());
        }

        public static void Create(DetentionRoomFunction function)
        {
            DetentionTimerDisplay timerDisplay = new GameObject("DetentionTimerDisplay_" + function.gameObject.name).AddComponent<DetentionTimerDisplay>();
            timerDisplay.function = function;
            timerDisplay.transform.SetParent(Singleton<CoreGameManager>.Instance.GetHud(0).transform, false);
            timerDisplay.transform.localScale = Vector3.one;
            timerDisplay.transform.SetSiblingIndex(6);
            timerDisplay.ready = true;
            timerDisplay.gameObject.SetActive(true);
            Check(timerDisplay, function);
        }

        public static void Check(DetentionTimerDisplay me, DetentionRoomFunction function)
        {
            List<DetentionTimerDisplay> displays = new List<DetentionTimerDisplay>();
            foreach (var display in FindObjectsOfType<DetentionTimerDisplay>())
            {
                if (display.function == function && display != me)
                {
                    displays.Add(display);
                }
            }
            while (displays.Count > 0)
            {
                Destroy(displays[0].gameObject);
                displays.RemoveAt(0);
            }
        }

        public void Awake()
        {
            text = gameObject.AddComponent<TextMeshProUGUI>();
            text.rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            text.rectTransform.sizeDelta = new Vector2(480f, 360f);
            text.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            text.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            text.fontSize = 24f;
            text.color = Color.red;
            text.alignment = TextAlignmentOptions.Center;
            text.text = Format();
        }

        public void Update()
        {
            if (function == null && ready)
            {
                Destroy(gameObject);
                return;
            }
            if (DetentionRoomTimer() <= 0f && !function.AccessToolsFieldGetValue<bool>("active"))
            {
                Destroy(gameObject);
                return;
            }
            if (text != null)
            {
                text.text = Format();
            }
        }
    }
}
