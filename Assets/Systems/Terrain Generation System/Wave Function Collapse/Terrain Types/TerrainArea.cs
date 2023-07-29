using System.Collections;
using System.Collections.Generic;
using Jergge.Extensions;
using UnityEngine;

public class TerrainArea : MonoBehaviour
{
    public static readonly int height = TerrainTile.zLength * 10;
    public static readonly int width = TerrainTile.xLength * 10;

    public List<TerrainTile> terrainTiles = new List<TerrainTile>();

    TerrainTile[,] tiles;

    public TerrainArea north;
    public TerrainArea northEast;
    public TerrainArea east;
    public TerrainArea southEast;
    public TerrainArea south;
    public TerrainArea southWest;
    public TerrainArea west;
    public TerrainArea northWest;

    public void WFCFillArea()
    {
        tiles = new TerrainTile[width, height];
        FillSuperpositions();
    }

    void FillSuperpositions()
    {
        TerrainTile randomStart = terrainTiles.Random();

        tiles[(int)Random.Range(0, width), (int)Random.Range(0, height)] = randomStart;


        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {

            }
        }
    }

}


