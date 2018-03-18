namespace Game.Scripts.Network.DarkRiftTags
{
    public class SyncPlayerTags
    {
        private const ushort Shift = Tags.SyncPlayer * Tags.TagsPerPlugin;

        public const ushort ClickPosition = 0 + Shift;
        public const ushort InitializePlayers = 1 + Shift;
        public const ushort SpawnPlayers = 2 + Shift;
        public const ushort PlayerDied = 3 + Shift;
        public const ushort UpdatePlayerState = 4 + Shift;
    }
}
