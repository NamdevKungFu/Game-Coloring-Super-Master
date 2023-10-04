using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewGame
{
    public static class DataManager
    {
        // PlayerPrefs
        public static int Level
        {
            get { return PlayerPrefs.GetInt("Level"); }
            set { PlayerPrefs.SetInt("Level", value); }
        }

        public static int GetOnpenPen(int typeId)
        {
            return PlayerPrefs.GetInt("Open Pen" + typeId);
        }
        public static void SetOpenPen(int typeId,int value)
        {
            PlayerPrefs.SetInt("Open Pen" + typeId, value);

        }

        public static Switch Sound
        {
            get { return PlayerPrefs.GetInt("Sound") == 0 ? Switch.ON : Switch.OFF; }
            set { PlayerPrefs.SetInt("Sound", (int)value); }
        }

        public static Switch Music
        {
            get { return PlayerPrefs.GetInt("Music") == 0 ? Switch.ON : Switch.OFF; }
            set { PlayerPrefs.SetInt("Music", (int)value); }
        }

        public static Switch Vibrate
        {
            get { return PlayerPrefs.GetInt("Vibrate") == 0 ? Switch.ON : Switch.OFF; }
            set { PlayerPrefs.SetInt("Vibrate", (int)value); }
        }

        public static Switch IsPointerOverGameObject
        {
            get { return PlayerPrefs.GetInt("IsPointerOverGameObject") == 0 ? Switch.ON : Switch.OFF; }
            set { PlayerPrefs.SetInt("IsPointerOverGameObject", (int)value); }
        }

       

        public static int Pen
        {
            get { return PlayerPrefs.GetInt("Pen"); }
            set { PlayerPrefs.SetInt("Pen", value); }
        }

        // Resources
        public static Sprite PenSelecting(bool isSelect)
        {
            return isSelect ? Resources.Load<Sprite>("NewGame/Common/Arts/Select") : Resources.Load<Sprite>("NewGame/Common/Arts/UnSelect");
        }

        public static Sprite GetSpriteDone()
        {
            //#if UNITY_EDITOR
            //            return Resources.Load<Sprite>("NewGame/Dones/Level " + Main.Instance.levelTarget);
            //#else
            return Resources.Load<Sprite>("NewGame/Dones/Level " + Level);
            //#endif
        }

        public static ParticleSystem GetBrushBlack()
        {
            return Resources.Load<ParticleSystem>("NewGame/Common/BrushBlack");
        }

        public static ParticleSystem GetBrushColor()
        {
            return Resources.Load<ParticleSystem>("NewGame/Common/BrushColor");
        }

        public static GameObject GetLevel(int index)
        {
            return Resources.Load<GameObject>("NewGame/Levels/Level " + index);
        }

        public static Sprite GetPreview(int index)
        {
            return Resources.Load<Sprite>("NewGame/Previews/Level " + index);
        }
    }

    public enum Switch
    {
        ON = 0,
        OFF = 1
    }
}
