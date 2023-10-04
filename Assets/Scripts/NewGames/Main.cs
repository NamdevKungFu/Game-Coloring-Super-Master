using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace NewGame
{
    public class Main : MonoBehaviour
    {
        public static Main Instance;
        public Button btn_Home;
        public Button btn_Test;
        public int id_Test;
        [Header("Buttons_Main")]
        public Button btnType_One;
        public Button btnType_Two;


        [Header("Buttons_Play")]
        public Button btnSetting;
        public Button btnReload;
        public Button btnDone;
        public Button btnSkin;
        public Text textLevel;
        public Image iconLevel;

        [Header("References")]
        public Transform objMain;
        public Transform objMainBtn;
        public Transform objSetting;
        public Coloring objColoring;
        public Transform objCompleted;
        public Transform objEndGame;
        public Transform objSelectPen;
        public ParticleSystem psConfetti;

        [Header("LockUI")]
        public Transform lock_Btn_One;
        public Transform lock_Btn_Two;

        public Transform parentPen;
        GameObject currentLevel;
        [HideInInspector] public float defaultCamera;

        private void Awake()
        {
            Instance = this;
            DataManager.SetOpenPen(0, 1);
            if (DataManager.Level > 15)
            {
                btnType_Two.GetComponent<Image>().raycastTarget = true;
                lock_Btn_Two.gameObject.SetActive(false);

                btnType_One.GetComponent<Image>().raycastTarget = false;
                lock_Btn_One.gameObject.SetActive(true);
                Debug.Log("Vao 1");
            }
            else
            {

                btnType_One.GetComponent<Image>().raycastTarget = true;
                lock_Btn_One.gameObject.SetActive(false);

                btnType_Two.GetComponent<Image>().raycastTarget = false;
                lock_Btn_Two.gameObject.SetActive(true);
                Debug.Log("Vao ");
            }

            btn_Test.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);
                DataManager.Level = id_Test;
            });


            btn_Home.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);
                ClearHome();

            });

            btnType_One.onClick.AddListener(() =>
            {

                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);
                Play();
                objMain.gameObject.SetActive(false);

            });
            btnType_Two.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);
                Play();
                objMain.gameObject.SetActive(false);

            });




            btnSetting.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);
                objSetting.gameObject.SetActive(true);
            });

            btnReload.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);
                FollowPath.Instance.Reload();
            });

            btnDone.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);
                FollowPath.Instance.NextPath();

                btnDone.gameObject.SetActive(false);
            });

            btnSkin.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);
                objSelectPen.gameObject.SetActive(true);
            });

            //DataManager.Level = 1;
        }

        private void Start()
        {
            if (DataManager.Level == 0)
            {
                DataManager.Level = 1;
            }

            SoundManager.Instance.Music();
            //            Play();
        }
        public void ClearHome()
        {

            objMainBtn.gameObject.SetActive(false);
            objSetting.gameObject.SetActive(false);
            objColoring.gameObject.SetActive(false);
            objCompleted.gameObject.SetActive(false);
            objEndGame.gameObject.SetActive(false);
            btnDone.gameObject.SetActive(false);

            objMain.gameObject.SetActive(true);

            if (currentLevel != null)
            {
                Destroy(currentLevel);
            }

            if (DataManager.Level > 15)
            {
                btnType_Two.GetComponent<Image>().raycastTarget = true;
                lock_Btn_Two.gameObject.SetActive(false);

                btnType_One.GetComponent<Image>().raycastTarget = false;
                lock_Btn_One.gameObject.SetActive(true);
            }
            else
            {
                btnType_One.GetComponent<Image>().raycastTarget = true;
                lock_Btn_One.gameObject.SetActive(false);

                btnType_Two.GetComponent<Image>().raycastTarget = false;
                lock_Btn_Two.gameObject.SetActive(true);
            }
        }
        public void Play()
        {
            if (currentLevel != null)
            {
                Destroy(currentLevel);
            }

            objMainBtn.gameObject.SetActive(true);

            objSetting.gameObject.SetActive(false);
            objColoring.gameObject.SetActive(false);
            objCompleted.gameObject.SetActive(false);
            objEndGame.gameObject.SetActive(false);

            btnDone.gameObject.SetActive(false);

            GameObject temp = null;
            temp = DataManager.GetLevel(DataManager.Level);

            if (temp != null)
            {
                currentLevel = Instantiate(temp);

                textLevel.text = "LEVEL " + DataManager.Level;
                iconLevel.sprite = DataManager.GetPreview(DataManager.Level);
                iconLevel.SetNativeSize();

                FollowPath followPath = temp.transform.GetChild(0).GetComponent<FollowPath>();
                Bounds myBounds = followPath.lines.GetChild(0).GetComponent<SpriteRenderer>().bounds;

                for (int i = 0; i < followPath.lines.childCount; i++)
                {
                    myBounds.Encapsulate(followPath.lines.GetChild(i).GetComponent<SpriteRenderer>().bounds);
                }

                defaultCamera = (myBounds.size.x * 1.5f) / (Camera.main.aspect * 2f);

                Camera.main.DOOrthoSize(defaultCamera, 0.5f);
                Camera.main.transform.DOScale(defaultCamera / 5f, 0.5f);
            }
            else
            {
                objMainBtn.gameObject.SetActive(false);
                objEndGame.gameObject.SetActive(true);
                PenRotate.Instance.MouseUp();

            }
        }

        public void ResetData()
        {
            objEndGame.gameObject.SetActive(false);

            //Reset
            DataManager.Level = 1;
            // Play();
            ClearHome();

            DataManager.Pen = 0;
            this.PostEvent(EventID.OnClickPenItem);

            for (int i = 0; i < parentPen.childCount; i++)
            {
                if (i != 0)
                {
                    DataManager.SetOpenPen(i, 0);
                }
            }



        }
    }
}
