using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewGame
{
    public class TriggerFill : MonoBehaviour
    {
        [HideInInspector] public TriggerFillManager parent;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<TriggerPen>() != null)
            {
                parent.IsDone();
                gameObject.SetActive(false);
            }
        }
    }
}
