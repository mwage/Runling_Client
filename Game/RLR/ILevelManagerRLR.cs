using Game.Scripts.Drones;
using Game.Scripts.RLR.MapGenerator;

namespace Game.Scripts.RLR
{
    public interface ILevelManagerRLR
    {
        DroneFactory DroneFactory { get; }
        MapGeneratorRLR MapGenerator { get; }
        RunlingChaser RunlingChaser { get; }

        void LoadDrones(int level);
        void EndLevel(float delay);
    }
}