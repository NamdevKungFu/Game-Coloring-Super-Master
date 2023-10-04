using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace NewGame
{
    public class RateUs : MonoBehaviour
    {
        public Button close;
        public Button rate;

        public List<Image> stars;

        public GameObject obj1;
        public GameObject obj2;

        System.Action<object> OnStarClickRateRef = null;

        private void Awake()
        {
            GetComponent<CanvasGroup>().alpha = 0f;

            OnStarClickRateRef = (e) =>
            {
                int index = (int)e;

                for (int i = 0; i < stars.Count; i++)
                    stars[i].color = Color.clear;

                for (int i = 0; i <= index; i++)
                    stars[i].color = Color.white;
            };

            close.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);

                GetComponent<CanvasGroup>().DOFade(0f, 0.25f).OnComplete(() => gameObject.SetActive(false));
            });

            rate.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);

                obj1.SetActive(false);
                obj2.SetActive(true);
            });
        }

        private void OnEnable()
        {
            obj1.SetActive(true);
            obj2.SetActive(false);

            GetComponent<CanvasGroup>().DOFade(1f, 0.25f);

            this.RegisterListener(EventID.OnClickStarRate, OnStarClickRateRef);
        }

        private void OnDisable()
        {
            if (Main.Instance.objColoring.gameObject.activeInHierarchy == false)
                DataManager.IsPointerOverGameObject = Switch.OFF;

            EventDispatcher.Instance.RemoveListener(EventID.OnClickStarRate, OnStarClickRateRef);
        }
    }
}
