using UnityEngine;

namespace RLR.MapGenerator
{
    public abstract class ALane
    {
        public GameObject PlatformPrefab;
        public GameObject LanePrefab;

        public static float WallSize { get; set; }
        public static float ColliderWallSize { get; set; }
        public float LaneWidth { get; }
        public float LaneLength { get; }
        public Vector3 Pos { get; set; }
        public Vector3 Rot { get; set; }
        public Vector3 Sc { get; set; }

        private readonly MapGeneratorRLR _mapGenerator;

        protected ALane(Vector3 position, Vector3 rotation, Vector3 scale, MapGeneratorRLR mapGenerator)
        {
            Pos = position;
            Rot = rotation;
            Sc = scale;
            _mapGenerator = mapGenerator;
            LaneWidth = scale.z;
            LaneLength = scale.x;
        }

        public virtual GameObject InstatiateCompleteLanePrefab(GameObject firstLineOfCenterPrefab, GameObject platformPrefabP, Transform terrain)
        {
            LanePrefab = Object.Instantiate(firstLineOfCenterPrefab, terrain); // assign prefab to local ref
            return PlatformPrefab = Object.Instantiate(platformPrefabP, terrain);
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
                    var localPos = ch.transform.localPosition;
                    var localScale = ch.transform.localScale;

                    // Scale ground
                    if (ch.CompareTag("Ground"))
                    {
                        ch.transform.localScale = new Vector3(LaneLength, localScale.y, LaneWidth);
                        SetLaneTexture(ch);
                    }

                    // Position and scale walls and colliders
                    if (ch.CompareTag("Top"))
                    {
                        ch.transform.localScale = new Vector3(LaneLength, localScale.y, WallSize);
                        ch.transform.localPosition = new Vector3(0, localPos.y, (LaneWidth + WallSize) / 2);
                        SetHorizontalWallTexture(ch);
                    }
                    else if (ch.CompareTag("Bottom"))
                    {
                        ch.transform.localScale = new Vector3(LaneLength, localScale.y, WallSize);
                        ch.transform.localPosition = new Vector3(0, localPos.y, -(LaneWidth + WallSize) / 2);
                        SetHorizontalWallTexture(ch);
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
                    // Scale walls and colliders
                    if (ch.CompareTag("Top")) 
                    {
                        SetDroneCollider(ch, child, 0F, (LaneWidth + ColliderWallSize) / 2, nextLineWidth, WallSize);
                        SetWallOrPlayerCollider(ch, child, 0F, (LaneWidth + WallSize) / 2, nextLineWidth, WallSize);
                    }

                    else if (ch.CompareTag("Right"))
                    {
                        SetDroneCollider(ch, child, (nextLineWidth - WallSize) / 2, 0F, ColliderWallSize, LaneWidth);
                    }
                    // scale individual 
                    
                    else if (ch.CompareTag("Bottom"))
                    {
                        SetDroneCollider(ch, child, 0F, -(LaneWidth  - WallSize ) / 2, nextLineWidth, ColliderWallSize);
                        SetWallOrPlayerCollider(ch, child, -WallSize/2, -(LaneWidth  + WallSize) / 2, nextLineWidth + WallSize, ColliderWallSize);

                    }

                    else if (ch.CompareTag("Left"))
                    {
                        SetWallOrPlayerCollider(ch, child, -(nextLineWidth + WallSize) / 2, WallSize/2, WallSize, LaneWidth+WallSize);
                        SetDroneCollider(ch, child, -(nextLineWidth + WallSize) / 2, 0F, WallSize, LaneWidth);
                    }

                    // Scale Ground
                    if (ch.CompareTag("Ground") || ch.CompareTag("SafeZone"))
                    {
                        SetWallOrPlayerCollider(ch, child, 0F, 0F, nextLineWidth, LaneWidth);
                    }

                    // Set material tiling
                    if (ch.CompareTag("Ground"))
                    {
                        SetPlatformTexture(ch);
                    }
                }
            }

            // Set position and rotation of the lane
            PlatformPrefab.transform.position = Pos - LanePrefab.transform.right * (LaneLength + nextLineWidth) / 2;
            PlatformPrefab.transform.localEulerAngles = Rot;
        }

        protected void SetDroneCollider(Transform collider, Transform colliderParent, float posX, float posZ, float scX, float scZ)
        {  
            if (colliderParent.name == "DroneCollider")
            {
                collider.transform.localScale = new Vector3(scX, collider.transform.localScale.y, scZ);
                collider.transform.localPosition = new Vector3(posX, collider.transform.localPosition.y, posZ);
            }
        }

        protected void SetWallOrPlayerCollider(Transform obj, Transform objParent, float posX, float posZ, float scX, float scZ)
        {
            if (objParent.name != "DroneCollider")
            {
                obj.transform.localScale = new Vector3(scX, obj.transform.localScale.y, scZ);
                obj.transform.localPosition = new Vector3(posX, obj.transform.localPosition.y, posZ);
                SetHorizontalWallTexture(obj);
            }
        }

        protected void SetPlatformTexture(Transform ch)
        {
            SetGroundTexture(ch, _mapGenerator.PlatformTilingX, _mapGenerator.PlatformTilingZ);
        }

        protected void SetHorizontalWallTexture(Transform ch)
        {
            SetWallTexture(ch, _mapGenerator.WallTilingX, _mapGenerator.WallTilingY);
        }

        protected void SetVerticalWallTexture(Transform ch)
        {
            SetWallTexture(ch, _mapGenerator.WallTilingX, _mapGenerator.WallTilingY);
        }

        protected void SetLaneTexture(Transform ch)
        {
            SetGroundTexture(ch, _mapGenerator.LaneTilingX, _mapGenerator.LaneTilingZ);
        }

        private void SetGroundTexture(Transform ch, float tilingX, float tilingZ)
        {
            if (ch.parent.name == "VisibleObjects")
            {
                ch.GetComponent<Renderer>().material.mainTextureScale = new Vector2(ch.localScale.x / tilingX, ch.localScale.z / tilingZ);
                ch.GetComponent<Renderer>().material.SetTextureScale("_BumpMap", new Vector2(ch.localScale.x / tilingX, ch.localScale.z / tilingZ));
            }
        }

        private void SetWallTexture(Transform ch, float tilingX, float tilingZ)
        {
            if (ch.parent.name == "VisibleObjects")
            {
                if (ch.CompareTag("Top") || ch.CompareTag("Bottom"))
                {
                    ch.GetComponent<Renderer>().material.mainTextureScale = new Vector2(ch.localScale.x / tilingX, ch.localScale.y / tilingZ);
                    ch.GetComponent<Renderer>().material.SetTextureScale("_BumpMap", new Vector2(ch.localScale.x / tilingX, ch.localScale.y / tilingZ));
                }
                else
                {
                    ch.GetComponent<Renderer>().material.mainTextureScale = new Vector2(ch.localScale.z / tilingX, ch.localScale.y / tilingZ);
                    ch.GetComponent<Renderer>().material.SetTextureScale("_BumpMap", new Vector2(ch.localScale.z / tilingX, ch.localScale.y / tilingZ));
                }
            }
        }
    }
}
