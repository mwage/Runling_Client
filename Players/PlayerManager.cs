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

        public int CurrentScore;
        public int TotalScore;
        public bool IsDead;
        public bool CheckIfDead;
        public int Lives;
        public bool IsImmobile = true;
        public bool IsSafe;
        public bool IsInvulnerable = true;
        public bool GodModeActive;
        public bool AutoClickerActive;
        public List<GameObject> Chaser;
        public ACharacter CharacterController;

        private void Awake()
        {
            Chaser = new List<GameObject>();
        }

        public void DestroyChaser()
        {
            for (var i=0; i< Chaser.Count; i++)
            {
                if (Chaser[i] != null)
                {
                    Destroy(Chaser[i]);
                }
            }
            Chaser.Clear();
        }
    }
}