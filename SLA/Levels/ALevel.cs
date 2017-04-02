
using Assets.Scripts.Drones;

namespace Assets.Scripts.SLA.Levels
{
    public abstract class ALevel : ILevel
    {
        protected readonly LevelManagerSLA Manager;
        protected readonly DroneFactory DroneFactory;
        
        protected ALevel(LevelManagerSLA manager)
        {
            Manager = manager;
            DroneFactory = Manager.DroneFactory;
        }

        public abstract float GetMovementSpeed();

        public abstract void CreateDrones();
    }
}
