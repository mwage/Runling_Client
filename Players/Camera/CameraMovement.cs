using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.Players.Camera
{
    public class CameraMovement : MonoBehaviour
    {

        void Start ()
        {
            SetCameraPitch(Settings.Instance.CameraAngle.Val);
        }

        public void SetCameraPitch(float pitchAngle)
        {
            transform.rotation = Quaternion.Euler(pitchAngle, 0, 0);
        }


    }
}
