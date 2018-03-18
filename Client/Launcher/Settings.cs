using UnityEngine;

namespace Client.Scripts.Launcher
{
    public class Settings
    {
        // Camera
        public float CameraRange { get; set; }
        public Limits CameraZoom { get; } = new Limits(5, 60, def: 20);
        public Limits CameraAngle { get; } = new Limits(5, 90, def: 45);
        public Limits CameraSpeed { get; } = new Limits(5, 50, def: 20);
        public bool FollowEnabled { get; set; } = true;
        public bool FollowState { get; set; } = true;
        public bool HideMiniMap { get; set; }

        public Settings()
        {
            LoadSettings();
        }

        public void LoadSettings()
        {
            HideMiniMap = PlayerPrefs.GetInt("HideMiniMap") != 0;
            FollowEnabled = (PlayerPrefs.GetFloat("CameraZoom") < 0.01 ? 0 : PlayerPrefs.GetInt("FollowEnabled")) == 1;
            CameraZoom.Val = PlayerPrefs.GetFloat("CameraZoom") > 0.01 ? PlayerPrefs.GetFloat("CameraZoom") : CameraZoom.Def;
            CameraAngle.Val = PlayerPrefs.GetFloat("CameraAngle") > 0.01 ? PlayerPrefs.GetFloat("CameraAngle") : CameraAngle.Def;
            CameraSpeed.Val = PlayerPrefs.GetFloat("CameraSpeed") > 0.01 ? PlayerPrefs.GetFloat("CameraSpeed") : CameraSpeed.Def;
        }
    }
    
    public class Limits
    {
        public float Val { get; set; }
        public float Min { get; }
        public float Max { get; }
        public float Def { get; }

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
