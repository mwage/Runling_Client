using Launcher;
using UI.OptionsMenu;
using UnityEngine;

namespace Players.Camera
{
    public class CameraHandleMovement : MonoBehaviour 
        // the scipt respons on camera position. It ensure rotation around Y axis (yaw), but doenst make around X (pitch) direcly [thats makes CameraMovement]
        // CameraHandle Object allows to use arrows to move properly with Camera with any Y-axis rotation.
    {
        public SetCamera SetCamera;

        private void Start()
        {
            SetCameraHandlePosition(GetWatchedPoint());  
        }

        private void LateUpdate()
        {
            var inputX = Input.GetAxis("Horizontal");
            var inputY = Input.GetAxis("Vertical");
            var moveX = inputX * GameControl.Settings.CameraSpeed.Val * Time.deltaTime;
            var moveY = inputY * GameControl.Settings.CameraSpeed.Val * Time.deltaTime;
            transform.position += (transform.forward * moveY + transform.right * moveX);

            if (GameControl.InputManager.GetButtonDown(HotkeyAction.RotateLeft))
            {
                RotateCameraYAxis(-90F);
            }
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.RotateRight))
            {
                RotateCameraYAxis(90F);
            }
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.ZoomMore))
            {
                ZoomMore();
                PlayerPrefs.SetFloat("CameraZoom", GameControl.Settings.CameraZoom.Val);
            }
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.ZoomLess))
            {
                ZoomLess();
                PlayerPrefs.SetFloat("CameraZoom", GameControl.Settings.CameraZoom.Val);
            }
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.ActivateFollow))
            {
                GameControl.Settings.FollowState = (GameControl.Settings.FollowState + 1) % 2;
            }
            if (GameControl.Settings.FollowEnabled == 1)
            {
                if (GameControl.Settings.FollowState == 1)
                {
                    if (GameControl.State.Player != null)
                    {
                        SetCameraHandlePosition(GameControl.State.Player.transform.position);
                    }
                }
            }
        }

        private void ZoomMore()
        {
            var watchedPoint = GetWatchedPoint();
            GameControl.Settings.CameraZoom.Decrease(5);
            SetCameraHandlePosition(watchedPoint);
        }

        private void ZoomLess()
        {
            var watchedPoint = GetWatchedPoint();
            GameControl.Settings.CameraZoom.Increase(5);
            SetCameraHandlePosition(watchedPoint);
        }

        public void SetCameraHandlePosition(Vector3 watchedPoint)
        {
            if (transform.GetComponentInChildren<UnityEngine.Camera>() != null)
            {
                transform.position = new Vector3(
                    watchedPoint.x - transform.forward.x * GameControl.Settings.CameraZoom.Val * Mathf.Cos(GameControl.Settings.CameraAngle.Val * Mathf.PI / 180),
                    GameControl.Settings.CameraZoom.Val * Mathf.Sin(GameControl.Settings.CameraAngle.Val * Mathf.PI / 180),
                    watchedPoint.z - transform.forward.z * GameControl.Settings.CameraZoom.Val * Mathf.Cos(GameControl.Settings.CameraAngle.Val * Mathf.PI / 180));
            }
        }

        public Vector3 GetWatchedPoint()
        {
            return new Vector3(
                transform.localPosition.x + transform.forward.x * GameControl.Settings.CameraZoom.Val * Mathf.Cos(GameControl.Settings.CameraAngle.Val * Mathf.PI / 180),
                0F,
                transform.localPosition.z + transform.forward.z * GameControl.Settings.CameraZoom.Val * Mathf.Cos(GameControl.Settings.CameraAngle.Val * Mathf.PI / 180));
        }

        private void RotateCameraYAxis(float degrees)
        {
            var watchedPoint = GetWatchedPoint();
            transform.Rotate(0F, degrees, 0F);
            SetCameraHandlePosition(watchedPoint);
        }
    }
}





