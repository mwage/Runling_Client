using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.RLR
{
    public class GenerateMap : MonoBehaviour {
        public GameObject Terrain;

        public void GenerateMapRLR()
        {
            int spiralsAmount = 5;
            int lineWidth = 7;
            float wallHeight = 3;
            int centerSize = 2;
            List<GameObject> linesGround = CreateLinesGround(spiralsAmount, lineWidth, centerSize);
            List<GameObject> linesWalls = CreateLinesWalls(spiralsAmount, wallHeight, lineWidth, centerSize, 14);
            List<GameObject> safeZones = CreateSafeZones(ref linesGround, lineWidth, centerSize);



        }

        private GameObject createCube(Vector3 pos, Vector3 rot, Vector3 scale, float segmentDimension = 1, int layer = 0, string tag = "")
        {
            GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
            line.transform.position = pos;
            line.transform.rotation = Quaternion.identity;
            line.transform.Rotate(rot);
            line.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
            line.layer = layer;
            if (tag != "") line.tag = tag;
            return line;
        }

        private List<GameObject> CreateLinesGround(int spiralsAmount, float lineWidth, int centerSize)
        {
            // center is centerSize*lineWidht square (lings invernuable there)
            List<GameObject> linesGround = new List<GameObject>(0);
            int centerOffset = centerSize - 1; // offset of lines out of the center of map, because of center size lenght must be 2*(centerSize - 1)*lineWidht
            for (var i = 0; i < spiralsAmount; i++)
            {
                // i - index of spiral that is being created
                // 4 linesGround (top, right, down, left) are done in loop
                linesGround.Add(createCube(new Vector3(-0.5F, 0, spiralsAmount - i + centerOffset) * lineWidth, new Vector3(0F, 0F, 0F), new Vector3((2 * spiralsAmount - 2 * i + 2 * centerOffset) * lineWidth, 1F, lineWidth)));
                linesGround.Add(createCube(new Vector3(spiralsAmount - i + centerOffset, 0F, 0F) * lineWidth, new Vector3(0F, 90F, 0F), new Vector3((2 * spiralsAmount - 2 * i - 1 + 2 * centerOffset) * lineWidth, 1F, lineWidth)));
                if (i == spiralsAmount - 1) break; // dont create last bottom and left line because they are diffrent
                linesGround.Add(createCube(new Vector3(0F, 0F, -spiralsAmount + i - centerOffset) * lineWidth, new Vector3(0F, 180F, 0F), new Vector3((2 * spiralsAmount - 2 * i - 1 + 2 * centerOffset) * lineWidth, 1F, lineWidth)));
                linesGround.Add(createCube(new Vector3(-spiralsAmount + i - centerOffset, 0F, -0.5F) * lineWidth, new Vector3(0F, 270F, 0F), new Vector3((2 * spiralsAmount - 2 * i - 2 + 2 * centerOffset) * lineWidth, 1F, lineWidth)));
            }
            // create last line
            linesGround.Add(createCube(new Vector3(-0.5F, 0F, -0.5F) * lineWidth, new Vector3(0F, 0F, 0F), new Vector3(2 * centerSize * lineWidth, 1F, 2 * centerSize * lineWidth)));
            foreach (var line in linesGround) line.transform.parent = Terrain.transform;
            return linesGround;
        }

        private List<GameObject> CreateLinesWalls(int spiralsAmount, float wallHeight, float lineWidth, int centerSize, int layer = 0)
        {
            List<GameObject> linesWall = new List<GameObject>(0);
            int centerOffset = centerSize - 1; // offset of lines out of the center of map
            for (var i = 0; i < spiralsAmount + 1; i++)
            {
                // create wall "above" the wall
                linesWall.Add(createCube(new Vector3(-0.5F, 0, spiralsAmount - i + 0.5F + centerOffset) * lineWidth, new Vector3(0F, 0F, 0F), new Vector3((2 * spiralsAmount - 2 * i + 2 + 2 * centerOffset) * lineWidth, wallHeight, 0.1F)));
                linesWall.Add(createCube(new Vector3(spiralsAmount - i + 0.5F + centerOffset, 0F, 0F) * lineWidth, new Vector3(0F, 90F, 0F), new Vector3((2 * spiralsAmount - 2 * i - 1 + 2 + 2 * centerOffset) * lineWidth, wallHeight, 0.1F)));
                //if (i == spiralsAmount ) break; // dont create last bottom and left wall of lines, because they are diffrent
                linesWall.Add(createCube(new Vector3(0F, 0F, -spiralsAmount + i - 0.5F - centerOffset) * lineWidth, new Vector3(0F, 0F, 0F), new Vector3((2 * spiralsAmount - 2 * i - 1 + 2 + 2 * centerOffset) * lineWidth, wallHeight, 0.1F)));
                linesWall.Add(createCube(new Vector3(-spiralsAmount + i - 0.5F - centerOffset, 0F, -0.5F) * lineWidth, new Vector3(0F, 90F, 0F), new Vector3((2 * spiralsAmount - 2 * i - 2 + 2 + 2 * centerOffset) * lineWidth, wallHeight, 0.1F)));
            }
            foreach (var wall in linesWall)
            {
                wall.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
                wall.transform.parent = Terrain.transform;
                wall.layer = layer;
            }
            return linesWall;
        }

        private List<GameObject> CreateSafeZones(ref List<GameObject> linesGrounds, float lineWidth, int centerSize)
        {
            // safe ground is on left side (-x local axis) of line ground. Its needed to create safeZone for (last-1) line on its right side. And safeZone for finish level
            List<GameObject> safeZones = new List<GameObject>(0);
            Vector3 safeZonePosition;
            for (int i = 0; i < linesGrounds.Count - 1; i++)
            {
                safeZonePosition = linesGrounds[i].transform.position - linesGrounds[i].transform.right * (linesGrounds[i].transform.localScale.x + lineWidth) / 2; // position of center of safe zone (basing on i-th line ground)
                safeZones.Add(createCube(safeZonePosition, Vector3.zero, new Vector3(lineWidth, 1F, lineWidth)));
                safeZones[i].GetComponent<Renderer>().material.color = new Color(1, 0, 0);
            }
            //create safeZone on right side of last-1 line
            safeZonePosition = linesGrounds[safeZones.Count - 1].transform.position + linesGrounds[safeZones.Count - 1].transform.right * (linesGrounds[safeZones.Count - 1].transform.localScale.x + lineWidth) / 2; // position of center of safe zone (basing on i-th line ground)
            safeZones.Add(createCube(safeZonePosition, Vector3.zero, new Vector3(lineWidth, 1F, lineWidth)));
            safeZones[safeZones.Count - 1].GetComponent<Renderer>().material.color = new Color(1, 0, 0);
            // create final safe zone
            safeZones.Add(createCube(Vector3.zero, Vector3.zero, new Vector3((2 * centerSize - 1) * lineWidth, 1.1F, (2 * centerSize - 1) * lineWidth)));
            safeZones[safeZones.Count - 1].GetComponent<Renderer>().material.color = new Color(1, 0, 0);
            foreach (var safeZone in safeZones) safeZone.transform.parent = Terrain.transform;
            return safeZones;
        }
    }
}

