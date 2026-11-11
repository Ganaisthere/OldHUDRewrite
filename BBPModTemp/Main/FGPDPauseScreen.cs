using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LegacyUIs.Main
{
    public class FGPDPauseScreen : MonoBehaviour
    {
        public static FGPDPauseScreen Instance { get; internal set; }

        public Canvas canvas;
        public CanvasScaler canvasScaler;
        public GraphicRaycaster graphicRaycaster;
        public RawImage backGround;
        public TextMeshProUGUI text;

        public void Awake()
        {
            canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            canvasScaler = gameObject.AddComponent<CanvasScaler>();
            canvasScaler.referenceResolution = new Vector2(480f, 360f);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
            graphicRaycaster.blockingMask = -1;
            graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

            backGround = new GameObject("backGround").AddComponent<RawImage>();
            backGround.transform.SetParent(transform, false);
            backGround.transform.localScale = Vector3.one;
            backGround.rectTransform.anchoredPosition3D = Vector3.zero;
            backGround.rectTransform.anchorMax = Vector3.one;
            backGround.rectTransform.anchorMin = Vector3.zero;
            backGround.color = new Color(0f, 0f, 0f, 0.5f);

            text = new GameObject("backGround").AddComponent<TextMeshProUGUI>();
            text.transform.SetParent(transform, false);
            text.transform.localScale = Vector3.one;
            text.rectTransform.anchoredPosition3D = Vector3.zero;
            text.rectTransform.sizeDelta = new Vector2(480f, 320f);
            text.fontSize = 24f;
            text.color = Color.black;
            text.alignment = TextAlignmentOptions.Center;
            text.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("FGPD_Men_Pause");

            gameObject.SetActive(false);
        }

        public void Update()
        {
            if (gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Singleton<CoreGameManager>.Instance.QuitButtonPressed();
                }
            }
        }
    }
}
