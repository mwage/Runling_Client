using Assets.Scripts.Drones;

namespace Assets.Scripts.RLR.Levels
{
    public abstract class ALevelRLR : ILevelRLR
    {
        protected readonly LevelManagerRLR Manager;
        protected readonly DroneFactory DroneFactory;
        protected readonly GenerateMapRLR GenerateMapRLR;

        protected ALevelRLR(LevelManagerRLR manager)
        {
            Manager = manager;
            DroneFactory = Manager.DroneFactory;
            GenerateMapRLR = Manager.GenerateMapRLR;
        }

        public abstract void CreateDrones();

        public virtual void GenerateMap()
        {
            GenerateMapRLR.GenerateMap(10,new float[] {7,5,7,5,7}, 1, 0.2f);
        }
    }
}
