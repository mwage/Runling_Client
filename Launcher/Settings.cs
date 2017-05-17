using UnityEngine;

namespace Launcher
{
    public class Settings
    {
        // Camera
        public float CameraRange = 0;
        public Limits CameraZoom = new Limits(10, 50, def: 40);
        public Limits CameraAngle = new Limits(10, 90, def: 90);
        public Limits CameraSpeed = new Limits(5, 50, def: 20);
        public int FollowEnabled;
        public bool FollowState = true;

        public Settings()
        {
            LoadSettings();
        }

        public void LoadSettings()
        {
            FollowEnabled = PlayerPrefs.GetFloat("CameraZoom") < 0.01 ? 1 : PlayerPrefs.GetInt("FollowEnabled");
            CameraZoom.Val = PlayerPrefs.GetFloat("CameraZoom") > 0.01 ? PlayerPrefs.GetFloat("CameraZoom") : CameraZoom.Def;
            CameraAngle.Val = PlayerPrefs.GetFloat("CameraAngle") > 0.01 ? PlayerPrefs.GetFloat("CameraAngle") : CameraAngle.Def;
            CameraSpeed.Val = PlayerPrefs.GetFloat("CameraSpeed") > 0.01 ? PlayerPrefs.GetFloat("CameraSpeed") : CameraSpeed.Def;
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
            Val = val;
            Def = def;
        }

        public void Decrease(float v)
        {
            Val = Mathf.Max(Val - v, Min);
        }
        public void Increase(float v)
        {
            Val = Mathf.Min(Val + v, Max);
        }
    }
}
