using UnityEngine;

namespace SLA
{
    public class SyncVarsSLA
    {
        public PhotonPlayer Owner;
        public bool FinishedVoting;
        public bool FinishedLoading;
        public int TotalScore = 0;
        public bool IsDead = true;
        public int Lives = 0;


        public SyncVarsSLA(PhotonPlayer owner)
        {
            Owner = owner;
        }
    }
}
