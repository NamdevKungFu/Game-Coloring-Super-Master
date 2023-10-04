using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewGame
{
    public class TriggerFillManager : MonoBehaviour
    {
        SpriteRenderer visual;
        List<Transform> triggerFills = new List<Transform>();
        Collider2D colliderPoly;

        bool theFirst;
        int lenghtOfTrigger, countTrigger;

        void Awake()
        {
            visual = GetComponent<SpriteRenderer>();
            colliderPoly = gameObject.AddComponent<PolygonCollider2D>();
            colliderPoly.isTrigger = true;

            SpawnCollider();
        }

        void SpawnCollider()
        {
            float maxX = transform.position.x + visual.bounds.size.x / 2f;
            float tempX = transform.position.x - visual.bounds.size.x / 2f;
            float tempY = transform.position.y + visual.bounds.size.y / 2f;
            float minY = transform.position.y - visual.bounds.size.y / 2f;
            float minX = tempX;

            Transform box = Resources.Load<Transform>("NewGame/Common/DotFill");
            Transform here;

            while (tempY > minY)
            {
                while (tempX < maxX)
                {
                    tempX += 0.16f;
                    here = Instantiate(box, new Vector3(tempX, tempY, 0f), Quaternion.identity, transform);
                    here.GetComponent<TriggerFill>().parent = this;

                    if (colliderPoly.OverlapPoint(here.position) == false)
                    {
                        here.gameObject.SetActive(false);
                    }
                    else
                    {
                        triggerFills.Add(here);
                    }
                }

                tempX = minX;
                tempY -= 0.16f;
            }

            lenghtOfTrigger = triggerFills.Count;

            //Destroy(colliderPoly);
        }

        public void IsDone()
        {
            if (lenghtOfTrigger <= 0)
                return;

            countTrigger++;

            if (countTrigger / (float)lenghtOfTrigger > 0.95f && theFirst == false)
            {
                SoundManager.Instance.Vibrate();
                theFirst = true;
                Main.Instance.btnDone.gameObject.SetActive(true);
            }
        }

        public int CountTrigger
        {
            get { return countTrigger; }
            set { countTrigger = value; }
        }

        public void Reload()
        {
            CountTrigger = 0;
            theFirst = false;

            for (int i = 0; i < lenghtOfTrigger; i++)
            {
                triggerFills[i].gameObject.SetActive(true);
            }
        }

        public Collider2D getCollidder2D()
        {
            return colliderPoly;
        }
    }
}
