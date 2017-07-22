using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLA
{
    public class PlayerStateSLA
    {
        public PhotonPlayer Owner;
        public GameObject Player;
        public bool FinishedVoting;
        public bool FinishedLoading;
        public int TotalScore;
        public bool IsDead = true;
        public int Lives = 0;

        public PlayerStateSLA(PhotonPlayer owner)
        {
            Owner = owner;
        }
    }
}
