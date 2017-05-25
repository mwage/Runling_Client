using Launcher;
using UnityEngine;

namespace Players.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        private void Start ()
        {
            SetCameraPitch(GameControl.Settings.CameraAngle.Val);
        }

        public void SetCameraPitch(float pitchAngle)
        {
            transform.localEulerAngles = new Vector3(pitchAngle, 0, 0);
        }
    }
}
