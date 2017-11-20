using DarkRift;

namespace Network.Synchronization.Data
{
    public class PlayerState : IDarkRiftSerializable
    {
        public uint Id { get; private set; }
        public float PosX { get; private set; }
        public float PosZ { get; private set; }
        public float Rotation { get; private set; }

        public PlayerState(uint id, float posX, float posZ, float rotation)
        {
            Id = id;
            PosX = posX;
            PosZ = posZ;
            Rotation = rotation;
        }

        public PlayerState()
        {
        }

        public void Deserialize(DeserializeEvent e)
        {
            Id = e.Reader.ReadUInt32();
            PosX = e.Reader.ReadSingle();
            PosZ = e.Reader.ReadSingle();
            Rotation = e.Reader.ReadSingle();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Id);
            e.Writer.Write(PosX);
            e.Writer.Write(PosZ);
            e.Writer.Write(Rotation);
        }
    }
}
