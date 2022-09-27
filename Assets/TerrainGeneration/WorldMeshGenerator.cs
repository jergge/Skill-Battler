using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TerrainGeneration{
public class WorldMeshGenerator : MonoBehaviour
{



    public int worldSizeX;
    public int worldSizeZ;
    [Range(.01f, 1f)] public float editorTerrainResolution = 1f;
    public float gameTerrianResolution = 3f;

    float resolutionToUse;
    public bool bakeCollider;
    public bool useShader = false; 

    public Material mat;
    public string layerToAdd;

    //public bool usePerlinNoiseMap = false;

    public NoiseData noiseData;
    //public IHeightMap noisesMap;

    public AnimationCurve heightCurve;
    [Range(0,100)]
    public float heightScale;

    public bool autoUpdate;

    //verticies per unity unit
    float spaceBetweenVerticies;

    int totalVerticiesX;
    int totalVerticiesZ;
    public Layer[] layers = new Layer[8];

    float maxMeshHeight;
    float minMeshHeight;

    const int textureSize = 512;
    const TextureFormat textureFormat = TextureFormat.RGB565;

    public float GetHeightAtPoint(Vector3 point)
    {
        float noise =  noiseData.GetNoiseAtPointGPU(point);
        float scaledNoise = Mathf.InverseLerp(-1, 1, noise);
        //Debug.Log(noise);
       //Debug.Log(scaledNoise);
        float evaluatedHeight = ((heightCurve.Evaluate(scaledNoise) * 2 -1 ) * heightScale) + transform.position.y;
        //Debug.Log(evaluatedHeight);
        return evaluatedHeight;
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
        noiseData.Configure();
        GenerateNewChuks();
    }

    public void GenerateNewChuks()
    {
        
        ClearChunks();
        resolutionToUse = (Application.isPlaying) ? gameTerrianResolution : editorTerrainResolution;
        float spaceBetweenVerticies = 1f / resolutionToUse;

        int totalVerticiesX = Mathf.RoundToInt(worldSizeX * resolutionToUse);
        int totalVerticiesZ = Mathf.RoundToInt(worldSizeZ * resolutionToUse);

        noiseData.Configure();
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

                chunk.Configure(xCount, zCount, chunkPos, spaceBetweenVerticies, noiseData, heightCurve, heightScale, mat);
                chunk.bakeCollider = bakeCollider;
                chunk.transform.SetParent(transform);
                chunk.CreateMesh();
                if (useShader)
                {
                    chunk.ApplyNoiseGPU();
                } else {
                    chunk.ApplyNoise();
                }
                    //Debug.Log(noiseData.minNoiseHeight + "    " + noiseData.maxNoiseHeight);
                chunk.gameObject.layer = gameObject.layer;
                chunkID++;
            }
        }

        minMeshHeight = float.MaxValue;
        maxMeshHeight = float.MinValue;

        foreach(TerrainChunk chunk in gameObject.GetComponentsInChildren<TerrainChunk>())
        {
            var minMax =    (useShader)
                            ? chunk.ApplyHeight(noiseData.minNoiseHeight, noiseData.maxNoiseHeight)
                            : chunk.ApplyHeight(noiseData.minNoiseHeight, noiseData.maxNoiseHeight);

            if (minMax.maxHeight > maxMeshHeight)
            {
                maxMeshHeight = minMax.maxHeight;
            } else if ( minMax.minHeight < minMeshHeight)
            {
                minMeshHeight = minMax.minHeight;
            }


            chunk.UpdateMesh();
        }

        ApplyToMaterial(mat);
        

    }
    public void RegenerateMeshFromNewNoise()
    {
        foreach(Transform t in transform)
        {
            TerrainChunk chunk;
            if(t.TryGetComponent<TerrainChunk>(out chunk))
            {
                chunk.SetNoiseData(noiseData);
                if (useShader)
                {
                    chunk.ApplyNoiseGPU();
                } else {
                    chunk.ApplyNoise();
                }
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

    void OnBlock()
    {
        GetHeightAtPoint(new Vector3(1000, 0, 1000));
    }

}}
