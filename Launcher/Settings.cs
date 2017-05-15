using UnityEngine;

namespace Assets.Scripts.Launcher
{
    public class Settings
    {
        // Camera
        public static float CameraRange = 0;

        public static Limits CameraZoom = new Limits(10, 50, def: 40);
        public static Limits CameraAngle = new Limits(10, 90, def: 90);
        public static Limits CameraSpeed = new Limits(5, 50, def: 20);
        public static int CameraFollow;

        private static Settings _instance;

        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Settings();
                return _instance;
            }
        }

        public void LoadSettings()
        {
            CameraZoom.Val = PlayerPrefs.GetFloat("CameraZoom") > 0.01 ? PlayerPrefs.GetFloat("CameraZoom") : CameraZoom.Def;
            CameraAngle.Val = PlayerPrefs.GetFloat("CameraAngle") > 0.01 ? PlayerPrefs.GetFloat("CameraAngle") : CameraAngle.Def;
            CameraSpeed.Val = PlayerPrefs.GetFloat("CameraSpeed") > 0.01 ? PlayerPrefs.GetFloat("CameraSpeed") : CameraSpeed.Def;
            CameraFollow = PlayerPrefs.GetInt("CameraFollow") != 0 ? PlayerPrefs.GetInt("CameraFollow") : 0;
        }
    }


    public class Limits
    {
        public float Min;
        public float Max;
        public float Val;
        public float Def;

        public Limits(float min, float max, float val = 10, float def = 10)
        {
            Min = min;
            Max = max;
            //Val = val;
            Def = def;
        }

        public void Decrease(float v)
        {
            Val = Val - v > Min ? Val - v : Min;
        }
        public void Increase(float v)
        {
            Val = Val + v < Max ? Val + v : Max;
        }
    }
}
