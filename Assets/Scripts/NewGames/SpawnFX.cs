using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace NewGame
{
    public class SpawnFX : MonoBehaviour
    {
        public static SpawnFX Instance;

        public GameObject[] fxs;

        private void Awake()
        {
            Instance = this;
        }

        public void Show(Vector3 pos)
        {
            GameObject temp = Instantiate(fxs[Random.Range(0, 3)], pos, Quaternion.identity);
            Destroy(temp, 1f);
            //temp.transform.DOMoveY(pos.y + 1f, 1f).OnComplete(() =>
            //{
            //    Destroy(temp);
            //});
        }
    }
}
