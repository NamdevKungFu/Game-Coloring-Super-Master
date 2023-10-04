using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NewGame
{
    public class IsOverPointerCanvas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool hasExit;

        public void OnPointerEnter(PointerEventData eventData)
        {
            DataManager.IsPointerOverGameObject = Switch.ON;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (hasExit)
                DataManager.IsPointerOverGameObject = Switch.OFF;
        }
    }
}
