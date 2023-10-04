using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NewGame
{
    public class SelectPen : MonoBehaviour
    {
        public Button btnHome;
        
        private void OnEnable()
        {
            DataManager.IsPointerOverGameObject = Switch.ON;
        }

        private void Awake()
        {
           

            btnHome.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);

                if (Main.Instance.objColoring.gameObject.activeInHierarchy == false)
                {
                    DataManager.IsPointerOverGameObject = Switch.OFF;
                }

                gameObject.SetActive(false);
            });
        }
    }
}
