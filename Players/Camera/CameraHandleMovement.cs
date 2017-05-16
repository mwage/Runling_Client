using System.Net.NetworkInformation;
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
            float moveX = inputX * Settings.Instance.CameraSpeed.Val * Time.deltaTime;
            float moveY = inputY * Settings.Instance.CameraSpeed.Val * Time.deltaTime;
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
                PlayerPrefs.SetFloat("CameraZoom", Settings.Instance.CameraZoom.Val);
            }
            if (InputManager.Instance.GetButtonDown(HotkeyAction.ZoomLess))
            {
                ZoomLess();
                PlayerPrefs.SetFloat("CameraZoom", Settings.Instance.CameraZoom.Val);
            }
            if (InputManager.Instance.GetButtonDown(HotkeyAction.ActivateFollow))
            {

                Settings.Instance.FollowEnabled = (Settings.Instance.FollowEnabled + 1) % 2;
            }
            if (Settings.Instance.FollowEnabled == 1)
            {
                if (Settings.Instance.FollowState == 1)
                {
                    if (GameControl.Player != null)
                    {
                        SetCameraHandlePosition(GameControl.Player.transform.position);
                    }
                }
            }
        }

        private void ZoomMore()
        {
            Vector3 watchedPoint = GetWatchedPoint();
            Settings.Instance.CameraZoom.Decrease(5);
            SetCameraHandlePosition(watchedPoint);
        }

        private void ZoomLess()
        {
            Vector3 watchedPoint = GetWatchedPoint();
            Settings.Instance.CameraZoom.Increase(5);
            SetCameraHandlePosition(watchedPoint);
        }

        public void SetCameraHandlePosition(Vector3 watchedPoint)
        {
            if (transform.GetComponentInChildren<UnityEngine.Camera>() != null)
            {
                transform.position = new Vector3(
                    watchedPoint.x - transform.forward.x * Settings.Instance.CameraZoom.Val * Mathf.Cos(Settings.Instance.CameraAngle.Val * Mathf.PI / 180),
                    Settings.Instance.CameraZoom.Val * Mathf.Sin(Settings.Instance.CameraAngle.Val * Mathf.PI / 180),
                    watchedPoint.z - transform.forward.z * Settings.Instance.CameraZoom.Val * Mathf.Cos(Settings.Instance.CameraAngle.Val * Mathf.PI / 180));
            }
        }

        public Vector3 GetWatchedPoint()
        {
            return new Vector3(
                transform.localPosition.x + transform.forward.x * Settings.Instance.CameraZoom.Val * Mathf.Cos(Settings.Instance.CameraAngle.Val * Mathf.PI / 180),
                0F,
                transform.localPosition.z + transform.forward.z * Settings.Instance.CameraZoom.Val * Mathf.Cos(Settings.Instance.CameraAngle.Val * Mathf.PI / 180));
        }

        private void RotateCameraYAxis(float degrees)
        {
            Vector3 watchedPoint = GetWatchedPoint();
            transform.Rotate(0F, degrees, 0F);
            SetCameraHandlePosition(watchedPoint);
        }
    }
}





