using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NewGame
{
    public class FillLoading : MonoBehaviour
    {
        public void OnFillDone()
        {
            SceneManager.LoadScene(1);
        }
    }
}
