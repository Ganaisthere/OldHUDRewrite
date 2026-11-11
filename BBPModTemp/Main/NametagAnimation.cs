using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace LegacyUIs.Main
{
    public class NametagAnimation : MonoBehaviour
    {
        public static Sprite[] sprites = null;
        public Item item;
        public Image image;
        public bool ready = false;
        public float timer = 0f;

        public static void Create(int playerNumber, Item item)
        {
            if (sprites == null)
            {
                sprites = new Sprite[8]
                {
                    Other.FindObjectOfTypeWithName<Sprite>("tag0000"),
                    Other.FindObjectOfTypeWithName<Sprite>("tag0001"),
                    Other.FindObjectOfTypeWithName<Sprite>("tag0002"),
                    Other.FindObjectOfTypeWithName<Sprite>("tag0003"),
                    Other.FindObjectOfTypeWithName<Sprite>("tag0004"),
                    Other.FindObjectOfTypeWithName<Sprite>("tag0005"),
                    Other.FindObjectOfTypeWithName<Sprite>("tag0006"),
                    Other.FindObjectOfTypeWithName<Sprite>("tag0007")
                };
            }

            NametagAnimation animation = new GameObject("NametagAnimation").AddComponent<NametagAnimation>();
            animation.item = item;
            animation.transform.localScale = Vector3.one;
            animation.transform.SetParent(Singleton<CoreGameManager>.Instance.GetHud(playerNumber).transform, false);
            animation.transform.SetSiblingIndex(1);
            animation.ready = true;
            animation.gameObject.SetActive(true);
        }

        public void Awake()
        {
            image = gameObject.AddComponent<Image>();
            image.rectTransform.anchoredPosition3D = new Vector3(0f, 64f, 0f);
            image.rectTransform.sizeDelta = new Vector2(192f, 192f);
            image.rectTransform.anchorMax = new Vector2(0.5f, 0f);
            image.rectTransform.anchorMin = new Vector2(0.5f, 0f);
            image.sprite = sprites[0];
            image.gameObject.SetActive(false);
            timer = 0f;
        }

        public void Update()
        {
            if (item == null && ready || Singleton<BaseGameManager>.Instance == null)
            {
                Destroy(gameObject);
                return;
            }
            timer += Time.deltaTime;
            if (timer > 1f)
            {
                timer %= 1f;
            }
            if (image != null)
            {
                int i = (int)math.floor(timer * 8f);
                image.sprite = sprites[i];
            }
        }
    }
}
