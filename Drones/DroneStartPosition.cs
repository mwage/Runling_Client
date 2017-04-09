using UnityEngine;

namespace Assets.Scripts.Drones
{
    public delegate Vector3 StartPositionDelegate(float size, Area boundary);

    public class DroneStartPosition
    {
        // Random position for bounding drones
        public static Vector3 GetRandomPositionGround(float size, Area boundary)
        {
            var startPos = new Vector3(Random.Range(boundary.LeftBoundary + (0.5f + size / 2), boundary.RightBoundary - (0.5f + size / 2)), 0.4f, Random.Range(boundary.BottomBoundary + (0.5f + size / 2), boundary.TopBoundary - (0.5f + size / 2)));
            return startPos;
        }

        // One of the 4 corners for bouncing drones
        public static Vector3 GetRandomCornerGround(float size, Area boundary)
        {
            var startPos = new Vector3();
            var location = Random.Range(0, 4);

            switch (location)
            {
                case 0:
                    startPos.Set(boundary.LeftBoundary + (0.5f + size / 2), 0.4f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
                case 1:
                    startPos.Set(boundary.RightBoundary - (0.5f + size / 2), 0.4f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
                case 2:
                    startPos.Set(boundary.LeftBoundary + (0.5f + size / 2), 0.4f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
                case 3:
                    startPos.Set(boundary.RightBoundary - (0.5f + size / 2), 0.4f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
            }

            return startPos;
        }

        // Random position for flying drones
        public static Vector3 GetRandomPositionAir(float size, Area boundary)
        {
            var startPos = new Vector3(Random.Range(boundary.LeftBoundary + (0.5f + size / 2), boundary.RightBoundary - (0.5f + size / 2)), 0.6f, Random.Range(boundary.BottomBoundary + (0.5f + size / 2), boundary.TopBoundary - (0.5f + size / 2)));
            return startPos;
        }

        // One of the 4 corners randomly for flying drones
        public static Vector3 GetRandomCornerAir(float size, Area boundary)
        {
            var startPos = new Vector3();
            var location = Random.Range(0, 4);

            switch (location)
            {
                case 0:
                    startPos.Set(boundary.LeftBoundary + (0.5f + size / 2), 0.6f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
                case 1:
                    startPos.Set(boundary.RightBoundary - (0.5f + size / 2), 0.6f, boundary.BottomBoundary + (0.5f + size / 2));
                    break;
                case 2:
                    startPos.Set(boundary.LeftBoundary + (0.5f + size / 2), 0.6f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
                case 3:
                    startPos.Set(boundary.RightBoundary - (0.5f + size / 2), 0.6f, boundary.TopBoundary - (0.5f + size / 2));
                    break;
            }

            return startPos;
        }

        public static Vector3 GetRandomBottomSector(float size, Area boundary)
        {
            var pos = new Vector3();
            pos.Set(Random.Range((boundary.LeftBoundary + (0.5f + size / 2)), (boundary.RightBoundary - (0.5f + size / 2))), 0.6f, boundary.BottomBoundary + (0.5f + size / 2));
            return pos;
        }

        public static Vector3 GetRandomTopSector(float size, Area boundary)
        {
            var pos = new Vector3();
            pos.Set(Random.Range((boundary.LeftBoundary + (0.5f + size / 2)), (boundary.RightBoundary - (0.5f + size / 2))), 0.6f, boundary.TopBoundary - (0.5f + size / 2));
            return pos;
        }

        public static Vector3 GetRandomLeftSector(float size, Area boundary)
        {
            var pos = new Vector3();
            pos.Set(boundary.LeftBoundary + (0.5f + size / 2), 0.6f, Random.Range((boundary.BottomBoundary + (0.5f + size / 2)), boundary.TopBoundary - (0.5f + size / 2)));
            return pos;
        }

        public static Vector3 GetRandomRightSector(float size, Area boundary)
        {
            var pos = new Vector3();
            pos.Set(boundary.RightBoundary - (0.5f + size / 2), 0.6f, Random.Range((boundary.BottomBoundary + (0.5f + size / 2)), boundary.TopBoundary - (0.5f + size / 2)));
            return pos;
        }
    }
}
