using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace NewGame
{
    public class Completed : MonoBehaviour
    {
        public Button btnReplay;
        public Button btnNext;

        public GameObject step1;
        public GameObject step2;
        public Text textPercent;
        public Image fillImage;
        public Image imgPen;
        public Image imgPenC;

        public Sprite[] penA;
        public Sprite[] penB;
        public Sprite[] penC;

        private void OnEnable()
        {
            step1.SetActive(true);
            step2.SetActive(false);
            StartCoroutine(DelaySeconds(2f));
            Main.Instance.objMainBtn.gameObject.SetActive(false);
            DataManager.IsPointerOverGameObject = Switch.ON;
            SoundManager.Instance.PencilVolume(0f);
        }

        private void OnDisable()
        {
            DataManager.IsPointerOverGameObject = Switch.OFF;
        }

        private void Awake()
        {
            btnReplay.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);
                Main.Instance.Play();
                gameObject.SetActive(false);
            });

            btnNext.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);
                if (DataManager.Level == 15)
                {
                    DataManager.Level++;
                    Main.Instance.ClearHome();
                }
                else
                {
                    DataManager.Level++;
                    Main.Instance.Play();
                }
                gameObject.SetActive(false);
            });
        }

        IEnumerator DelaySeconds(float time)
        {
            imgPenC.gameObject.SetActive(false);

            int level = DataManager.Level;
            int currentValue = level % 20;
            int valuePen = level / 20;
            valuePen++;

            imgPen.sprite = penA[valuePen];
            fillImage.sprite = penB[valuePen];

            imgPen.SetNativeSize();
            fillImage.SetNativeSize();

            yield return new WaitForSeconds(time);
            Debug.Log("currentValue" + currentValue);
            Debug.Log("currentValue" + level);
            fillImage.DOFillAmount(currentValue / 20f, 0.25f);
            fillImage.gameObject.SetActive(true);

            textPercent.text = ((currentValue % 20f == 0 ? 20 : currentValue) * 5) + "%";
            textPercent.gameObject.SetActive(true);

            Debug.Log("Value pen: " + valuePen);

            if (textPercent.text.Equals("100%"))
            {
                valuePen--;
                imgPenC.sprite = penC[valuePen];
                imgPenC.gameObject.SetActive(true);

                fillImage.gameObject.SetActive(false);
                textPercent.gameObject.SetActive(false);
                DataManager.SetOpenPen(valuePen, 1);
                DataManager.Pen = valuePen;
                this.PostEvent(EventID.OnClickPenItem);
            }

            step1.SetActive(false);
            step2.SetActive(true);
        }
    }
}
