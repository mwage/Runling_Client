using UnityEngine;

namespace Drones
{
    public delegate Vector3 StartPositionDelegate(float size, Area boundary);

    public class DroneStartPosition
    {
        // Random position
        public static Vector3 GetRandomPosition(float size, Area boundary)
        {
            return new Vector3(Random.Range(boundary.LeftBoundary + (0.5f + size / 2), boundary.RightBoundary - (0.5f + size / 2)), 0.4f, Random.Range(boundary.BottomBoundary + (0.5f + size / 2), boundary.TopBoundary - (0.5f + size / 2)));
        }

        // One of the 4 corners
        public static Vector3 GetRandomCorner(float size, Area boundary)
        {
            var startPos = new Vector3();
            var location = Random.Range(0, 4);

            switch (location)
            {
                case 0:
                    startPos = GetTopLeftCorner(size, boundary);
                    break;
                case 1:
                    startPos = GetTopRightCorner(size, boundary);
                    break;
                case 2:
                    startPos = GetBottomLeftCorner(size, boundary);
                    break;
                case 3:
                    startPos = GetBottomRightCorner(size, boundary);
                    break;
            }

            return startPos;
        }

        public static Vector3 GetRandomBottomSector(float size, Area boundary)
        {
            return new Vector3(Random.Range((boundary.LeftBoundary + (0.5f + size / 2)), (boundary.RightBoundary - (0.5f + size / 2))), 0.4f, boundary.BottomBoundary + (0.5f + size / 2));
        }

        public static Vector3 GetRandomTopSector(float size, Area boundary)
        {
            return new Vector3(Random.Range((boundary.LeftBoundary + (0.5f + size / 2)), (boundary.RightBoundary - (0.5f + size / 2))), 0.4f, boundary.TopBoundary - (0.5f + size / 2));
        }

        public static Vector3 GetRandomLeftSector(float size, Area boundary)
        {
            return new Vector3(boundary.LeftBoundary + (0.5f + size / 2), 0.4f, Random.Range((boundary.BottomBoundary + (0.5f + size / 2)), boundary.TopBoundary - (0.5f + size / 2)));
        }

        public static Vector3 GetRandomRightSector(float size, Area boundary)
        {
            return new Vector3(boundary.RightBoundary - (0.5f + size / 2), 0.4f, Random.Range((boundary.BottomBoundary + (0.5f + size / 2)), boundary.TopBoundary - (0.5f + size / 2)));
        }

        public static Vector3 GetTopLeftCorner(float size, Area boundary)
        {
            return new Vector3(boundary.LeftBoundary + (0.5f + size / 2), 0.4f, boundary.TopBoundary - (0.5f + size / 2));
        }

        public static Vector3 GetTopRightCorner(float size, Area boundary)
        {
            return new Vector3(boundary.RightBoundary - (0.5f + size / 2), 0.4f, boundary.TopBoundary - (0.5f + size / 2));
        }

        public static Vector3 GetBottomLeftCorner(float size, Area boundary)
        {
            return new Vector3(boundary.LeftBoundary + (0.5f + size / 2), 0.4f, boundary.BottomBoundary + (0.5f + size / 2));
        }

        public static Vector3 GetBottomRightCorner(float size, Area boundary)
        {
            return new Vector3(boundary.RightBoundary - (0.5f + size / 2), 0.4f, boundary.BottomBoundary + (0.5f + size / 2));
        }
    }
}
