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

        public int CurrentScore = 0;
        public int TotalScore = 0;
        public bool IsDead = true;
        public int Lives = 1;
        public bool IsImmobile = true;
        public bool IsSafe = false;
        public bool IsInvulnerable = true;
        public bool GodModeActive = false;
        public bool AutoClickerActive = false;
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