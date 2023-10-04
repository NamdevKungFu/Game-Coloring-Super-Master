using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewGame
{
    public class FadeIn : MonoBehaviour
    {
        SpriteRenderer spr;
        Color myColor;

        void OnEnable()
        {
            spr = GetComponent<SpriteRenderer>();
            myColor = spr.color;
            StartCoroutine(FadeNow());
        }

        IEnumerator FadeNow()
        {
            float origin = spr.color.a;
            myColor.a = 0f;
            spr.color = myColor;

            while (myColor.a < origin)
            {
                yield return null;
                myColor.a += Time.deltaTime / 1f;
                spr.color = myColor;
            }

            yield return null;
            myColor.a = origin;
            spr.color = myColor;
        }
    }
}
