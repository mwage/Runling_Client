using UnityEngine;

namespace Assets.Scripts.Launcher
{
    public class Settings
    {
        // Camera
        public static float CameraRange = 0;

        public Limits CameraZoom = new Limits(10, 50, def: 40);
        public Limits CameraAngle = new Limits(10, 90, def: 90);
        public Limits CameraSpeed = new Limits(5, 50, def: 20);
        public  int FollowEnabled;
        public  int FollowState;


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

        private Settings()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            CameraZoom.Val = PlayerPrefs.GetFloat("CameraZoom") > 0.01 ? PlayerPrefs.GetFloat("CameraZoom") : CameraZoom.Def;
            CameraAngle.Val = PlayerPrefs.GetFloat("CameraAngle") > 0.01 ? PlayerPrefs.GetFloat("CameraAngle") : CameraAngle.Def;
            CameraSpeed.Val = PlayerPrefs.GetFloat("CameraSpeed") > 0.01 ? PlayerPrefs.GetFloat("CameraSpeed") : CameraSpeed.Def;
            FollowEnabled = PlayerPrefs.GetInt("FollowEnabled");
            FollowState = PlayerPrefs.GetInt("FollowState");
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
