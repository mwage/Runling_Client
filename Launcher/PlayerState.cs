using Characters.Types;
using Players;
using SLA;
using UnityEngine;

namespace Launcher
{
    public class PlayerState
    {
        // Player
        public GameObject Player;
        public CharacterDto CharacterDto;
        public PlayerTrigger PlayerTrigger;
        public ACharacter CharacterController;

        public bool IsInvulnerable = false;
        public bool IsSafe = false;
        public bool IsImmobile = false;
        public bool AutoClickerActive = false;
        public bool GodModeActive = false;

        public SyncVarsSLA[] SyncVars;

        //Synced via network, should be removed
        public float MoveSpeed = 0;
        public bool IsDead = true;
        public int TotalScore = 0;
        public int Lives = 0;
    }
}
