using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        protected List<GameObject> SafeZones = new List<GameObject>(0);

        public void GenerateMap(int centerSize, float[] lanesWidth, float gapBetweenLines, float wallSize)
        {
            int centerSizeHalf = centerSize / 2;
            FlyingDroneColliderOffset = 20;

            // Destroy previous level
            foreach (Transform child in Terrain.transform)
            {
                Destroy(child.GetComponent<GameObject>());
            }

            Lanes = calculateLanesParameters(lanesWidth.Length, lanesWidth, gapBetweenLines, centerSizeHalf);

            InistiatePrefabsOfDiffrentLanes(ref Lanes, CenterPrefab, LaneStandardPrefab, FirstLineOfCenterPrefab, SecondLineOfCenterPrefab, PlatformPrefab, StartPlatform);
            AssignParamtersOfWalls(ref Lanes, wallSize, wallSize);

            for (var i = 0; i < Lanes.Count; i++)
            {
                var nextLaneWidht = i == Lanes.Count - 1 ? Lanes[Lanes.Count - 1].LaneWidth : Lanes[i + 1].LaneWidth;

                Lanes[i].SetGroundAndWallsParameters();
                Lanes[i].SetSafeZoneParameters(nextLaneWidht);
            }
            Lanes[0].InstantiateRamp(LaneStandardPrefab, Terrain.transform); // adding ramp near center

            CreateFlyingDroneColliders(Lanes, FlyingDroneCollider, FlyingDroneColliderOffset, Terrain.transform);
        }

        public List<GameObject> GetSafeZones()
        {
            return SafeZones;
        }

        public Area[] GetDroneSpawnArea()
        {
            Area[] lanes = new Area[Lanes.Count+1];
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

        private void InistiatePrefabsOfDiffrentLanes(ref List<ALane> lanes, GameObject CenterPrefab, GameObject LaneStandardPrefab, GameObject FirstLineOfCenterPrefab, GameObject SecondLineOfCenterPrefab, GameObject platformPrefab, GameObject StartPlatform)
        {
            SafeZones.Add(lanes[0].InstatiateCompleteLanePrefab(FirstLineOfCenterPrefab, CenterPrefab, Terrain.transform));
            SafeZones.Add(lanes[1].InstatiateCompleteLanePrefab(SecondLineOfCenterPrefab, platformPrefab, Terrain.transform));
            for (int i = 2; i < lanes.Count - 1; i++)
            {
                SafeZones.Add(lanes[i].InstatiateCompleteLanePrefab(LaneStandardPrefab, platformPrefab, Terrain.transform));
            }
            SafeZones.Add(lanes[lanes.Count - 1].InstatiateCompleteLanePrefab(LaneStandardPrefab, StartPlatform, Terrain.transform)); // starting lane
        }



        private void AssignParamtersOfWalls(ref List<ALane> lanes, float wallWidth, float colliderWidth)
        {
            ALane.WallSize = wallWidth;
            ALane.ColliderWallSize = colliderWidth;
        }

        private List<ALane> calculateLanesParameters(int spiralsAmount, float[] linesWidht, float gap, float centerSize)
        {
            List<ALane> lanes = new List<ALane>(0);

            // create first (near center) lines, left one is first, then bottom right top
            lanes.Add(new LaneFirstOffCenter(new Vector3(-(gap + centerSize + linesWidht[0] / 2), 0, -gap / 2),
                                              new Vector3(0F, -90F, 0F),
                                              new Vector3(gap + 2 * centerSize, 1F, linesWidht[0]),
                                              centerSize));

            lanes.Add(new LaneSecondOffCenter(new Vector3(-(linesWidht[0] / 2), 0, -(centerSize + gap + linesWidht[0] / 2)),
                                        new Vector3(0F, 180F, 0F),
                                        new Vector3(2 * gap + 2 * centerSize + linesWidht[0], 1F, linesWidht[0])));

            lanes.Add(new LaneStandard(new Vector3(centerSize + gap + linesWidht[0] / 2, 0, 0),
                                        new Vector3(0F, 90F, 0F),
                                        new Vector3(2 * (gap + centerSize), 1F, linesWidht[0])));

            lanes.Add(new LaneStandard(new Vector3(-(gap + linesWidht[0]) / 2, 0, centerSize + gap + linesWidht[0] / 2),
                                        new Vector3(0F, 0F, 0F),
                                        new Vector3(3 * gap + 2 * centerSize + linesWidht[0], 1F, linesWidht[0])));

            for (int i = 1; i < spiralsAmount; i++)
            {
                if (i == 1) // left and bottom line of 1st spiral (starts of 0 (center one)) have diffrence dependicies
                {
                    lanes.Add(new LaneStandard(new Vector3(lanes[4 * (i - 1)].Pos.x - gap - (linesWidht[i] + linesWidht[i - 1]) / 2, 0F, lanes[4 * (i - 1)].Pos.z - linesWidht[i - 1] / 2),
                                                new Vector3(0F, -90F, 0F),
                                                new Vector3(lanes[4 * (i - 1)].Sc.x + 2 * gap + linesWidht[i - 1], 1F, linesWidht[i])));

                    lanes.Add(new LaneStandard(new Vector3(0F, 0F, lanes[4 * (i - 1) + 1].Pos.z - (gap + (linesWidht[i - 1] + linesWidht[i]) / 2)),
                                                new Vector3(0F, 180F, 0F),
                                                new Vector3(lanes[4 * (i - 1) + 1].Sc.x + 2 * gap + linesWidht[i - 1], 1F, linesWidht[i])));
                }
                else
                {
                    lanes.Add(new LaneStandard(new Vector3(lanes[4 * (i - 1)].Pos.x - gap - (linesWidht[i] + linesWidht[i - 1]) / 2, 0F, lanes[4 * (i - 1)].Pos.z + (linesWidht[i - 2] - linesWidht[i - 1]) / 2),
                                                new Vector3(0F, -90F, 0F),
                                                new Vector3(lanes[4 * (i - 1)].Sc.x + 2 * gap + linesWidht[i - 1] + linesWidht[i - 2], 1F, linesWidht[i])));

                    lanes.Add(new LaneStandard(new Vector3(0F, 0F, lanes[4 * (i - 1) + 1].Pos.z - (gap + (linesWidht[i - 1] + linesWidht[i]) / 2)),
                                                new Vector3(0F, 180F, 0F),
                                                new Vector3(lanes[4 * (i - 1) + 1].Sc.x + 2 * gap + 2 * linesWidht[i - 1], 1F, linesWidht[i])));
                }
                lanes.Add(new LaneStandard(new Vector3(lanes[4 * (i - 1) + 2].Pos.x + gap + (linesWidht[i] + linesWidht[i - 1]) / 2, 0F, 0F),
                                            new Vector3(0F, 90F, 0F),
                                            new Vector3(lanes[4 * (i - 1) + 2].Sc.x + 2 * gap + 2 * linesWidht[i - 1], 1F, linesWidht[i])));
                lanes.Add(new LaneStandard(new Vector3(lanes[4 * (i - 1) + 3].Pos.x + (-linesWidht[i] + linesWidht[i - 1]) / 2, 0F, lanes[4 * (i - 1) + 3].Pos.z + gap + (linesWidht[i - 1] + linesWidht[i]) / 2),
                                            new Vector3(0F, 0F, 0F),
                                            new Vector3(lanes[4 * (i - 1) + 3].Sc.x + 2 * gap + linesWidht[i - 1] + linesWidht[i], 1F, linesWidht[i])));
            }
            return lanes;
        }

        private void CreateFlyingDroneColliders(List<ALane> lanes, GameObject FlyingDroneCollider, float flyingDroneColliderOffset, Transform Terrain)
        {
            List<GameObject> flyingDroneColliders = new List<GameObject>(0);
            Vector3 colliderLen = lanes[lanes.Count - 1].Sc + new Vector3(2 * lanes[lanes.Count - 1].LaneWidth + 2 * flyingDroneColliderOffset, 10f, 0f);
            for (int i = 0; i < 4; i++)
            {
                flyingDroneColliders.Add(Instantiate(FlyingDroneCollider, Terrain));
                flyingDroneColliders[i].transform.position = lanes[lanes.Count - 1 - i].Pos + lanes[lanes.Count - 1 - i].LanePrefab.transform.forward * flyingDroneColliderOffset;
                flyingDroneColliders[i].transform.localEulerAngles = lanes[lanes.Count - 1 - i].Rot;
                flyingDroneColliders[i].transform.localScale = colliderLen;
            }
        }
    }   
}
