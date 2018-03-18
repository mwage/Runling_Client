namespace Game.Scripts.Network.DarkRiftTags
{
    public class GameServerTags
    {
        private const ushort Shift = Tags.GameServer * Tags.TagsPerPlugin;

        public const ushort RegisterServer = 0 + Shift;
        public const ushort ServerAvailable = 1 + Shift;
        public const ushort InitializeGame = 2 + Shift;
        public const ushort ServerReady = 3 + Shift;
        public const ushort IdentifyPlayer = 4 + Shift;
        public const ushort IdentifyPlayerFailed = 5 + Shift;
        public const ushort PlayerJoined = 6 + Shift;

        public const ushort TestModeSLA = 100 + Shift;
    }
}
