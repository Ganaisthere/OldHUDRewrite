using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LegacyUIs.Main
{
    public class BootsAnimation : MonoBehaviour
    {
        public static Sprite sprite = Other.FindObjectOfTypeWithName<Sprite>("TechnoBootsSprite");
        public Item item;
        public Image image;
        public bool ready = false;
        public bool ending = false;
        public IEnumerator enumerator;

        public static void Create(int playerNumber, Item item)
        {
            BootsAnimation animation = new GameObject("BootsAnimation").AddComponent<BootsAnimation>();
            animation.item = item;
            animation.transform.localScale = Vector3.one;
            animation.transform.SetParent(Singleton<CoreGameManager>.Instance.GetHud(playerNumber).transform, false);
            animation.transform.SetSiblingIndex(2);
            animation.ready = true;
            animation.gameObject.SetActive(true);
        }

        public void Awake()
        {
            image = gameObject.AddComponent<Image>();
            image.rectTransform.anchoredPosition3D = new Vector3(0f, 64f, 0f);
            image.rectTransform.sizeDelta = new Vector2(128f, 128f);
            image.rectTransform.anchorMax = new Vector2(0.5f, 1.5f);
            image.rectTransform.anchorMin = new Vector2(0.5f, 1.5f);
            image.sprite = sprite;
            enumerator = Intro();
            StartCoroutine(enumerator);
        }

        public void Update()
        {
            if (Singleton<BaseGameManager>.Instance == null)
            {
                Destroy(gameObject);
                return;
            }
            if (image != null && !ending && item == null && ready)
            {
                if (enumerator != null)
                {
                    StopCoroutine(enumerator);
                }
                enumerator = Outro();
                StartCoroutine(enumerator);
            }
        }

        public IEnumerator Intro()
        {
            float timer = 0f;
            while (timer < 2f)
            {
                if (image == null)
                {
                    yield break;
                }
                timer += Time.deltaTime;
                image.rectTransform.anchorMax = new Vector2(0.5f, 1.5f - 1f * timer);
                image.rectTransform.anchorMin = new Vector2(0.5f, 1.5f - 1f * timer);
                yield return null;
            }
            image.rectTransform.anchorMax = new Vector2(0.5f, -1.5f);
            image.rectTransform.anchorMin = new Vector2(0.5f, -1.5f);
            enumerator = null;
            yield break;
        }

        public IEnumerator Outro()
        {
            float timer = 0f;
            while (timer < 2f)
            {
                if (image == null)
                {
                    yield break;
                }
                timer += Time.deltaTime;
                image.rectTransform.anchorMax = new Vector2(0.5f, -1.5f + 1f * timer);
                image.rectTransform.anchorMin = new Vector2(0.5f, -1.5f + 1f * timer);
                yield return null;
            }
            Destroy(gameObject);
            yield break;
        }
    }
}
