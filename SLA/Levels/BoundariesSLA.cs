using Assets.Scripts.Drones;

namespace Assets.Scripts.SLA.Levels
{
    public class BoundariesSLA
    {

        // Area declared in Assets.Scripts.Drones.DroneFactory
        
        public static Area BouncingSla = new Area
        {
            LeftBoundary = -35f,
            RightBoundary = 35f,
            TopBoundary = 5f,
            BottomBoundary = -5f
        };

        public static Area FlyingSla = new Area
        {
            LeftBoundary = -40f,
            RightBoundary = 40f,
            TopBoundary = 20f,
            BottomBoundary = -20f
        };

        public static Area BouncingMainMenu = new Area
        {
            LeftBoundary = -22.5f,
            RightBoundary = 22.5f,
            TopBoundary = 10f,
            BottomBoundary = -10f
        };
    }
}