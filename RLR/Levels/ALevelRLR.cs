using Assets.Scripts.Drones;

namespace Assets.Scripts.RLR.Levels
{
    public abstract class ALevelRLR : ILevelRLR
    {
        protected readonly LevelManagerRLR Manager;
        protected readonly DroneFactory DroneFactory;

        protected ALevelRLR(LevelManagerRLR manager)
        {
            Manager = manager;
            DroneFactory = Manager.DroneFactory;
        }

        public abstract float GetMovementSpeed();

        public abstract void CreateDrones();
    }
}
