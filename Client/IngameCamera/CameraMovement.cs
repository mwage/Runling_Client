using Client.Scripts.Launcher;
using UnityEngine;

namespace Client.Scripts.IngameCamera
{
    public class CameraMovement : MonoBehaviour
    {
        public Camera Camera;

        private GameObject _followTarget;

        private void Start()
        {
            SetCameraPitch(GameControl.Settings.CameraAngle.Val);
            SetCameraPosition(GetWatchedPoint());
        }

        public void SetCameraPitch(float pitchAngle)
        {
            transform.localEulerAngles = new Vector3(pitchAngle, 0, 0);
        }

        public void InitializeFollowTarget(GameObject followTarget)
        {
            _followTarget = followTarget;
        }

        public void GetInput()
        {
            InputMoveCamera();
            InputRotateCamera();
            InputZoomCamera();
            CheckFollowInput();
        }

        private void ZoomMore()
        {
            var watchedPoint = GetWatchedPoint();
            GameControl.Settings.CameraZoom.Decrease(5);
            SetCameraPosition(watchedPoint);
        }

        private void ZoomLess()
        {
            var watchedPoint = GetWatchedPoint();
            GameControl.Settings.CameraZoom.Increase(5);
            SetCameraPosition(watchedPoint);
        }

        public void SetCameraPosition(Vector3 watchedPoint)
        {
            if (transform.GetComponentInChildren<Camera>() != null)
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
            SetCameraPosition(watchedPoint);
        }

        private void SetWatchedPointInCameraRange(ref Vector3 newWatchedPoint)
        {
            if (newWatchedPoint.x < -GameControl.Settings.CameraRange)
                newWatchedPoint.x = -GameControl.Settings.CameraRange + 0.1F;

            if (newWatchedPoint.x > GameControl.Settings.CameraRange)
                newWatchedPoint.x = GameControl.Settings.CameraRange - 0.1F;

            if (newWatchedPoint.z < -GameControl.Settings.CameraRange)
                newWatchedPoint.z = -GameControl.Settings.CameraRange + 0.1F;

            if (newWatchedPoint.z > GameControl.Settings.CameraRange)
                newWatchedPoint.z = GameControl.Settings.CameraRange - 0.1F;
        }

        private void InputMoveCamera()
        {
            if (GameControl.Settings.FollowEnabled && GameControl.Settings.FollowState)
                return;

            var inputX = Input.GetAxis("Horizontal");
            var inputY = Input.GetAxis("Vertical");
            var moveX = inputX * GameControl.Settings.CameraSpeed.Val * Time.deltaTime;
            var moveY = inputY * GameControl.Settings.CameraSpeed.Val * Time.deltaTime;
            var newWatchedPoint = GetWatchedPoint() + (transform.forward * moveY + transform.right * moveX);
            SetWatchedPointInCameraRange(ref newWatchedPoint);
            SetCameraPosition(newWatchedPoint);
        }

        private void InputRotateCamera()
        {
            if (GameControl.HotkeyMapping.GetButtonDown(HotkeyAction.RotateLeft))
            {
                RotateCameraYAxis(-90F);
            }
            if (GameControl.HotkeyMapping.GetButtonDown(HotkeyAction.RotateRight))
            {
                RotateCameraYAxis(90F);
            }
        }

        private void InputZoomCamera()
        {
            if (GameControl.HotkeyMapping.GetButtonDown(HotkeyAction.ZoomMore))
            {
                ZoomMore();
                PlayerPrefs.SetFloat("CameraZoom", GameControl.Settings.CameraZoom.Val);
                //PlayerPrefs.Save(); // it makes nullexception, because some hotkeys arent assignet yet, but works without it probably too 
            }
            if (GameControl.HotkeyMapping.GetButtonDown(HotkeyAction.ZoomLess))
            {
                ZoomLess();
                PlayerPrefs.SetFloat("CameraZoom", GameControl.Settings.CameraZoom.Val);
                //PlayerPrefs.Save();
            }
        }

        private void CheckFollowInput()
        {
            if (GameControl.HotkeyMapping.GetButtonDown(HotkeyAction.ActivateFollow))
            {
                ActivateOrDeactivateFollow();
            }
            SetFollowPosition();
        }

        public void ActivateOrDeactivateFollow()
        {
            GameControl.Settings.FollowState = !GameControl.Settings.FollowState;
        }

        private void SetFollowPosition()
        {
            if (GameControl.Settings.FollowEnabled && GameControl.Settings.FollowState)
            {
                if (_followTarget != null)
                {
                    SetCameraPosition(_followTarget.transform.position);
                }
            }
        }
    }
}