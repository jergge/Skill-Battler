using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TerrainGeneration{
public class WorldMeshGenerator : MonoBehaviour
{
    [Header("Noise Sampler Settings")]
    public NoiseSampler noiseSampler;
    public bool randomInBuild = false;
    public Vector2 randomOffsetMinMax;
    Vector2 randomOffset;
    public Vector2 randomScaleMinMax;
    float randomScale;

    [Header("World Size")]
    public float worldSizeX;
    public float worldSizeZ;
    public AnimationCurve heightCurve;
    [Range(0,100)]
    public float heightScale;

    [Header("Vertex Resolution")]
    //verticies per unity unit
    [Range(.05f, 3)] public float verteciesPerUnitEditor = 1f;
    [Range(.05f, 3)] public float verteciesPerUnitGame = 2f;
    float verticiesPerUnit => (Application.isPlaying) ? verteciesPerUnitGame : verteciesPerUnitEditor;
    public bool autoUpdate;

    [Header("Physics Settings")]
    public bool bakeCollider;
    public float raycastTestHeight = 200;

    [Header("Matieral Settings")]
    public Material heightMapMaterial;
    public bool generateWaterMesh = true;
    public Material flatWaterMaterial;
    public Layer[] layers = new Layer[8];


    int totalVerticiesX;
    int totalVerticiesZ;


    float maxMeshHeight;
    float minMeshHeight;

    const int textureSize = 512;
    const TextureFormat textureFormat = TextureFormat.RGB565;

    public float GetHeightAtPoint(Vector2 point)
    {
        RaycastHit hitInfo;
        //Debug.Log("Testing height at point " + point);
        if(Physics.Raycast(new Vector3(point.x, raycastTestHeight + maxMeshHeight, point.y), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Walls and Floors"), QueryTriggerInteraction.UseGlobal))
        {
            return hitInfo.point.y;
        }

        throw new System.OverflowException("could not find terrain with the raycast");
    }

    public void ApplyToMaterial(Material material)
    {
        material.SetInt ("layerCount", layers.Length);
        material.SetColorArray ("baseColours", layers.Select(x => x.tint).ToArray());
        material.SetFloatArray ("baseStartingHeights", layers.Select(x => x.startHeight).ToArray());
        material.SetFloatArray ("baseBlends", layers.Select(x => x.blendStrength).ToArray());
        material.SetFloatArray ("baseColourStrength", layers.Select(x => x.tintStrength).ToArray());
        material.SetFloatArray ("baseTextureScales", layers.Select(x => x.textureScale).ToArray());
        Texture2DArray texturesArray = GenerateTextureArray(layers.Select(x => x.texture).ToArray());
        material.SetTexture("baseTextures", texturesArray);

        float tempMin = (heightCurve.Evaluate(-1) *2 -1) * heightScale;
        material.SetFloat("minHeight", tempMin);
        float tempMax = (heightCurve.Evaluate(1) *2 -1) * heightScale;
        material.SetFloat("maxHeight", tempMax);
    }

    Texture2DArray GenerateTextureArray(Texture2D[] textures) 
    {
        Texture2DArray texArray = new Texture2DArray(textureSize, textureSize, textures.Length, textureFormat, true);
        int i = 0;
        foreach ( Texture2D t in textures)
        {
            texArray.SetPixels(t.GetPixels(), i);
            i++;
        }
        texArray.Apply();
        return texArray;
    }

    void Awake()
    {
        if (randomInBuild)
        {
            GenerateNewRandom();
        }
        noiseSampler.Reset();
        GenerateNewChuks();
        //GenerateWater();
    }

    void GenerateNewRandom()
    {
        randomOffset = new Vector2(Random.Range(-1000, 1000), Random.Range(-1000, 1000));
        randomScale = Random.Range(100,300);

        noiseSampler.scale = randomScale;
        noiseSampler.offset = randomOffset;
    }

    void GenerateWater()
    {
        if (!generateWaterMesh)
        {
            return;
        }
        //Debug.Log("min: " + minMeshHeight + "  | max: " + maxMeshHeight);
        float topOfWater = Mathf.Lerp(minMeshHeight, maxMeshHeight, layers[1].startHeight);
        // float topOfWater = 100f;

        Vector3[] verticies = new Vector3[4] 
        {
            new Vector3(0,          topOfWater, 0           ),      //bottom left
            new Vector3(0,          topOfWater, worldSizeZ  ),      //top left
            new Vector3(worldSizeX, topOfWater, 0           ),      //bottom right
            new Vector3(worldSizeX, topOfWater, worldSizeZ  )       //top right
        };
        int[] triangles = new int[6] 
        {
            0, 1, 2,
            1, 3, 2
        };


        GameObject waterObj = new GameObject("Water Mesh");
        waterObj.AddComponent<MeshRenderer>();
        waterObj.AddComponent<MeshFilter>();
        waterObj.transform.SetParent(transform);

        Mesh mesh = new Mesh();
        
        waterObj.GetComponent<MeshFilter>().sharedMesh = mesh;

        mesh.Clear();

        mesh.SetVertices(verticies);
        mesh.SetTriangles(triangles, 0);

        waterObj.GetComponent<MeshRenderer>().sharedMaterial = flatWaterMaterial;

        waterObj.layer = gameObject.layer;

        mesh.RecalculateNormals();

    }

    public void GenerateNewChuks()
    {
        //delete all the existing terrain chunks that are children
        ClearChunks();

        //The total # of verticies in each direction of the world mesh
        totalVerticiesX = Mathf.CeilToInt(worldSizeX * verticiesPerUnit);
        totalVerticiesZ = Mathf.CeilToInt(worldSizeZ * verticiesPerUnit);
        // Debug.Log("Total Verticies x / z:     " + totalVerticiesX + " / " + totalVerticiesX);

        //How far apart the verticies must be (according to how many verticies we want per unit)
        float spaceBetweenVerticiesX = worldSizeX / (float)(totalVerticiesX);
        float spaceBetweenVerticiesZ = worldSizeZ / (float)(totalVerticiesZ);
        // Debug.Log("Space between Verticies x / z:     " + spaceBetweenVerticiesX + " / " + spaceBetweenVerticiesZ);

        //the maximum lenght of a chunk along the X or Z dimension
        float chunkOffsetX = (TerrainChunk.maxSideVertexCount-1) * spaceBetweenVerticiesX;
        float chunkOffsetZ = (TerrainChunk.maxSideVertexCount-1) * spaceBetweenVerticiesZ;
        // Debug.Log("Chunk offsets x / z:     " + chunkOffsetX + " / " + chunkOffsetZ);

        int numberOfChunksX = Mathf.CeilToInt(totalVerticiesX / (float)(TerrainChunk.maxSideVertexCount - 1));
        int numberOfChunksZ = Mathf.CeilToInt(totalVerticiesZ / (float)(TerrainChunk.maxSideVertexCount - 1));
        // Debug.Log("Number of chunks x / z:     " + numberOfChunksX + " / " + numberOfChunksZ);

        noiseSampler.Reset();
        noiseSampler.OnVaulesUpdated += RegenerateMeshFromNewNoise; 

        //Label in inspector to ID each chunk
        int chunkID = 0;

        //the number of verticies for each chunk to create along the direction
        int verticiesForChunkZ;
        int verticiesForChunkX;

        for( int z = 0; z < numberOfChunksZ; z++ )
        {
                verticiesForChunkZ = ( (z != numberOfChunksZ -1) || totalVerticiesZ % (TerrainChunk.maxSideVertexCount -1) == 0 ) 
                                    ? TerrainChunk.maxSideVertexCount 
                                    : totalVerticiesZ % (TerrainChunk.maxSideVertexCount -1 ) + 1; 
            
            for (int x = 0; x < numberOfChunksX; x++ )
            {
                verticiesForChunkX = ( (x != numberOfChunksX -1) || totalVerticiesX % (TerrainChunk.maxSideVertexCount -1) == 0 ) 
                                    ? TerrainChunk.maxSideVertexCount 
                                    : totalVerticiesX % (TerrainChunk.maxSideVertexCount-1) + 1; 
                
                TerrainChunk chunk = new GameObject("Chunk " + chunkID.ToString()).AddComponent<TerrainChunk>();
                Vector3 chunkPos = new Vector3(x * chunkOffsetX, 0, z * chunkOffsetZ) + transform.position;

                chunk.Configure(verticiesForChunkX, verticiesForChunkZ, chunkPos, spaceBetweenVerticiesX, spaceBetweenVerticiesZ, noiseSampler, heightCurve, heightScale, heightMapMaterial);
                chunk.bakeCollider = bakeCollider;
                chunk.transform.SetParent(transform);
                
                chunk.CreateMesh();
                chunk.SetNoiseToHeight();

                //Debug.Log(noiseData.minNoiseHeight + "    " + noiseData.maxNoiseHeight);
                chunk.gameObject.layer = gameObject.layer;
                chunkID++;
            }
        }

        minMeshHeight = ((heightCurve.Evaluate(0) * 2 - 1) * heightScale);
        maxMeshHeight = ((heightCurve.Evaluate(1) * 2 - 1) * heightScale);
        
        foreach(TerrainChunk chunk in gameObject.GetComponentsInChildren<TerrainChunk>())
        {
            var minMax = chunk.ApplyHeight(noiseSampler.minNoiseHeight, noiseSampler.maxNoiseHeight);
            chunk.UpdateMesh();
        }

        GenerateWater();

        ApplyToMaterial(heightMapMaterial);
        

    }

    public void RegenerateMeshFromNewNoise()
    {
        foreach(Transform t in transform)
        {
            TerrainChunk chunk;
            if(t.TryGetComponent<TerrainChunk>(out chunk))
            {
                chunk.SetNoiseData(noiseSampler);
                chunk.SetNoiseToHeight();

            chunk.ApplyHeight(noiseSampler.minNoiseHeight, noiseSampler.maxNoiseHeight);
            chunk.UpdateMesh();
            }
        }

        noiseSampler.OnVaulesUpdated += RegenerateMeshFromNewNoise;
    }
    
    public void ClearChunks()
    {
        noiseSampler.OnVaulesUpdated -= RegenerateMeshFromNewNoise;
        for (int i = transform.childCount -1; i >= 0; i--)
        {

            //transform.GetChild(i).gameObject.GetComponent<TerrainChunk>().Invoke("DestroyInEditMode", .01f);
            
            DestroyImmediate( transform.GetChild(i).gameObject);
        }
    }

    void OnVaulesUpdated()
    {
        if(!Application.isPlaying)
        {
            noiseSampler.OnVaulesUpdated -= RegenerateMeshFromNewNoise;
            RegenerateMeshFromNewNoise(); 
        }
    }

    void OnValidate()
    {
        if (noiseSampler != null)
        {
            //noiseData.OnVaulesUpdated += OnVaulesUpdated;
        }
    }

    public Vector2 GetCentre2D()
    {
        Vector2 centre = new Vector2(worldSizeX / 2 + transform.position.x, worldSizeZ / 2 + transform.position.z);

        return centre;
    }

}}
