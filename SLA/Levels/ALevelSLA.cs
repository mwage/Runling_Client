
using Assets.Scripts.Drones;

namespace Assets.Scripts.SLA.Levels
{
    public abstract class ALevelSLA : ILevelSLA
    {
        protected readonly LevelManagerSLA Manager;
        protected readonly DroneFactory DroneFactory;
        
        protected ALevelSLA(LevelManagerSLA manager)
        {
            Manager = manager;
            DroneFactory = Manager.DroneFactory;
        }

        public abstract float GetMovementSpeed();

        public abstract void CreateDrones();
    }
}
