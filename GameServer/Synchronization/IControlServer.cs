using Game.Scripts.Players;
using System.Collections.Generic;

namespace Server.Scripts.Synchronization
{
    public interface IControlServer
    {
        Dictionary<uint, PlayerManager> PlayerManagers { get; }
    }
}
