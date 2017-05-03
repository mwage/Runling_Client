using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Assets.Scripts.Drones;

namespace Assets.Scripts.RLR
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

        protected List<Lane> Lanes;
        

        public void GenerateMap(int centerSize, float[] lanesWidth, float gapBetweenLines, float wallSize)
        {
            int centerSizeHalf = centerSize / 2;
            FlyingDroneColliderOffset = 20;
            int spiralsAmount = lanesWidth.Length;

            // Destroy previous level
            foreach (Transform child in Terrain.transform)
            {
                Destroy(child.GetComponent<GameObject>());
            }

            Lanes = calculateLanesParameters(spiralsAmount, lanesWidth, gapBetweenLines, centerSizeHalf);

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

        private void InistiatePrefabsOfDiffrentLanes(ref List<Lane> lanes, GameObject CenterPrefab, GameObject LaneStandardPrefab, GameObject FirstLineOfCenterPrefab, GameObject SecondLineOfCenterPrefab, GameObject platformPrefab, GameObject StartPlatform)
        {
            lanes[0].InstatiateCompleteLanePrefab(FirstLineOfCenterPrefab, CenterPrefab, Terrain.transform);
            lanes[1].InstatiateCompleteLanePrefab(SecondLineOfCenterPrefab, platformPrefab, Terrain.transform);
            for (int i = 2; i < lanes.Count - 1; i++)
            {
                lanes[i].InstatiateCompleteLanePrefab(LaneStandardPrefab, platformPrefab, Terrain.transform);
            }
            lanes[lanes.Count - 1].InstatiateCompleteLanePrefab(LaneStandardPrefab, StartPlatform, Terrain.transform); // starting lane
        }

        private void AssignParamtersOfWalls(ref List<Lane> lanes, float wallWidth, float colliderWidth)
        {
            Lane.WallSize = wallWidth;
            Lane.ColliderWallSize = colliderWidth;
        }

        private List<Lane> calculateLanesParameters(int spiralsAmount, float[] linesWidht, float gap, float centerSize)
        {
            List<Lane> lanes = new List<Lane>(0);

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

        private void CreateFlyingDroneColliders(List<Lane> lanes, GameObject FlyingDroneCollider, float flyingDroneColliderOffset, Transform Terrain)
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

        public abstract class Lane
        {
            public Vector3 Pos, Rot, Sc;
            public GameObject LineGround;
            public GameObject LineWallUpper;
            public GameObject LineWallLower;
            public GameObject PlatformPrefab;
            public GameObject LanePrefab;
            public static float WallSize, ColliderWallSize;
            public readonly float LaneWidth, LaneLength;

            protected Lane(Vector3 position, Vector3 rotation, Vector3 scale)  
            {
                Pos = position;
                Rot = rotation;
                Sc = scale;
                LaneWidth = scale.z;
                LaneLength = scale.x;
            }

            public virtual void InstatiateCompleteLanePrefab(GameObject firstLineOfCenterPrefab, GameObject platformPrefabP, Transform terrain)
            {
                LanePrefab = Instantiate<GameObject>(firstLineOfCenterPrefab, terrain); // assign prefab to local ref
                PlatformPrefab = Instantiate<GameObject>(platformPrefabP, terrain);
            }

            public virtual void InstantiateRamp(GameObject ramp, Transform terrain)
            {
            }

            public virtual void SetGroundAndWallsParameters()
            {
                foreach (Transform child in LanePrefab.transform)
                {
                    foreach (Transform ch in child)
                    {
                        Vector3 localPos = ch.transform.localPosition;
                        Vector3 localScale = ch.transform.localScale;

                        // Scale walls and colliders
                        if (ch.tag == "Top" || ch.tag == "Bottom")
                        {
                            ch.transform.localScale = new Vector3(LaneLength, localScale.y, WallSize);
                        }

                        // Scale ground
                        if (ch.tag == "Ground")
                        {
                            ch.transform.localScale = new Vector3(LaneLength, localScale.y, LaneWidth);
                        }

                        // Position walls and colliders
                        if (ch.tag == "Top")
                        {
                            ch.transform.localPosition = new Vector3(0, localPos.y, (LaneWidth + WallSize) / 2);
                        }
                        else if (ch.tag == "Bottom")
                        {
                            ch.transform.localPosition = new Vector3(0, localPos.y, -(LaneWidth + WallSize) / 2);
                        }
                    }
                }

                // Set position and rotation of the lane
                LanePrefab.transform.position = Pos;
                LanePrefab.transform.localEulerAngles = Rot;
            }

            public virtual void SetSafeZoneParameters(float nextLineWidth)
            {
                foreach (Transform child in PlatformPrefab.transform)
                {
                    foreach (Transform ch in child)
                    {
                        Vector3 localPos = ch.transform.localPosition;
                        Vector3 localScale = ch.transform.localScale;

                        // Scale walls and colliders
                        if (ch.tag == "Top" || ch.tag == "Bottom")
                        {
                            ch.transform.localScale = new Vector3(nextLineWidth + 2 * WallSize, localScale.y, WallSize);
                        }
                        if (ch.tag == "Right" || ch.tag == "Left")
                        {
                            ch.transform.localScale = new Vector3(WallSize, localScale.y, LaneWidth + 2 * WallSize);
                        }

                        // Scale ground
                        if (ch.tag == "Ground" || ch.tag == "SafeZone")
                        {
                            ch.transform.localScale = new Vector3(nextLineWidth, localScale.y, LaneWidth);
                        }

                        // Position walls and colliders
                        if (ch.tag == "Top")
                        {
                            ch.transform.localPosition = new Vector3(0, localPos.y, (LaneWidth + WallSize) / 2);
                        }
                        else if (ch.tag == "Bottom")
                        {
                            ch.transform.localPosition = new Vector3(0, localPos.y, -(LaneWidth + WallSize) / 2);
                        }
                        else if (ch.tag == "Right")
                        {
                            ch.transform.localPosition = new Vector3((nextLineWidth + WallSize) / 2, localPos.y, 0F);
                        }
                        else if (ch.tag == "Left")
                        {
                            ch.transform.localPosition = new Vector3(-(nextLineWidth + WallSize) / 2, localPos.y, 0F);
                        }
                    }
                }

                // Set position and rotation of the lane
                PlatformPrefab.transform.position = Pos - LanePrefab.transform.right * (LaneLength + nextLineWidth) / 2;
                PlatformPrefab.transform.localEulerAngles = Rot;
            }
        }

        public class LaneStandard : Lane
        {
            public LaneStandard(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
            {

            }
        }

        public class LaneFirstOffCenter : Lane
        {
            private float _centerSize;
            public LaneFirstOffCenter(Vector3 position, Vector3 rotation, Vector3 scale, float centerSize) : base(position, rotation, scale)
            {
                _centerSize = centerSize;
            }

            public override void InstantiateRamp(GameObject ramp, Transform terrain)
            {

                var thisLane = Instantiate(ramp, terrain);
                thisLane.transform.position = Vector3.zero;
                float gap = -(Pos.x + LaneWidth / 2 + _centerSize);
                foreach (Transform child in thisLane.transform)
                {
                    foreach (Transform ch in child)
                    {
                        Vector3 localPos = ch.transform.localPosition;
                        Vector3 localScale = ch.transform.localScale;

                        // Scale walls and colliders
                        if (ch.tag == "Top" || ch.tag == "Bottom")
                        {
                            ch.transform.localScale = new Vector3(gap, localScale.y, WallSize);
                        }

                        // Scale ground
                        if (ch.tag == "Ground")
                        {
                            ch.transform.localScale = new Vector3(gap, localScale.y, LaneWidth);
                            ch.transform.localPosition = new Vector3(-(_centerSize + gap / 2), localPos.y, _centerSize - LaneWidth/ 2);
                        }

                        // Position walls and colliders
                        if (ch.tag == "Top")
                        {
                            ch.transform.localPosition = new Vector3(-(_centerSize + gap / 2), localPos.y, _centerSize + WallSize / 2);
                        }
                        else if (ch.tag == "Bottom")
                        {
                            ch.transform.localPosition = new Vector3(-(_centerSize + gap / 2), localPos.y, _centerSize - LaneWidth - WallSize / 2);
                        }
                    }
                }
            }


            public override void SetGroundAndWallsParameters()
            {
                base.SetGroundAndWallsParameters();
                foreach (Transform child in LanePrefab.transform)
                {
                    foreach (Transform ch in child)
                    {
                        Vector3 localPos = ch.transform.localPosition;
                        Vector3 localScale = ch.transform.localScale;

                        // Scale ground
                        if (ch.tag == "Right")
                        {
                            ch.transform.localScale = new Vector3(WallSize, localScale.y, LaneWidth);
                            ch.transform.localPosition = new Vector3((LaneLength + WallSize) / 2, localPos.y, 0F);
                        }

                        if (ch.tag == "Bottom")
                        {
                            ch.transform.localScale = new Vector3(LaneLength - LaneWidth, localScale.y, WallSize);
                            ch.transform.localPosition = new Vector3(localPos.x - LaneWidth / 2, localPos.y, -(LaneWidth / 2 + WallSize / 2));
                        }
                    }
                }

                // Set position and rotation of the lane
                LanePrefab.transform.position = Pos;
                LanePrefab.transform.localEulerAngles = Rot;
            }

            public override void SetSafeZoneParameters(float nextLineWidth)
            {
                foreach (Transform child in PlatformPrefab.transform)
                {
                    foreach (Transform ch in child)
                    {
                        Vector3 localPos = ch.transform.localPosition;
                        Vector3 localScale = ch.transform.localScale;

                        // Scale walls and colliders
                        if (ch.tag == "Top" || ch.tag == "Bottom")
                        {
                            ch.transform.localScale = new Vector3(_centerSize * 2, localScale.y, WallSize);
                        }
                        else if (ch.tag == "Right")
                        {
                            ch.transform.localScale = new Vector3(WallSize, localScale.y, _centerSize * 2 + 2 * WallSize);
                        }
                        else if (ch.tag == "Left")
                        {
                            if (child.name == "DroneCollider")
                            {
                                ch.transform.localScale = new Vector3(WallSize, localScale.y, LaneWidth + 2 * WallSize);
                            }
                            else
                            {
                                ch.transform.localScale = new Vector3(WallSize, localScale.y, 2*_centerSize-LaneWidth);
                            }
                        }

                        // Scale ground and safezone
                        if (ch.tag == "Ground" || ch.tag == "SafeZone")
                        {
                            ch.transform.localScale = new Vector3(_centerSize * 2, localScale.y, _centerSize * 2);
                        }

                        // Position walls and colliders
                        if (ch.tag == "Top")
                        {
                            ch.transform.localPosition = new Vector3(0, localPos.y, _centerSize + WallSize / 2);
                        }
                        else if (ch.tag == "Bottom")
                        {
                            ch.transform.localPosition = new Vector3(0, localPos.y, -_centerSize - WallSize / 2);
                        }
                        else if (ch.tag == "Right")
                        {
                            ch.transform.localPosition = new Vector3(_centerSize + WallSize / 2, localPos.y, 0);
                        }
                        else if (ch.tag == "Left")
                        {
                            ch.transform.localPosition = child.name == "DroneCollider" ? 
                                new Vector3(Pos.x + LaneWidth / 2, localPos.y, (_centerSize - LaneWidth / 2)) : 
                                new Vector3(-(_centerSize + WallSize / 2), localPos.y, -LaneWidth / 2);
                        }
                        if (ch.tag == "Finish")
                        {
                            ch.transform.localScale = new Vector3(_centerSize * 2, localScale.y, _centerSize * 2);

                        }
                    }
                }
            }
        }

        public class LaneSecondOffCenter : Lane
        {
            public LaneSecondOffCenter(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale)
            {

            }

            public override void SetGroundAndWallsParameters()
            {
                foreach (Transform child in LanePrefab.transform)
                {
                    foreach (Transform ch in child)
                    {
                        Vector3 localPos = ch.transform.localPosition;
                        Vector3 localScale = ch.transform.localScale;

                        // Scale ground
                        if (ch.tag == "Ground")
                        {
                            ch.transform.localScale = new Vector3(LaneLength, localScale.y, LaneWidth);
                        }

                        // Position walls and colliders
                        if (ch.tag == "Top")
                        {
                            ch.transform.localScale = new Vector3(LaneLength + WallSize, localScale.y, WallSize);
                            ch.transform.localPosition = new Vector3(0, localPos.y, (LaneWidth + WallSize) / 2);
                        }
                        else if (ch.tag == "Bottom")
                        {
                            ch.transform.localScale = new Vector3(LaneLength - LaneWidth, localScale.y, WallSize);
                            ch.transform.localPosition = new Vector3(-LaneWidth / 2, localPos.y, -(LaneWidth + WallSize) / 2);
                        }
                        else if (ch.tag == "Right")
                        {
                            ch.transform.localScale = new Vector3(WallSize, localScale.y, LaneWidth);
                            ch.transform.localPosition = new Vector3((LaneLength + WallSize) / 2, localPos.y, 0F);
                        }
                    }
                }

                // Set position and rotation of the lane
                LanePrefab.transform.position = Pos;
                LanePrefab.transform.localEulerAngles = Rot;
            }
        }
    }
}
