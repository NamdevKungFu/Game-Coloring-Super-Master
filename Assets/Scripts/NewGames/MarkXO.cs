using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkXO : MonoBehaviour
{
    public static MarkXO Instance;

    private void Awake()
    {
        Instance = this;
    }
}
