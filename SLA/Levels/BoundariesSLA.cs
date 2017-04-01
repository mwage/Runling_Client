
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boundaries in which new drones can be spawned
public struct Area
{
    public float leftBoundary;
    public float rightBoundary;
    public float topBoundary;
    public float bottomBoundary;
}


public class BoundariesSLA : MonoBehaviour
{
    public Area bouncingSLA;
    public Area flyingSLA;
    public Area test;

    public void Awake()
    {
        // Boundaries for bouncing drones
        bouncingSLA = new Area();
        bouncingSLA.leftBoundary = -35f;
        bouncingSLA.rightBoundary = 35f;
        bouncingSLA.topBoundary = 5f;
        bouncingSLA.bottomBoundary = -5f;

        // Boundaries for flying drones
        flyingSLA = new Area();
        flyingSLA.leftBoundary = -40f;
        flyingSLA.rightBoundary = 40f;
        flyingSLA.topBoundary = 20f;
        flyingSLA.bottomBoundary = -20f;

        // Boundaries for bouncing drones
        test = new Area();
        test.leftBoundary = -15f;
        test.rightBoundary = 15f;
        test.topBoundary = 2f;
        test.bottomBoundary = -2f;
    }
}
