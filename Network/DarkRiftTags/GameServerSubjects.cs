namespace Network.DarkRiftTags
{
    public class GameServerSubjects
    {
        public const ushort RegisterServer = 0;
        public const ushort ServerAvailable = 1;
        public const ushort InitializeGame = 2;
        public const ushort ServerReady = 3;
        public const ushort IdentifyPlayer = 4;
        public const ushort IdentifyPlayerFailed = 5;
        public const ushort PlayerJoined = 6;

        public const ushort TestModeSLA = 100;
    }
}
