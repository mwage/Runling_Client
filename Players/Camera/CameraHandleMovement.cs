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

        void Start()
        {
            SetCameraHandlePosition(GetWatchedPoint());  
        }

        void LateUpdate()
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");
            float moveX = inputX * GameControl.Instance.Settings.CameraSpeed.Val * Time.deltaTime;
            float moveY = inputY * GameControl.Instance.Settings.CameraSpeed.Val * Time.deltaTime;
            transform.position += (transform.forward * moveY + transform.right * moveX);

            if (GameControl.Instance.InputManager.GetButtonDown(HotkeyAction.RotateLeft))
            {
                RotateCameraYAxis(-90F);
            }
            if (GameControl.Instance.InputManager.GetButtonDown(HotkeyAction.RotateRight))
            {
                RotateCameraYAxis(90F);
            }
            if (GameControl.Instance.InputManager.GetButtonDown(HotkeyAction.ZoomMore))
            {
                ZoomMore();
                PlayerPrefs.SetFloat("CameraZoom", GameControl.Instance.Settings.CameraZoom.Val);
            }
            if (GameControl.Instance.InputManager.GetButtonDown(HotkeyAction.ZoomLess))
            {
                ZoomLess();
                PlayerPrefs.SetFloat("CameraZoom", GameControl.Instance.Settings.CameraZoom.Val);
            }
            if (GameControl.Instance.InputManager.GetButtonDown(HotkeyAction.ActivateFollow))
            {
                GameControl.Instance.Settings.FollowState = (GameControl.Instance.Settings.FollowState + 1) % 2;
            }
            if (GameControl.Instance.Settings.FollowEnabled == 1)
            {
                if (GameControl.Instance.Settings.FollowState == 1)
                {
                    if (GameControl.Instance.State.Player != null)
                    {
                        SetCameraHandlePosition(GameControl.Instance.State.Player.transform.position);
                    }
                }
            }
        }

        private void ZoomMore()
        {
            Vector3 watchedPoint = GetWatchedPoint();
            GameControl.Instance.Settings.CameraZoom.Decrease(5);
            SetCameraHandlePosition(watchedPoint);
        }

        private void ZoomLess()
        {
            Vector3 watchedPoint = GetWatchedPoint();
            GameControl.Instance.Settings.CameraZoom.Increase(5);
            SetCameraHandlePosition(watchedPoint);
        }

        public void SetCameraHandlePosition(Vector3 watchedPoint)
        {
            if (transform.GetComponentInChildren<UnityEngine.Camera>() != null)
            {
                transform.position = new Vector3(
                    watchedPoint.x - transform.forward.x * GameControl.Instance.Settings.CameraZoom.Val * Mathf.Cos(GameControl.Instance.Settings.CameraAngle.Val * Mathf.PI / 180),
                    GameControl.Instance.Settings.CameraZoom.Val * Mathf.Sin(GameControl.Instance.Settings.CameraAngle.Val * Mathf.PI / 180),
                    watchedPoint.z - transform.forward.z * GameControl.Instance.Settings.CameraZoom.Val * Mathf.Cos(GameControl.Instance.Settings.CameraAngle.Val * Mathf.PI / 180));
            }
        }

        public Vector3 GetWatchedPoint()
        {
            return new Vector3(
                transform.localPosition.x + transform.forward.x * GameControl.Instance.Settings.CameraZoom.Val * Mathf.Cos(GameControl.Instance.Settings.CameraAngle.Val * Mathf.PI / 180),
                0F,
                transform.localPosition.z + transform.forward.z * GameControl.Instance.Settings.CameraZoom.Val * Mathf.Cos(GameControl.Instance.Settings.CameraAngle.Val * Mathf.PI / 180));
        }

        private void RotateCameraYAxis(float degrees)
        {
            Vector3 watchedPoint = GetWatchedPoint();
            transform.Rotate(0F, degrees, 0F);
            SetCameraHandlePosition(watchedPoint);
        }
    }
}





