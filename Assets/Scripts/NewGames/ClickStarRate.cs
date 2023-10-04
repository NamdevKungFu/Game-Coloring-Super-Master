using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NewGame
{
    public class ClickStarRate : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);
            this.PostEvent(EventID.OnClickStarRate, transform.GetSiblingIndex());
        }
    }
}
