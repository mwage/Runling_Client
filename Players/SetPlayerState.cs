using Launcher;
using SLA;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Players
{
    public class SetPlayerState : MonoBehaviour
    {
        [SerializeField] private PlayerTrigger _playerTrigger;

        public PhotonView PhotonView;
        private GameObject _model;
        private GameObject _trigger;

        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            _model = transform.Find("Model").gameObject;
            _trigger = transform.Find("Trigger").gameObject;
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "SLA")
            {
                _model.SetActive(!GameControl.PlayerState.SyncVars[PhotonView.owner.ID - 1].IsDead);
                _trigger.SetActive(!GameControl.PlayerState.SyncVars[PhotonView.owner.ID - 1].IsDead);
            }
            else
            {
                _model.SetActive(!GameControl.PlayerState.IsDead);
                _trigger.SetActive(!GameControl.PlayerState.IsDead);
            }
        }

        [PunRPC]
        private void SetDead(int playerID)
        {
            if (SceneManager.GetActiveScene().name == "SLA")
            {
                GameControl.PlayerState.SyncVars[playerID - 1].IsDead = true;
                GameControl.PlayerState.SyncVars[playerID - 1].IsImmobile = true;
                GameControl.PlayerState.SyncVars[playerID - 1].IsInvulnerable = true;
            }
            else
            {
                GameControl.PlayerState.IsDead = true;
                GameControl.PlayerState.IsImmobile = true;
                GameControl.PlayerState.IsInvulnerable = true;
            }
        }

        [PunRPC]
        private void SetFinished()
        {
            GameControl.GameState.FinishedLevel = true;
            _playerTrigger.FinishedLevel = true;
        }

        [PunRPC]
        private void SetSafe(int playerID, bool value)
        {
            GameControl.PlayerState.SyncVars[playerID - 1].IsSafe = value;
        }
    }
}