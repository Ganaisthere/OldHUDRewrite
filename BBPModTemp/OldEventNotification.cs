using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace OldHUDRewrite
{
    public class OldEventNotification : MonoBehaviour
    {
        public static OldEventNotification Instance { get; internal set; }

        public RawImage backGround;
        public TextMeshProUGUI eventText;

        public void Awake()
        {
            Instance = this;

            backGround = new GameObject("OEN-BackGround").AddComponent<RawImage>();
            backGround.transform.SetParent(transform, false);
            backGround.rectTransform.sizeDelta = new Vector2(64f, 128f);
            backGround.rectTransform.anchoredPosition3D = new Vector3(0f, 96f, 0f);
            backGround.rectTransform.anchorMax = new Vector2(1f, 0f);
            backGround.rectTransform.anchorMin = new Vector2(0f, 0f);
            backGround.rectTransform.pivot = new Vector2(0.5f, 1f);
            backGround.color = new Color(0f, 0f, 0f, 0.5f);

            eventText = new GameObject("OEN-EventText").AddComponent<TextMeshProUGUI>();
            eventText.transform.SetParent(backGround.transform, false);
            eventText.rectTransform.sizeDelta = new Vector2(420f, 360f);
            eventText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            eventText.rectTransform.anchoredPosition3D = new Vector3(0f, 16f, 0f);
            eventText.fontSize = 24;
            eventText.alignment = TextAlignmentOptions.Center;
            eventText.text = "";
            eventText.color = Color.white;

            backGround.gameObject.SetActive(false);
            eventText.gameObject.SetActive(false);
        }

        public void Update()
        {
            if (backGround != null)
            {
                backGround.transform.SetAsLastSibling();
            }
            if (eventText != null)
            {
                eventText.transform.SetAsLastSibling();
            }
        }

        public void AnnounceEvent(RandomEvent randomEvent)
        {
            if (randomEvent == null)
            {
                return;
            }
            StartCoroutine(Announce(randomEvent.GetType().Name));
        }

        public IEnumerator Announce(string text)
        {
            eventText.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("Event_" + text);
            
            backGround.gameObject.SetActive(true);
            eventText.gameObject.SetActive(true);

            float timer = 0f;
            while (timer < 5f)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            backGround.gameObject.SetActive(false);
            eventText.gameObject.SetActive(false);
        }
    }
}
