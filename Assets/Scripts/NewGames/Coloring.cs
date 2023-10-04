using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace NewGame
{
    public class Coloring : MonoBehaviour
    {
        RectTransform m_Rect;

        private void Awake()
        {
            m_Rect = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            DataManager.IsPointerOverGameObject = Switch.ON;
            m_Rect.anchoredPosition = new Vector2(0f, -400f);
            m_Rect.DOAnchorPosY(400f, 0.25f);
        }

        private void OnDisable()
        {
            DataManager.IsPointerOverGameObject = Switch.OFF;
        }

        public void WakeUp(Color[] colors)
        {
            gameObject.SetActive(true);

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Image>().color = colors[i];
            }
        }
    }
}
