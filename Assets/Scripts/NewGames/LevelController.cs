using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewGame
{
    public class LevelController : MonoBehaviour
    {
        private void Start()
        {
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
        }
    }
}
