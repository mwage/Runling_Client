using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class CameraMovement : MonoBehaviour
    {
        private float _cameraSpeed;

        void Start ()
        {
            //set camera sensitivity
            _cameraSpeed = GameControl.CameraSpeed;
            transform.rotation = Quaternion.Euler(GameControl.CameraAngle, 0, 0);
        }
	
        //move camera on WASD keypress/hold
        void LateUpdate ()
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");

            float moveX = inputX * _cameraSpeed * Time.deltaTime;
            float moveY = inputY * _cameraSpeed * Time.deltaTime;

            transform.Translate(moveX, moveY, 0f);
        }

        public void SetCameraPosition(float x, float y, float z)
        {
            var zOffset = GameControl.CameraZoom * Mathf.Cos(GameControl.CameraAngle * Mathf.PI / 180);
            transform.position =
                new Vector3(Mathf.Clamp(x, -GameControl.CameraRange, GameControl.CameraRange),
                    y + GameControl.CameraZoom * Mathf.Sin(GameControl.CameraAngle * Mathf.PI / 180),
                    Mathf.Clamp(z - zOffset, -GameControl.CameraRange - zOffset, GameControl.CameraRange - zOffset));
        }

        public Vector3 GetLookAtPosition()
        {
            return new Vector3(transform.localPosition.x, transform.localPosition.y / Mathf.Sin(GameControl.CameraAngle * Mathf.PI / 180),
                transform.localPosition.z + GameControl.CameraZoom * Mathf.Cos(GameControl.CameraAngle * Mathf.PI / 180));
        }
    }
}
