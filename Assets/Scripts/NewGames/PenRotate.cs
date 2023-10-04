using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace NewGame
{
    public class PenRotate : MonoBehaviour
    {
        public static PenRotate Instance;
        public Transform ObjEff;
        public List<Sprite> s1;
        public List<Sprite> s2;

        Vector3 pivotAngle;
        Vector2 direction;

        System.Action<object> OnClickPenItemRef = null;

        private void Awake()
        {
            Instance = this;

            OnClickPenItemRef = (e) =>
            {   
                
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = s2[DataManager.Pen];
                transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = s1[DataManager.Pen];

                Debug.Log("ss");
                if (DataManager.Pen != 0)
                {
                    ObjEff.gameObject.SetActive(true);
                }
                else
                {
                    ObjEff.gameObject.SetActive(false);

                }
            };
        }

        private void OnEnable()
        {
            this.RegisterListener(EventID.OnClickPenItem, OnClickPenItemRef);
        }

        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener(EventID.OnClickPenItem, OnClickPenItemRef);
        }

        private void Start()
        {
            pivotAngle.y = 10f;
            pivotAngle.x = 0f;
            transform.localPosition = new Vector3(0f, -0.25f, 0f);

            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = s2[DataManager.Pen];
            transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = s1[DataManager.Pen];

            if (DataManager.Pen != 0)
            {
                ObjEff.gameObject.SetActive(true);
            }
            else
            {
                ObjEff.gameObject.SetActive(false);

            }
        }

        private void Update()
        {
            direction = transform.position - pivotAngle;
            transform.localRotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f, Vector3.forward);

            if (FollowPath.Instance != null)
            {
                transform.parent.position = FollowPath.Instance.transform.position;
            }

            InputPen();
        }

        public void MouseUp()
        {
            transform.DOLocalMove(new Vector3(0f, -0.2f, 0f), 0.1f);
        }

        void InputPen()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                DataManager.Pen = 1;
                this.PostEvent(EventID.OnClickPenItem);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                DataManager.Pen = 2;
                this.PostEvent(EventID.OnClickPenItem);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                DataManager.Pen = 3;
                this.PostEvent(EventID.OnClickPenItem);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                DataManager.Pen = 4;
                this.PostEvent(EventID.OnClickPenItem);
            }


            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                DataManager.Pen = 5;
                this.PostEvent(EventID.OnClickPenItem);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                DataManager.Pen = 6;
                this.PostEvent(EventID.OnClickPenItem);
            }

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                DataManager.Pen = 7;
                this.PostEvent(EventID.OnClickPenItem);
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                DataManager.Pen = 8;
                this.PostEvent(EventID.OnClickPenItem);
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                DataManager.Pen = 9;
                this.PostEvent(EventID.OnClickPenItem);
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                DataManager.Pen = 0;
                this.PostEvent(EventID.OnClickPenItem);
            }
        }
    }
}
