using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Drones;

namespace Assets.Scripts.RLR.GenerateMap
{
    public class GenerateMapRLR : MonoBehaviour
    {
        public GameObject PlatformPrefab;
        public GameObject CenterPrefab;
        public GameObject Terrain;
        public GameObject LaneStandardPrefab;
        public GameObject FirstLineOfCenterPrefab;
        public GameObject SecondLineOfCenterPrefab;
        public GameObject StartPlatform;
        public GameObject FlyingDroneCollider;
        public float FlyingDroneColliderOffset;

        protected List<ALane> Lanes;
        protected List<GameObject> SafeZones ;
        protected List<GameObject> AirCollider ;

        public void GenerateMap(int centerSize, float[] lanesWidth, float gapBetweenLines, float wallSize)
        {
            var centerSizeHalf = centerSize / 2;
            FlyingDroneColliderOffset = 20;

            SafeZones = new List<GameObject>();
            AirCollider = new List<GameObject>();
            // Destroy previous level
            foreach (Transform child in Terrain.transform)
            {
                Destroy(child.gameObject);
            }

            Lanes = CalculateLanesParameters(lanesWidth.Length, lanesWidth, gapBetweenLines, centerSizeHalf);

            InstantiatePrefabsOfDiffrentLanes(ref Lanes, CenterPrefab, LaneStandardPrefab, FirstLineOfCenterPrefab, SecondLineOfCenterPrefab, PlatformPrefab, StartPlatform);
            AssignParamtersOfWalls(wallSize, wallSize);

            for (var i = 0; i < Lanes.Count; i++)
            {
                var nextLaneWidth = i == Lanes.Count - 1 ? Lanes[Lanes.Count - 1].LaneWidth : Lanes[i + 1].LaneWidth;

                Lanes[i].SetGroundAndWallsParameters();
                Lanes[i].SetSafeZoneParameters(nextLaneWidth);
            }
            Lanes[0].InstantiateRamp(LaneStandardPrefab, Terrain.transform); // adding ramp near center

            AirCollider = CreateFlyingDroneColliders(Lanes, FlyingDroneCollider, FlyingDroneColliderOffset, Terrain.transform);
        }

        public Vector3 GetStartPlatform()
        {
            return SafeZones[SafeZones.Count - 1].transform.position;
        }

        public List<GameObject> GetSafeZones()
        {
            return SafeZones;
        }

        public float GetAirColliderRange()
        {
            return AirCollider[0].transform.localScale.x;
        }

        public Area[] GetDroneSpawnArea()
        {
            var lanes = new Area[Lanes.Count+1];
            lanes[0].LeftBoundary = -(Lanes[Lanes.Count-1].Pos.z + FlyingDroneColliderOffset / 2);
            lanes[0].RightBoundary = Lanes[Lanes.Count-1].Pos.z + FlyingDroneColliderOffset / 2;
            lanes[0].TopBoundary = Lanes[Lanes.Count-1].Pos.z + FlyingDroneColliderOffset / 2;
            lanes[0].BottomBoundary = -(Lanes[Lanes.Count-1].Pos.z + FlyingDroneColliderOffset / 2);
            
            for (var i = 0; i < Lanes.Count; i++)
            {
                var lane = Lanes[i];
                int[] sign = {1, 1, -1, -1};
                var scale = Quaternion.Euler(lane.Rot) * lane.Sc;
                lanes[Lanes.Count - i].LeftBoundary = lane.Pos.x + scale.x/2 * sign[i % 4];
                lanes[Lanes.Count - i].TopBoundary = lane.Pos.z + scale.z/2 * sign[(i+1) % 4];
                lanes[Lanes.Count - i].RightBoundary = lane.Pos.x + scale.x/2 * sign[(i+2) % 4];
                lanes[Lanes.Count - i].BottomBoundary = lane.Pos.z + scale.z/2 * sign[(i+3) % 4];
            }
            return lanes;
        }

        private void InstantiatePrefabsOfDiffrentLanes(ref List<ALane> lanes, GameObject centerPrefab, GameObject laneStandardPrefab, GameObject firstLineOfCenterPrefab, GameObject secondLineOfCenterPrefab, GameObject platformPrefab, GameObject startPlatform)
        {
            SafeZones.Add(lanes[0].InstatiateCompleteLanePrefab(firstLineOfCenterPrefab, centerPrefab, Terrain.transform));
            SafeZones.Add(lanes[1].InstatiateCompleteLanePrefab(secondLineOfCenterPrefab, platformPrefab, Terrain.transform));
            for (var i = 2; i < lanes.Count - 1; i++)
            {
                SafeZones.Add(lanes[i].InstatiateCompleteLanePrefab(laneStandardPrefab, platformPrefab, Terrain.transform));
            }
            SafeZones.Add(lanes[lanes.Count - 1].InstatiateCompleteLanePrefab(laneStandardPrefab, startPlatform, Terrain.transform)); // starting lane
        }

        private static void AssignParamtersOfWalls(float wallWidth, float colliderWidth)
        {
            ALane.WallSize = wallWidth;
            ALane.ColliderWallSize = colliderWidth;
        }

