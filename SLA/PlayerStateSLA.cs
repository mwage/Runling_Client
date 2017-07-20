using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLA
{
    public class PlayerStateSLA
    {
        public PhotonPlayer Owner;
        public bool FinishedVoting;
        public bool FinishedLoading;

        public PlayerStateSLA(PhotonPlayer owner)
        {
            Owner = owner;
        }
    }
}
