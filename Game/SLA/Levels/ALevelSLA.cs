using Game.Scripts.Drones;

namespace Game.Scripts.SLA.Levels
{
    public abstract class ALevelSLA : ILevelSLA
    {
        protected readonly ILevelManagerSLA Manager;
        protected readonly DroneFactory DroneFactory;

        protected ALevelSLA(ILevelManagerSLA manager)
        {
            Manager = manager;
            DroneFactory = Manager.DroneFactory;
        }

        public abstract float GetMovementSpeed();

        public abstract void CreateDrones();
    }
}