        private List<ALane> CalculateLanesParameters(int spiralsAmount, float[] linesWidth, float gap, float centerSize)
        {
            var lanes = new List<ALane>
            {
                new LaneFirstOffCenter(new Vector3(-(gap + centerSize + linesWidth[0] / 2), 0, -gap / 2), new Vector3(0F, -90F, 0F), new Vector3(gap + 2 * centerSize, 1F, linesWidth[0]), centerSize),
                new LaneSecondOffCenter(new Vector3(-(linesWidth[0] / 2), 0, -(centerSize + gap + linesWidth[0] / 2)), new Vector3(0F, 180F, 0F), new Vector3(2 * gap + 2 * centerSize + linesWidth[0], 1F, linesWidth[0])),
                new LaneStandard(new Vector3(centerSize + gap + linesWidth[0] / 2, 0, 0), new Vector3(0F, 90F, 0F), new Vector3(2 * (gap + centerSize), 1F, linesWidth[0])),
                new LaneStandard(new Vector3(-(gap + linesWidth[0]) / 2, 0, centerSize + gap + linesWidth[0] / 2), new Vector3(0F, 0F, 0F), new Vector3(3 * gap + 2 * centerSize + linesWidth[0], 1F, linesWidth[0]))
            };

            // create first (near center) lines, left one is first, then bottom right top
            for (var i = 1; i < spiralsAmount; i++)
            {
                if (i == 1) // left and bottom line of 1st spiral (starts of 0 (center one)) have diffrence dependicies
                {
                    lanes.Add(new LaneStandard(new Vector3(lanes[4 * (i - 1)].Pos.x - gap - (linesWidth[i] + linesWidth[i - 1]) / 2, 0F, lanes[4 * (i - 1)].Pos.z - linesWidth[i - 1] / 2),
                                                new Vector3(0F, -90F, 0F),
                                                new Vector3(lanes[4 * (i - 1)].Sc.x + 2 * gap + linesWidth[i - 1], 1F, linesWidth[i])));

                    lanes.Add(new LaneStandard(new Vector3(0F, 0F, lanes[4 * (i - 1) + 1].Pos.z - (gap + (linesWidth[i - 1] + linesWidth[i]) / 2)),
                                                new Vector3(0F, 180F, 0F),
                                                new Vector3(lanes[4 * (i - 1) + 1].Sc.x + 2 * gap + linesWidth[i - 1], 1F, linesWidth[i])));
                }
                else
                {
                    lanes.Add(new LaneStandard(new Vector3(lanes[4 * (i - 1)].Pos.x - gap - (linesWidth[i] + linesWidth[i - 1]) / 2, 0F, lanes[4 * (i - 1)].Pos.z + (linesWidth[i - 2] - linesWidth[i - 1]) / 2),
                                                new Vector3(0F, -90F, 0F),
                                                new Vector3(lanes[4 * (i - 1)].Sc.x + 2 * gap + linesWidth[i - 1] + linesWidth[i - 2], 1F, linesWidth[i])));

                    lanes.Add(new LaneStandard(new Vector3(0F, 0F, lanes[4 * (i - 1) + 1].Pos.z - (gap + (linesWidth[i - 1] + linesWidth[i]) / 2)),
                                                new Vector3(0F, 180F, 0F),
                                                new Vector3(lanes[4 * (i - 1) + 1].Sc.x + 2 * gap + 2 * linesWidth[i - 1], 1F, linesWidth[i])));
                }
                lanes.Add(new LaneStandard(new Vector3(lanes[4 * (i - 1) + 2].Pos.x + gap + (linesWidth[i] + linesWidth[i - 1]) / 2, 0F, 0F),
                                            new Vector3(0F, 90F, 0F),
                                            new Vector3(lanes[4 * (i - 1) + 2].Sc.x + 2 * gap + 2 * linesWidth[i - 1], 1F, linesWidth[i])));
                lanes.Add(new LaneStandard(new Vector3(lanes[4 * (i - 1) + 3].Pos.x + (-linesWidth[i] + linesWidth[i - 1]) / 2, 0F, lanes[4 * (i - 1) + 3].Pos.z + gap + (linesWidth[i - 1] + linesWidth[i]) / 2),
                                            new Vector3(0F, 0F, 0F),
                                            new Vector3(lanes[4 * (i - 1) + 3].Sc.x + 2 * gap + linesWidth[i - 1] + linesWidth[i], 1F, linesWidth[i])));
            }
            return lanes;
        }

        private List<GameObject> CreateFlyingDroneColliders(IList<ALane> lanes, GameObject flyingDroneCollider, float flyingDroneColliderOffset, Transform terrain)
        {
            var flyingDroneColliders = new List<GameObject>();
            var colliderLen = lanes[lanes.Count - 1].Sc + new Vector3(2 * lanes[lanes.Count - 1].LaneWidth + 2 * flyingDroneColliderOffset, 10f, 0f);
            for (var i = 0; i < 4; i++)
            {
                flyingDroneColliders.Add(Instantiate(flyingDroneCollider, terrain));
                flyingDroneColliders[i].transform.position = lanes[lanes.Count - 1 - i].Pos + lanes[lanes.Count - 1 - i].LanePrefab.transform.forward * flyingDroneColliderOffset;
                flyingDroneColliders[i].transform.localEulerAngles = lanes[lanes.Count - 1 - i].Rot;
                flyingDroneColliders[i].transform.localScale = colliderLen;
            }
            return flyingDroneColliders;
        }
    }   
}
