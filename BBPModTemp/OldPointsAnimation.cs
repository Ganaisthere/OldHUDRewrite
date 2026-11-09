using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace OldHUDRewrite
{
    public class OldPointsAnimation : MonoBehaviour
    {
        public static OldPointsAnimation Instance { get; internal set; }

        public TextMeshProUGUI totalTmp;
        public List<TextMeshProUGUI> additionTmps = new List<TextMeshProUGUI>();
        public int displayPoints;

        public void Awake()
        {
            additionTmps.Clear();
            Instance = this;

            totalTmp = new GameObject("OPA-TotalTmp").AddComponent<TextMeshProUGUI>();
            totalTmp.transform.SetParent(transform, false);
            totalTmp.transform.SetSiblingIndex(3);
            totalTmp.rectTransform.sizeDelta = new Vector2(256f, 32f);
            totalTmp.rectTransform.anchoredPosition3D = new Vector3(0f, -16f, 0f);
            totalTmp.rectTransform.anchorMax = new Vector2(0.5f, 0.675f);
            totalTmp.rectTransform.anchorMin = new Vector2(0.5f, 0.675f);
            totalTmp.fontSize = 24f;
            totalTmp.alignment = TextAlignmentOptions.Center;
            totalTmp.color = Color.black;

            displayPoints = Singleton<CoreGameManager>.Instance.GetPoints(0);
            totalTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("Hud_TotalYTPs") + "\n" + displayPoints.ToString();
            totalTmp.gameObject.SetActive(false);
        }

        public void Update()
        {
            if (totalTmp != null)
            {
                if (additionTmps.Count <= 0)
                {
                    displayPoints = Singleton<CoreGameManager>.Instance.GetPoints(0);
                }
                totalTmp.text = Singleton<LocalizationManager>.Instance.GetLocalizedText("Hud_TotalYTPs") + "\n" + displayPoints.ToString();
            }
        }

        public void ShowDisplay(bool val)
        {
            if (totalTmp == null)
            {
                return;
            }
            if (val)
            {
                totalTmp.gameObject.SetActive(true);
                return;
            }
            totalTmp.gameObject.SetActive(false);
        }

        public void AddScore(int points)
        {
            TextMeshProUGUI additionTmp = new GameObject("OPA-AdditionTmp").AddComponent<TextMeshProUGUI>();
            additionTmp.transform.SetParent(totalTmp.transform, false);
            additionTmp.rectTransform.sizeDelta = new Vector2(256f, 64f);
            additionTmp.rectTransform.anchoredPosition3D = new Vector3(0f, -128f, 0f);
            additionTmp.fontSize = 24f;
            additionTmp.alignment = TextAlignmentOptions.Center;
            additionTmp.color = Color.black;
            additionTmp.text = points.ToString();
            if (points > 0)
            {
                additionTmp.text = "+" + points.ToString();
            }
            additionTmps.Add(additionTmp);
            ShowDisplay(true);
            StartCoroutine(Adder(additionTmp, displayPoints + points));
        }

        public IEnumerator Adder(TextMeshProUGUI text, int addedValue)
        {
            float timer = 0f;
            while (text.rectTransform.anchoredPosition3D.y < -14f)
            {
                if (text == null)
                {
                    yield break;
                }
                else
                {
                    yield return null;
                }
                timer += Time.deltaTime;
                text.rectTransform.anchoredPosition3D = new Vector3(0f, -128f + timer * 128f, 0f);
            }
            text.rectTransform.anchoredPosition3D = new Vector3(0f, -16f, 0f);
            text.enabled = false;
            displayPoints = addedValue;
            Update();
            yield return new WaitForSeconds(1f);
            if (additionTmps.Contains(text))
            {
                additionTmps.Remove(text);
            }
            if (additionTmps.Count <= 0)
            {
                ShowDisplay(false);
            }
            if (text != null)
            {
                Destroy(text.gameObject);
            }
            yield break;
        }

        public void OnDestroy()
        {
            additionTmps.Clear();
        }
    }
}
