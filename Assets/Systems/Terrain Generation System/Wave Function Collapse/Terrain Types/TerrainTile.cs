using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile : MonoBehaviour
{
    public static readonly int xLength = 50;
    public static readonly int zLength = 50;
    public static readonly int verticiesPerLength = 3;

    public Mesh mesh;

    Vector3[] verticies;
    int[] triangles;

    public List<TerrainTileBorder> canBorder = new List<TerrainTileBorder>();

    public void CreateMesh()
    {

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        verticies = new Vector3[zLength * xLength * verticiesPerLength * verticiesPerLength];
        triangles = new int[verticies.Length * 6];

        for (int z = 0, i=0; z < zLength  * verticiesPerLength; z++)
        {
            for (int x = 0; x < xLength * verticiesPerLength; x++)
            {
                verticies[i] = new Vector3(x * xLength/(float)verticiesPerLength, 0, z * zLength/(float)verticiesPerLength);
                i++;
            }
        }

        int vert = 0, tri = 0;
            for (int z = 0; z < zLength * verticiesPerLength - 1; z++)
            {
                for (int x = 0; x < xLength * verticiesPerLength - 1; x++)
                {
                    triangles[tri + 0] = vert + 0;
                    triangles[tri + 1] = vert + 0 + xLength * verticiesPerLength;
                    triangles[tri + 2] = vert + 1;
                    triangles[tri + 3] = vert + 1;
                    triangles[tri + 4] = vert + 0 + xLength * verticiesPerLength;
                    triangles[tri + 5] = vert + 1 + xLength * verticiesPerLength;

                    vert++;
                    tri += 6;
                }
                vert++;
            }
    }

}

[System.Serializable]
public class TerrainTileBorder
{
    public TerrainTile terrainTile;
    [Range(0, 10)] public float weight;
}
