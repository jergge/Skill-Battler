using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainGeneration;

public class SpawnManager : MonoBehaviour
{
    public WorldMeshGenerator worldMesh;
    public List<EnemySpawns> randomEnemies;
    public Player player;

    public float extraHeight = 4f;

    public float distanceFromEdge = 100f;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
        SpawnEnemies();
    }

    void SpawnPlayer()
    {
        Vector2 mapCentre2D = worldMesh.GetCentre2D();
        Vector3 pos = new Vector3 (mapCentre2D.x, worldMesh.GetHeightAtPoint(new Vector3(mapCentre2D.x, 0, mapCentre2D.y)) + extraHeight, mapCentre2D.y);

        //float spawnHeight = worldMesh.getHeightAtPoint(transform.position);

       // pos = pos + Vector3.up * (spawnHeight+extraHeight + worldMesh.transform.position.y);

        GameObject.Instantiate(player, pos, Quaternion.identity);

    }

    void SpawnEnemies()
    {
        System.Random prng = new System.Random();
        foreach( EnemySpawns type in randomEnemies)
        {
            for ( int i = 0; i < type.count; i++)
            {
                float xCoord;
                float zCoord;

                xCoord = prng.Next(Mathf.RoundToInt(worldMesh.transform.position.x + distanceFromEdge), Mathf.RoundToInt(worldMesh.transform.position.x + worldMesh.worldSizeX - distanceFromEdge));
                zCoord = prng.Next(Mathf.RoundToInt(worldMesh.transform.position.z + distanceFromEdge), Mathf.RoundToInt(worldMesh.transform.position.z + worldMesh.worldSizeZ - distanceFromEdge));

                Vector3 pos = new Vector3(xCoord, worldMesh.GetHeightAtPoint(new Vector3(xCoord, 0, zCoord)) + extraHeight, zCoord);

                GameObject.Instantiate<LivingEntity>(type.prefab, pos, Quaternion.identity);
            }
        }
    }
}

[System.Serializable]
public struct EnemySpawns
{
    public LivingEntity prefab;
    public int count;
}
