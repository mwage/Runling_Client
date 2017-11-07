using System.Collections.Generic;
using Characters.Types;
using UnityEngine;

namespace Players
{
    public class PlayerManager : MonoBehaviour
    {
        public GameObject Model;
        public GameObject Trigger;
        public GameObject Shield;
        public GameObject GodMode;

        public int CurrentScore { get; set; }
        public int TotalScore { get; set; }
        public bool IsDead { get; set; } = true;
        public int Lives { get; set; } = 1;
        public bool IsImmobile { get; set; } = true;
        public bool IsSafe { get; set; }
        public bool IsInvulnerable { get; set; } = true;
        public bool GodModeActive { get; set; }
        public bool AutoClickerActive { get; set; }
        public List<GameObject> Chaser { get; } = new List<GameObject>();
        public ACharacter CharacterController { get; set; }

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