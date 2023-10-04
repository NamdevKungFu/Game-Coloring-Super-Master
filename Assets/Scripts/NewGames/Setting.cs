using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace NewGame
{
    public class Setting : MonoBehaviour
    {
        [Header("Assets")]
        public Sprite musicOn, musicOff;
        public Sprite soundOn, soundOff;
        public Sprite vibraOn, vibraOff;

        [Header("References")]
        public Button btnMusic;
        public Button btnSound;
        public Button btnVibra;
        public Button btnClose;
        public Button btnRateUs;
        public Button btnPrivacyPolicy;
        public Button btnTermsofuse;

        public GameObject objRate;

        private void Awake()
        {
            GetComponent<CanvasGroup>().alpha = 0f;

            btnMusic.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);

                if (DataManager.Music == Switch.ON)
                {
                    DataManager.Music = Switch.OFF;
                    btnMusic.GetComponent<Image>().sprite = musicOff;
                }
                else
                {
                    DataManager.Music = Switch.ON;
                    btnMusic.GetComponent<Image>().sprite = musicOn;
                }

                SoundManager.Instance.Music();
            });

            btnSound.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);

                if (DataManager.Sound == Switch.ON)
                {
                    DataManager.Sound = Switch.OFF;
                    btnSound.GetComponent<Image>().sprite = soundOff;
                    return;
                }

                DataManager.Sound = Switch.ON;
                btnSound.GetComponent<Image>().sprite = soundOn;
            });

            btnVibra.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);

                if (DataManager.Vibrate == Switch.ON)
                {
                    DataManager.Vibrate = Switch.OFF;
                    btnVibra.GetComponent<Image>().sprite = vibraOff;
                    return;
                }

                DataManager.Vibrate = Switch.ON;
                btnVibra.GetComponent<Image>().sprite = vibraOn;

            });

            btnClose.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);

                if (Main.Instance.objColoring.gameObject.activeInHierarchy == false)
                    DataManager.IsPointerOverGameObject = Switch.OFF;

                GetComponent<CanvasGroup>().DOFade(0f, 0.25f).OnComplete(() => gameObject.SetActive(false));
            });

            btnRateUs.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);

                GetComponent<CanvasGroup>().DOFade(0f, 0.25f).OnComplete(() => gameObject.SetActive(false));
                objRate.SetActive(true);
            });
        }

        private void OnEnable()
        {
            GetComponent<CanvasGroup>().DOFade(1f, 0.25f);
            DataManager.IsPointerOverGameObject = Switch.ON;
        }

        private void Start()
        {
            if (DataManager.Music == Switch.OFF)
            {
                btnMusic.GetComponent<Image>().sprite = musicOff;
            }
            else
            {
                btnMusic.GetComponent<Image>().sprite = musicOn;
            }

            if (DataManager.Sound == Switch.OFF)
            {
                btnSound.GetComponent<Image>().sprite = soundOff;
            }
            else
            {
                btnSound.GetComponent<Image>().sprite = soundOn;
            }

            if (DataManager.Vibrate == Switch.OFF)
            {
                btnVibra.GetComponent<Image>().sprite = vibraOff;
            }
            else
            {
                btnVibra.GetComponent<Image>().sprite = vibraOn;
            }
        }
    }
}