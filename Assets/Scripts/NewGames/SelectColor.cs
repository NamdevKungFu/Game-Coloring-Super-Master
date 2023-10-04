using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NewGame
{
    public class SelectColor : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SoundManager.Instance.PlayEffect(SoundManager.Instance.clipPop);
            FollowPath.Instance.SelectColor(GetComponent<Image>().color);
            transform.parent.gameObject.SetActive(false);
        }
    }
}