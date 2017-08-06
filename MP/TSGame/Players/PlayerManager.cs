using Characters.Types;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.Players
{
    public class PlayerManager : TrueSyncBehaviour
    {
        public GameObject Model;
        public GameObject Trigger;
        public GameObject Shield;
        public GameObject GodMode;

        public int CurrentScore;
        public int TotalScore;
        public bool IsDead;
        public bool CheckIfDead;
        public int Lives;
        public bool IsImmobile;
        public bool IsSafe;
        public bool IsInvulnerable;
        public bool GodModeActive;
        public bool AutoClickerActive;
        public GameObject Chaser;
        public ACharacter CharacterController;
        public PlayerMovement PlayerMovement;

        private void Awake()
        {
            PlayerMovement = GetComponent<PlayerMovement>();
        }

        public void SetOwner(TSPlayerInfo player)
        {
//            owner = player;
            PlayerMovement.owner = player;
        }

        public void DestroyChaser()
        {
            if (Chaser != null)
            {
                TrueSyncManager.SyncedDestroy(Chaser);
            }
        }
    }
}