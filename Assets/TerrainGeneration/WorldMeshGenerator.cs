using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TerrainGeneration{
public class WorldMeshGenerator : MonoBehaviour
{

    public bool generateRandomInBuild = false;

    public float worldSizeX;
    public float worldSizeZ;
    public float heightToTestHeightFrom;
    [Range(.01f, 1f)] public float editorTerrainResolution = 1f;
    public float gameTerrianResolution = 3f;

    float resolutionToUse;
    public bool bakeCollider;

    public Material heightMapMaterial;
    public bool generateWaterMesh = true;
    public Material flatWaterMaterial;

    public NoiseSampler noiseData;

    //public bool useMultipleNoiseDatas = false;
    //for the new system that itterates over each of these to get more useful terrain
    //public List<NoiseSampler> noiseDatas = new List<NoiseSampler>();
    //public IHeightMap noisesMap;

    public AnimationCurve heightCurve;
    [Range(0,100)]
    public float heightScale;

    public bool autoUpdate;

    //verticies per unity unit
    float spaceBetweenVerticies;

    float totalVerticiesX;
    float totalVerticiesZ;
    public Layer[] layers = new Layer[8];

    float maxMeshHeight;
    float minMeshHeight;

    const int textureSize = 512;
    const TextureFormat textureFormat = TextureFormat.RGB565;

    public float GetHeightAtPoint(Vector2 point)
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(new Vector3(point.x, heightToTestHeightFrom, point.y), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Walls and Floors"), QueryTriggerInteraction.UseGlobal))
        {
            return hitInfo.point.y;
        }

        throw new System.OverflowException("could not find terrain with the raycast");

        // float noise =  noiseData.Sample(new Vector2(point.x, point.z));
        // float scaledNoise = Mathf.InverseLerp(-1, 1, noise);
        // //Debug.Log(noise);
        // //Debug.Log(scaledNoise);
        // float evaluatedHeight = ((heightCurve.Evaluate(scaledNoise) * 2 -1 ) * heightScale) + transform.position.y;
        // //Debug.Log(evaluatedHeight);
        // return evaluatedHeight;
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

        //material.SetFloat("minHeight", minMeshHeight);
        float tempMin = (heightCurve.Evaluate(-1) *2 -1) * heightScale;
        material.SetFloat("minHeight", tempMin);
        //Debug.Log(tempMin);

        float tempMax = (heightCurve.Evaluate(1) *2 -1) * heightScale;
        material.SetFloat("maxHeight", tempMax);
        // material.SetFloat("maxHeight", maxMeshHeight);
        //Debug.Log(heightCurve.Evaluate(1)* heightScale);
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
        noiseData.Reset();
        GenerateNewChuks();
        //GenerateWater();
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
        
        ClearChunks();
        resolutionToUse = (Application.isPlaying) ? gameTerrianResolution : editorTerrainResolution;
        float spaceBetweenVerticies = 1f / resolutionToUse;
        // float spaceBetweenVerticies = 1f / resolutionToUse;

        int totalVerticiesX = Mathf.CeilToInt(worldSizeX * resolutionToUse);
        worldSizeX = totalVerticiesX * spaceBetweenVerticies;
        int totalVerticiesZ = Mathf.CeilToInt(worldSizeZ * resolutionToUse);

        noiseData.Reset();
        noiseData.OnVaulesUpdated += RegenerateMeshFromNewNoise; 

        int chunkID = 0;
        for( int z = 0; z <= totalVerticiesZ / TerrainChunk.maxSideVertexCount; z++ )
        {
            int zCount = ( ((z+1) * TerrainChunk.maxSideVertexCount) < totalVerticiesZ ) ? TerrainChunk.maxSideVertexCount : totalVerticiesZ % TerrainChunk.maxSideVertexCount; 
            for (int x = 0; x <= totalVerticiesX / TerrainChunk.maxSideVertexCount; x++ )
            {
                int xCount = ( ((x+1) * TerrainChunk.maxSideVertexCount) < totalVerticiesX ) ? TerrainChunk.maxSideVertexCount : totalVerticiesX % TerrainChunk.maxSideVertexCount; 
                TerrainChunk chunk = new GameObject("Chunk " + chunkID.ToString()).AddComponent<TerrainChunk>();

                Vector3 chunkPos = new Vector3(x * (TerrainChunk.maxSideVertexCount-1) / resolutionToUse, 0, z * (TerrainChunk.maxSideVertexCount-1) / resolutionToUse) + transform.position;

                chunk.Configure(xCount, zCount, chunkPos, spaceBetweenVerticies, noiseData, heightCurve, heightScale, heightMapMaterial);
                chunk.bakeCollider = bakeCollider;
                chunk.transform.SetParent(transform);
                chunk.CreateMesh();
                
                chunk.SetNoiseToHeight();

                    //Debug.Log(noiseData.minNoiseHeight + "    " + noiseData.maxNoiseHeight);
                chunk.gameObject.layer = gameObject.layer;
                chunkID++;
            }
        }

        minMeshHeight = float.MaxValue;
        maxMeshHeight = float.MinValue;

        foreach(TerrainChunk chunk in gameObject.GetComponentsInChildren<TerrainChunk>())
        {
            var minMax = chunk.ApplyHeight(noiseData.minNoiseHeight, noiseData.maxNoiseHeight);

            if (minMax.maxHeight > maxMeshHeight)
            {
                maxMeshHeight = minMax.maxHeight;
            } 
            if ( minMax.minHeight < minMeshHeight)
            {
                minMeshHeight = minMax.minHeight;
            }


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
                chunk.SetNoiseData(noiseData);
                chunk.SetNoiseToHeight();

            chunk.ApplyHeight(noiseData.minNoiseHeight, noiseData.maxNoiseHeight);
            chunk.UpdateMesh();
            }
        }

        noiseData.OnVaulesUpdated += RegenerateMeshFromNewNoise;
    }
    public void ClearChunks()
    {
        noiseData.OnVaulesUpdated -= RegenerateMeshFromNewNoise;
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
            noiseData.OnVaulesUpdated -= RegenerateMeshFromNewNoise;
            RegenerateMeshFromNewNoise(); 
        }
    }

    void OnValidate()
    {
        if (noiseData != null)
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
