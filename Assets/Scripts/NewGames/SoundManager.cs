using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;

namespace NewGame
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        public AudioSource bgSource;
        public AudioSource fxSource;
        public AudioSource pencilSource;

        public AudioClip clipSuccess;
        public AudioClip clipPop;

        private void Awake()
        {
            Instance = this;
        }

        public void Music()
        {
            if (DataManager.Music == Switch.ON)
            {
                bgSource.Play();
                return;
            }

            bgSource.Stop();
        }

        public void PlayEffect(AudioClip clip)
        {
            if (DataManager.Sound == Switch.ON)
            {
                AudioSource sr = Instantiate(fxSource, transform);
                sr.clip = clip;
                sr.Play();

                Destroy(sr.gameObject, clip.length);
            }
        }

        public void PencilVolume(float value)
        {
            if(DataManager.Sound == Switch.ON)
            {
                pencilSource.volume = value;
                return;
            }

            pencilSource.volume = 0f;
        }

        public void Vibrate()
        {
            if (DataManager.Vibrate == Switch.ON)
            {
                HapticFeedback.MediumFeedback();
            }
        }
    }
}
