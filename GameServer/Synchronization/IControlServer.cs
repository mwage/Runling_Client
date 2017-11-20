using System.Collections.Generic;
using Players;

namespace Server.Scripts.Synchronization
{
    public interface IControlServer
    {
        Dictionary<uint, PlayerManager> PlayerManagers { get; }
    }
}
