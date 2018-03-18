namespace Game.Scripts.Network.DarkRiftTags
{
    public class VotingTags
    {
        private const ushort Shift = Tags.Voting * Tags.TagsPerPlugin;

        public const ushort StartVoting = 0 + Shift;
        public const ushort SubmitVote = 1 + Shift;
        public const ushort VoteUpdate = 2 + Shift;
        public const ushort FinishVoting = 3 + Shift;
        public const ushort StartGame = 4 + Shift;
    }
}
