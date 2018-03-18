namespace Game.Scripts.Network.DarkRiftTags
{
    public class SyncDroneTags
    {
        private const ushort Shift = Tags.SyncDrone * Tags.TagsPerPlugin;

        public const ushort SpawnDrone = 0 + Shift;
        public const ushort UpdateDroneState = 1 + Shift;
        public const ushort DestroyDrone = 2 + Shift;
    }
}
