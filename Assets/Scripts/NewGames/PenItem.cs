using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NewGame
{
    public class PenItem : MonoBehaviour
    {
        public Button btnSelect;

        public GameObject objOpen;
        public GameObject objSelect;
        public GameObject objLock;
        public Image filling;
        public Image fillingLock;
        public GameObject fx;
        public GameObject avatar;
        public GameObject avatarLock;
        public GameObject textLock;
        public GameObject shadowLock;

        System.Action<object> OnClickPenItemRef = null;

        private void Awake()
        {
            OnClickPenItemRef = (e) => Refesh();
            this.RegisterListener(EventID.OnClickPenItem, OnClickPenItemRef);




        }

        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener(EventID.OnClickPenItem, OnClickPenItemRef);
        }

        private void OnEnable()
        {
            Refesh();
        }

        public void Refesh()
        {
            btnSelect.onClick.RemoveAllListeners();

            objOpen.SetActive(false);
            objLock.SetActive(false);
            objSelect.SetActive(false);
            filling.gameObject.SetActive(false);
            fillingLock.gameObject.SetActive(false);
            fx.SetActive(false);
            avatar.SetActive(false);
            avatarLock.SetActive(false);
            textLock.SetActive(false);
            shadowLock.SetActive(false);

            if (DataManager.Pen == transform.GetSiblingIndex()) // select
            {
                objSelect.SetActive(true);
                filling.gameObject.SetActive(true);
                fx.SetActive(true);
                avatar.SetActive(true);
                Debug.Log("Duoc Chon " + transform.name);
                return;
            }

/*            if (transform.GetSiblingIndex() < DataManager.Level / 5)
            {
                objOpen.SetActive(true);
                filling.gameObject.SetActive(true);
                fx.SetActive(true);
                avatar.SetActive(true);
                Debug.Log("da mo " + transform.name);

                btnSelect.onClick.AddListener(() =>
                {
                    SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);
                    DataManager.Pen = transform.GetSiblingIndex();
                    this.PostEvent(EventID.OnClickPenItem);
                });

                return;
            }*/
            // open
            if (DataManager.GetOnpenPen(transform.GetSiblingIndex()) == 1) // open
            {
                objOpen.SetActive(true);
                filling.gameObject.SetActive(true);
                fx.SetActive(true);
                avatar.SetActive(true);

                btnSelect.onClick.AddListener(() =>
                {
                    SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);
                    DataManager.Pen = transform.GetSiblingIndex();
                    this.PostEvent(EventID.OnClickPenItem);
                });

                return;
            }

            if (transform.GetSiblingIndex() - 1 == DataManager.Level / 20) // filling
            {
                Debug.Log("filling " + transform.name);

                objLock.SetActive(true);
                filling.gameObject.SetActive(true);
                filling.fillAmount = DataManager.Level % 20 / 20f;
                avatarLock.SetActive(true);
                textLock.SetActive(true);
                shadowLock.SetActive(true);

                return;
            }

            //textUnlock.text = "Level\n" + (transform.GetSiblingIndex() * 10); //lock
            Debug.Log("Chua mo: " + transform.name);

            objLock.SetActive(true);
            fillingLock.gameObject.SetActive(true);
            avatarLock.SetActive(true);
            textLock.SetActive(true);
            shadowLock.SetActive(true);
        }
    }
}
