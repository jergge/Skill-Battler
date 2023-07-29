using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCTerrainManager : MonoBehaviour
{
    [SerializeField] int areasHigh;
    [SerializeField] int areasWide;

    public List<TerrainArea> areas = new List<TerrainArea>();

    TerrainArea[,] typesArray;

    public void GenerateAreas()
    {
        ClearTerrain();
        typesArray = new TerrainArea[areasWide, areasHigh];

        for (int x = 0; x < areasWide; x++)
        {
            for (int z = 0; z < areasHigh; z++)
            {
                var temp = GameObject.Instantiate<TerrainArea>(areas[0], new Vector3(x * TerrainArea.width, 0, z * TerrainArea.height), Quaternion.identity);
                temp.transform.SetParent(transform);
                temp.name = x + ", " + z;
                typesArray[x, z] = temp;
            }
        }

        FillNeighbours();
        FillAreas();
    }

    public void ClearTerrain()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            //transform.GetChild(i).gameObject.GetComponent<TerrainChunk>().Invoke("DestroyInEditMode", .01f);
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    void FillNeighbours()
    {
        for (int x = 0; x < areasWide; x++)
        {
            for (int z = 0; z < areasHigh; z++)
            {
                TerrainArea north =     (z < areasHigh - 1) ? typesArray[x, z + 1] : null;
                TerrainArea south =     (z > 0)             ? typesArray[x, z - 1] : null;
                TerrainArea east =      (x < areasWide - 1) ? typesArray[x + 1, z] : null;
                TerrainArea west =      (x > 0)             ? typesArray[x - 1, z] : null;
                TerrainArea northeast = (north is not null && east is not null) ? typesArray[x + 1, z + 1] : null;
                TerrainArea southeast = (south is not null && east is not null) ? typesArray[x + 1, z - 1] : null;
                TerrainArea southwest = (south is not null && west is not null) ? typesArray[x - 1, z - 1] : null;
                TerrainArea northwest = (north is not null && west is not null) ? typesArray[x - 1, z + 1] : null;

                typesArray[x, z].north = north;
                typesArray[x, z].south = south;
                typesArray[x, z].east = east;
                typesArray[x, z].west = west;
                typesArray[x, z].northEast = northeast;
                typesArray[x, z].southEast = southeast;
                typesArray[x, z].southWest = southwest;
                typesArray[x, z].northWest = northwest;
            }
        }
    }

    void FillAreas()
    {
        for (int x = 0; x < areasWide; x++)
        {
            for (int z = 0; z < areasHigh; z++)
            {
                typesArray[x, z].WFCFillArea();
            }
        }
    }
}
