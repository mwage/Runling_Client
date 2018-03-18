namespace Game.Scripts.Network.DarkRiftTags
{
    public class SyncGameTags
    {
        private const ushort Shift = Tags.SyncGame * Tags.TagsPerPlugin;

        public const ushort Countdown = 0 + Shift;
        public const ushort PrepareLevel = 1 + Shift;
        public const ushort StartLevel = 2 + Shift;
        public const ushort HidePanels = 3 + Shift;
    }
}
