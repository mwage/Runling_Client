namespace MP
{
    public class SyncVars
    {
        public PhotonPlayer Owner;
        public bool FinishedVoting = false;
        public bool FinishedLoading = false;
        public int CurrentScore = 0;
        public int TotalScore = 0;
        public bool IsDead = true;
        public int Lives = 0;
        public bool IsImmobile;
        public bool IsSafe;
        public bool IsInvulnerable;
        public bool GodModeActive;

        public SyncVars(PhotonPlayer owner)
        {
            Owner = owner;
        }
    }
}