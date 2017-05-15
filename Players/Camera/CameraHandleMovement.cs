using Assets.Scripts.Launcher;
using Assets.Scripts.UI.OptionsMenu;
using UnityEngine;

namespace Assets.Scripts.Players.Camera
{
    public class CameraHandleMovement : MonoBehaviour 
        // the scipt respons on camera position. It ensure rotation around Y axis (yaw), but doenst make around X (pitch) direcly [thats makes CameraMovement]
        // CameraHandle Object allows to use arrows to move properly with Camera with any Y-axis rotation.
    {
        public SetCamera SetCamera;
        private GameObject _player;


        void Start()
        {
            SetCameraHandlePosition(GetWatchedPoint());  
        }

        void LateUpdate()
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");
            float moveX = inputX * Settings.CameraSpeed.Val * Time.deltaTime;
            float moveY = inputY * Settings.CameraSpeed.Val * Time.deltaTime;
            transform.position += (transform.forward * moveY + transform.right * moveX);

            if (InputManager.Instance.GetButtonDown(HotkeyAction.RotateLeft))
            {
                RotateCameraYAxis(-90F);
            }
            if (InputManager.Instance.GetButtonDown(HotkeyAction.RotateRight))
            {
                RotateCameraYAxis(90F);
            }
            if (InputManager.Instance.GetButtonDown(HotkeyAction.ZoomMore))
            {
                ZoomMore();
                PlayerPrefs.SetFloat("CameraZoom", Settings.CameraZoom.Val);
            }
            if (InputManager.Instance.GetButtonDown(HotkeyAction.ZoomLess))
            {
                ZoomLess();
                PlayerPrefs.SetFloat("CameraZoom", Settings.CameraZoom.Val);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.F))        /////// INPUT NOT WORKING PROPERLY
            {
                Settings.CameraFollow = (Settings.CameraFollow + 1) % 2;
            }
            if (Settings.CameraFollow == 1)
            {
                _player = GameObject.Find("Manticore(Clone)");
                if (_player != null) // maybe be bad for multiplayer)     need refactor of player
                {
                    SetCameraHandlePosition(_player.transform.position);
                }
            }
        }

        private void ZoomMore()
        {
            Vector3 watchedPoint = GetWatchedPoint();
            Settings.CameraZoom.Decrease(5);
            SetCameraHandlePosition(watchedPoint);
        }

        private void ZoomLess()
        {
            Vector3 watchedPoint = GetWatchedPoint();
            Settings.CameraZoom.Increase(5);
            SetCameraHandlePosition(watchedPoint);
        }

        public void SetCameraHandlePosition(Vector3 watchedPoint)
        {
            if (transform.GetComponentInChildren<UnityEngine.Camera>() != null)
            {
                transform.position = new Vector3(
                    watchedPoint.x - transform.forward.x * Settings.CameraZoom.Val * Mathf.Cos(Settings.CameraAngle.Val * Mathf.PI / 180),
                    Settings.CameraZoom.Val * Mathf.Sin(Settings.CameraAngle.Val * Mathf.PI / 180),
                    watchedPoint.z - transform.forward.z * Settings.CameraZoom.Val * Mathf.Cos(Settings.CameraAngle.Val * Mathf.PI / 180));
            }
        }

        public Vector3 GetWatchedPoint()
        {
            return new Vector3(
                transform.localPosition.x + transform.forward.x * Settings.CameraZoom.Val * Mathf.Cos(Settings.CameraAngle.Val * Mathf.PI / 180),
                0F,
                transform.localPosition.z + transform.forward.z * Settings.CameraZoom.Val * Mathf.Cos(Settings.CameraAngle.Val * Mathf.PI / 180));
        }

        private void RotateCameraYAxis(float degrees)
        {
            Vector3 watchedPoint = GetWatchedPoint();
            transform.Rotate(0F, degrees, 0F);
            SetCameraHandlePosition(watchedPoint);
        }
    }
}





