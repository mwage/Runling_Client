using Characters.Types;
using Players;
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
        public bool IsDead = true;
        public bool IsInvulnerable = false;
        public bool IsSafe = false;
        public bool IsImmobile = false;
        public int Lives = 0;
        public bool AutoClickerActive = false;
        public bool GodModeActive = false;
        public int TotalScore = 0;
    }
}
