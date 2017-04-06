using Assets.Scripts.Drones;

namespace Assets.Scripts.SLA.Levels
{
    public class BoundariesSLA
    {
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

        public static Area Test = new Area
        {
            LeftBoundary = -15f,
            RightBoundary = 15f,
            TopBoundary = 2f,
            BottomBoundary = -2f
        };
    }
}