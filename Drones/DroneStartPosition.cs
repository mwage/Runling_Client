using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneStartPosition : MonoBehaviour {

    // Random position for bounding drones
    public Vector3 RandomPositionGround(float size, Area boundary)
    {
        Vector3 startPos = new Vector3(Random.Range(boundary.leftBoundary + (0.5f + size / 2), boundary.rightBoundary - (0.5f + size / 2)), 0.4f, Random.Range(boundary.bottomBoundary + (0.5f + size / 2), boundary.topBoundary - (0.5f + size / 2)));
        return startPos;
    }

    // One of the 4 corners for bouncing drones
    public Vector3 RandomCornerGround(float size, Area boundary)
    {
        Vector3 startPos = new Vector3();
        int location = Random.Range(0, 4);

        if (location == 0)
        {
            startPos.Set(boundary.leftBoundary + (0.5f + size / 2), 0.4f, boundary.bottomBoundary + (0.5f + size / 2));
        }
        else if (location == 1)
        {
            startPos.Set(boundary.rightBoundary - (0.5f + size / 2), 0.4f, boundary.bottomBoundary + (0.5f + size / 2));
        }
        else if (location == 2)
        {
            startPos.Set(boundary.leftBoundary + (0.5f + size / 2), 0.4f, boundary.topBoundary - (0.5f + size / 2));
        }
        else if (location == 3)
        {
            startPos.Set(boundary.rightBoundary - (0.5f + size / 2), 0.4f, boundary.topBoundary - (0.5f + size / 2));
        }

        return startPos;
    }


    // Random position for flying drones
    public Vector3 RandomPositionAir(float size, Area boundary)
    {
        Vector3 startPos = new Vector3(Random.Range(boundary.leftBoundary + (0.5f + size / 2), boundary.rightBoundary - (0.5f + size / 2)), 0.6f, Random.Range(boundary.bottomBoundary + (0.5f + size / 2), boundary.topBoundary - (0.5f + size / 2)));
        return startPos;
    }

    // One of the 4 corners randomly for flying drones
    public Vector3 RandomCornerAir(float size, Area boundary)
    {
        Vector3 startPos = new Vector3();
        int location = Random.Range(0, 4);

        if (location == 0)
        {
            startPos.Set(boundary.leftBoundary + (0.5f + size / 2), 0.6f, boundary.bottomBoundary + (0.5f + size / 2));
        }
        else if (location == 1)
        {
            startPos.Set(boundary.rightBoundary - (0.5f + size / 2), 0.6f, boundary.bottomBoundary + (0.5f + size / 2));
        }
        else if (location == 2)
        {
            startPos.Set(boundary.leftBoundary + (0.5f + size / 2), 0.6f, boundary.topBoundary - (0.5f + size / 2));
        }
        else if (location == 3)
        {
            startPos.Set(boundary.rightBoundary - (0.5f + size / 2), 0.6f, boundary.topBoundary - (0.5f + size / 2));
        }

        return startPos;
    }


}
