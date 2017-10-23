using DarkRift;
using System.Collections.Generic;
using System.Linq;

namespace Network.Chat
{
    public class ChatGroup : IDarkRiftSerializable
    {
        public string Name { get; private set; }
        public List<string> Users;

        public void Serialize(SerializeEvent e)
        {
        }
        
        public void Deserialize(DeserializeEvent e)
        {
            Name = e.Reader.ReadString();
            Users = e.Reader.ReadStrings().ToList();
        }
    }
}