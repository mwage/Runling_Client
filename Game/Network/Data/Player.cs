using DarkRift;
using Game.Scripts.GameSettings;

namespace Game.Scripts.Network.Data
{
    public class Player : IDarkRiftSerializable
    {
        public uint Id { get; set; }
        public string Name { get; private set; }
        public bool IsHost { get; set; }
        public PlayerColor Color { get; private set; }
        public GameMode? Vote { get; set; } = null;
        public bool FinishedVoting { get; set; } = false;

        public Player(uint id, string name, PlayerColor color)
        {
            Id = id;
            Name = name;
            Color = color;
        }

        public Player()
        {
        }

        public void Deserialize(DeserializeEvent e)
        {
            Id = e.Reader.ReadUInt32();
            Name = e.Reader.ReadString();
            IsHost = e.Reader.ReadBoolean();
            Color = (PlayerColor)e.Reader.ReadByte();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Id);
            e.Writer.Write(Name);
            e.Writer.Write(IsHost);
            e.Writer.Write((byte)Color);
        }
    }
}
