using DarkRift;

namespace Network.Synchronization.Data
{
    public class DroneState : IDarkRiftSerializable
    {
        public ushort Id { get; private set; }
        public float PosX { get; private set; }
        public float PosZ { get; private set; }

        public DroneState(ushort id, float posX, float posZ)
        {
            Id = id;
            PosX = posX;
            PosZ = posZ;
        }

        public DroneState()
        {
        }

        public void Deserialize(DeserializeEvent e)
        {
            Id = e.Reader.ReadUInt16();
            PosX = e.Reader.ReadSingle();
            PosZ = e.Reader.ReadSingle();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Id);
            e.Writer.Write(PosX);
            e.Writer.Write(PosZ);
        }
    }
}
