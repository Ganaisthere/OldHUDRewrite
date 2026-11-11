using UnityEngine;
using UnityEngine.UI;

namespace LegacyUIs.Main
{
    public class OldItemSlots : MonoBehaviour
    {
        public static OldItemSlots Instance { get; internal set; }
        public RectTransform rectTransform;
        public RawImage[] slotBackgrounds;
        public Image[] slotItems;
        public Image slotOverlay;
        public bool ready = false;

        public void Awake()
        {
            Instance = this;
            slotBackgrounds = new RawImage[9];
            slotItems = new Image[9];

            for (int i = 0; i < 9; i++)
            {
                RawImage background = new GameObject($"SlotBackground({i})").AddComponent<RawImage>();
                background.transform.SetParent(transform);
                background.transform.localScale = Vector3.one;
                background.rectTransform.sizeDelta = new Vector2(64f, 64f);
                background.rectTransform.anchoredPosition3D = new Vector3(-635f + 75f * i, -40f, 0f);
                background.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                background.color = Color.white;

                Image item = new GameObject("SlotItem").AddComponent<Image>();
                item.transform.SetParent(background.transform);
                item.transform.localScale = Vector3.one;
                item.rectTransform.sizeDelta = new Vector2(48f, 48f);
                item.rectTransform.anchoredPosition3D = new Vector3(0f, -5f, 0f);
                if (i == 0 || i == 8)
                {
                    item.rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
                }
                item.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                item.sprite = Other.FindObjectOfTypeWithName<Sprite>("Transparent");

                slotBackgrounds[i] = background;
                slotItems[i] = item;
            }

            slotOverlay = new GameObject("SlotOverlay").AddComponent<Image>();
            slotOverlay.transform.SetParent(transform);
            slotOverlay.transform.localScale = Vector3.one;
            slotOverlay.rectTransform.sizeDelta = new Vector2(704f, 114f);
            slotOverlay.rectTransform.anchoredPosition3D = Vector3.zero;
            slotOverlay.rectTransform.pivot = new Vector2(1f, 1f);
            slotOverlay.sprite = BasePlugin.assetMan.Get<Sprite>("OldItemSlot");

            rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.anchoredPosition3D = Vector3.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchorMin = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            transform.SetAsFirstSibling();
            ready = true;
        }

        public void Update()
        {
            if (slotItems == null || Singleton<CoreGameManager>.Instance == null)
            {
                return;
            }
            bool one = BasePlugin.Instance.ConfigBiggerItemIcon.Value;
            bool two = BasePlugin.Instance.ConfigFGPDNothingItemIcon.Value;
            ItemManager manager = Singleton<CoreGameManager>.Instance.GetPlayer(0).itm;
            Sprite Transparent = Other.FindObjectOfTypeWithName<Sprite>("Transparent");
            Sprite PlaceHolder = BasePlugin.assetMan.Get<Sprite>("PlaceHolder");
            for (int i = 0; i < 9; i++)
            {
                Sprite sprite = Transparent;
                if (manager.items[i].itemType == Items.None)
                {
                    if (two)
                    {
                        sprite = PlaceHolder;
                    }
                }
                else if (one)
                {
                    sprite = manager.items[i].itemSpriteLarge;
                }
                else
                {
                    sprite = manager.items[i].itemSpriteSmall;
                }
                if (slotItems[i] == null)
                {
                    break;
                }
                slotItems[i].sprite = sprite;
                if (manager.selectedItem == i)
                {
                    slotBackgrounds[i].color = Color.red;
                }
                else
                {
                    slotBackgrounds[i].color = Color.white;
                }
            }
            if (manager.maxItem >= 0)
            {
                float edit = 2f / 3f;
                transform.localScale = Vector3.one * edit;
                rectTransform.anchoredPosition3D = new Vector3(75f * edit * (8 - manager.maxItem), 0f, 0f);
            }
            else
            {
                transform.localScale = Vector3.zero;
            }
        }
    }
}
