using DarkRift;
using Game.Scripts.Drones.DroneTypes;

namespace Game.Scripts.Network.Data
{
    public class SpawnDroneData : IDarkRiftSerializable
    {
        public DroneState State { get; private set; }
        public float Speed { get; private set; }
        public float Size { get; private set; }
        public DroneColor Color { get; private set; }
        public DroneType DroneType { get; private set; }

        public SpawnDroneData( DroneState state, float speed, float size, DroneColor color, DroneType droneType)
        {
            State = state;
            Speed = speed;
            Size = size;
            Color = color;
            DroneType = droneType;
        }

        public SpawnDroneData()
        {
        }

        public void Deserialize(DeserializeEvent e)
        {
            State = e.Reader.ReadSerializable<DroneState>();
            Speed = e.Reader.ReadSingle();
            Size = e.Reader.ReadSingle();
            Color = (DroneColor)e.Reader.ReadByte();
            DroneType = (DroneType)e.Reader.ReadByte();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(State);
            e.Writer.Write(Speed);
            e.Writer.Write(Size);
            e.Writer.Write((byte)Color);
            e.Writer.Write((byte)DroneType);
        }
    }
}
