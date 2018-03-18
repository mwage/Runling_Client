using Game.Scripts.Characters;
using Game.Scripts.Network;
using Game.Scripts.Network.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Players
{
    public class PlayerManager : MonoBehaviour
    {
        public GameObject Model;
        public GameObject Trigger;
        public GameObject GodMode;
        public GameObject Shield;

        public bool IsDead { get; set; } = true;
        public int Lives { get; set; } = 1;
        public bool IsImmobile { get; set; } = true;
        public bool IsSafe { get; set; }
        public bool IsInvulnerable { get; set; } = true;
        public bool GodModeActive { get; set; }
        public bool AutoClickerActive { get; set; }
        public List<GameObject> Chaser { get; } = new List<GameObject>();
        public Player Player { get; set; }
        public PlayerMovement PlayerMovement { get; set; }
        public CharacterManager CharacterManager { get; set; }

        // Networking
        public PlayerStateManager PlayerStateManager { get; set; }

        // TODO: Remove after implementing score for RLR    
        public int TotalScore { get; set; }

        public void DestroyChaser()
        {
            foreach (var chaser in Chaser)
            {
                if (chaser != null)
                {
                    Destroy(chaser);
                }
            }

            Chaser.Clear();
        }
    }
}