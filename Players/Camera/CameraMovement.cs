using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.Players.Cameras
{
    public class CameraMovement : MonoBehaviour
    {

        void Start ()
        {
            SetCameraPitch(GameControl.CameraAngle.Val);
        }
	
        void LateUpdate ()
        {
         
        }

        public void SetCameraPitch(float pitchAngle)
        {
            transform.rotation = Quaternion.Euler(pitchAngle, 0, 0);
        }


    }
}
