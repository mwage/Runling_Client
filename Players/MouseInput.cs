using Launcher;
using Network.Synchronization;
using System.Collections;
using UnityEngine;

namespace Players
{
    public class MouseInput
    {
        public bool IsAutoClicking { get; private set; }

        private readonly PlayerManager _playerManager;
        private readonly InputServer _inputServer;
        private Coroutine _autoClickRoutine;

        public MouseInput(PlayerManager playerManager, InputServer inputServer)
        {
            _playerManager = playerManager;
            _inputServer = inputServer;
        }

        public void ProcessInput()
        {
            if (_playerManager.AutoClickerActive)
            {
                if (!IsAutoClicking)
                {
                    _autoClickRoutine = _inputServer.StartCoroutine(DoAutoclick());
                    IsAutoClicking = true;
                }
            }

            if (!_playerManager.AutoClickerActive)
            {
                if (IsAutoClicking)
                {
                   _inputServer.StopCoroutine(_autoClickRoutine);
                    IsAutoClicking = false;
                }
            }

            // On right mouseclick, set new target location
            if (Input.GetMouseButtonDown(1))
            {
                HandleClick();
            }
        }

        private IEnumerator DoAutoclick()
        {
            while (true)
            {
                HandleClick();
                yield return new WaitForSeconds(0.05f);
            }
        }

        private void HandleClick()
        {
            var clickPosition = CalculateClickPosition(Input.mousePosition);

            if (clickPosition == null)
                return;

            if (GameControl.GameState.Solo)
            {
                _playerManager.PlayerMovement.MoveToPosition(clickPosition.Value);
            }
            else
            {
                SyncPlayerManager.SendClickPosition(clickPosition.Value);
            }

            PlayClickAnimation(clickPosition.Value);
        }

        private static Vector3? CalculateClickPosition(Vector3 position)
        {
            RaycastHit hit;
            var ray = UnityEngine.Camera.main.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, PlayerMovement.DefLayer))
            {
                return hit.point;
            }
            return null;
        }

        private void PlayClickAnimation(Vector3 clickPos)
        {
            var click = Object.Instantiate(_inputServer.MouseClickPrefab, clickPos, Quaternion.Euler(0, 45, 0));
            if (_playerManager.IsImmobile)
            {
                foreach (Transform child in click.transform)
                {
                    child.GetComponent<Renderer>().material.color = Color.red;
                    foreach (Transform ch in child)
                    {
                        ch.GetComponent<Renderer>().material.color = Color.red;
                    }
                }
            }
        }
    }
}
