using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TerrainGeneration{

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider))]
public class TerrainChunk : MonoBehaviour
{
    public static int maxSideVertexCount = 255;

    MinMaxTracker tracker;
    public void SetTracker(MinMaxTracker tracker)
    {
        this.tracker = tracker;
    }

    Vector3[,] verticies2DArray;
    Vector2[,] verticies2DArrayXZ;
    float[,] noise2DArray;

    //Things for the mesh
    public Mesh mesh;
    Vector3[] verticies;
    int[] triangles;
        //Things for the height (noise and calculations)
        Vector2[] verticiesXZ;
        float[] noise;

    public bool showVertexGizmos = false;
    bool useHeightCurve = true;

    float spaceBetweenVerticiesX;
    float spaceBetweenVerticiesZ;

    int numVerticiesX;
    int numVerticiesZ;

    public bool bakeCollider = true;

    float heightScale;

    NoiseSampler noiseSampler;
    Material material;
    AnimationCurve heightCurve;
    
    public void Configure(int numVerticiesX, int numVerticiesZ, Vector3 position,float spaceBetweenVerticiesX, float spaceBetweenVerticiesZ, NoiseSampler noiseSampler, AnimationCurve heightCurve, float heightScale, Material material, bool useHeightCurve)
    {
        if(numVerticiesX > maxSideVertexCount || numVerticiesZ > maxSideVertexCount)
        {
            throw new System.ArgumentOutOfRangeException("You cannot assign more verticies to a chunk than the set limit of: " + maxSideVertexCount);
        }

        this.numVerticiesX = numVerticiesX;
        this.numVerticiesZ = numVerticiesZ;

        this.spaceBetweenVerticiesX = spaceBetweenVerticiesX;
        this.spaceBetweenVerticiesZ = spaceBetweenVerticiesZ;

        transform.position = position;
        this.noiseSampler = noiseSampler;
        this.heightCurve = heightCurve;
        this.heightScale = heightScale;
        this.material = material;
        this.useHeightCurve = useHeightCurve;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetNoiseSampler(NoiseSampler sampler)
    {
        this.noiseSampler = sampler;
    }

    public void CreateMesh() {
        verticies = new Vector3[numVerticiesX * numVerticiesZ];
        //verticies2DArray = new Vector3[numVerticiesX, numVerticiesZ];
        //verticies2DArrayXZ = new Vector2[numVerticiesX, numVerticiesZ];
        verticiesXZ = new Vector2[verticies.Length];
        triangles = new int[numVerticiesX * numVerticiesZ * 6];
        
        for (int i = 0, z = 0; z < numVerticiesZ; z++) {
            for (int x = 0; x < numVerticiesX; x++)
            {
                Vector3 newCoord = new Vector3(x*spaceBetweenVerticiesX, 0, z*spaceBetweenVerticiesZ);
                verticies[i] = newCoord;
                //verticies2DArray[x, z] = newCoord;

                Vector2 new2Coord = new Vector2(x * spaceBetweenVerticiesX, z * spaceBetweenVerticiesZ);
                verticiesXZ[i] = new2Coord;
                //verticies2DArrayXZ[x, z] = new2Coord;
                i ++;
            }
        }
        
        int vert = 0, tri = 0;
        for (int z = 0; z < numVerticiesZ-1; z++)
        {
            for (int x = 0; x < numVerticiesX-1; x++)
            {
                triangles[tri + 0] = vert + 0;
                triangles[tri + 1] = vert + 0 + numVerticiesX;
                triangles[tri + 2] = vert + 1;
                triangles[tri + 3] = vert + 1;
                triangles[tri + 4] = vert + 0 + numVerticiesX;
                triangles[tri + 5] = vert + 1 + numVerticiesX;

                vert++;
                tri += 6;
            }
            vert++;
        }
    }

    [System.Obsolete("Seperate the noise creation from the noise application")]
    public void SetNoiseToHeight()
    {
        verticies = noiseSampler.SampleOverride(verticies, NoiseSampler.ReplaceComponent.y, transform.position);
    }

    public void GenerateNoise()
    {
        noise = noiseSampler.Sample(verticiesXZ, new Vector2(transform.position.x, transform.position.z));
        //noise2DArray = noiseSampler.Sample(verticies2DArrayXZ, new Vector2(transform.position.z, transform.position.z));
    }

    public void RescaleNoise(float lower, float upper)
    {
        noise = noiseSampler.RescaleNoise(noise, lower, upper);
    }

    public (float minHeight, float maxHeight) ApplyHeight(float minNoise, float maxNoise)
    {
        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;
        for (int i = 0; i < verticies.Length; i++) 
        {
            //float newHeight = ((heightCurve.Evaluate((Mathf.InverseLerp(-1, 1, verticies[i].y))) * 2 -1 ) * heightScale);
            float newHeight = (useHeightCurve) ? ((heightCurve.Evaluate((Mathf.InverseLerp(-1, 1, noise[i]))) * 2 -1 ) * heightScale) : noise[i] * heightScale;
            verticies[i].y = newHeight;

            if (newHeight > maxHeight)
            {
                maxHeight = newHeight;
            } else if (newHeight < minHeight)
            {
                minHeight = newHeight;
            }
        }

        return (minHeight, maxHeight);
    }

    public void UpdateMesh() {
        mesh.Clear();

        mesh.vertices = verticies;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        if(bakeCollider)
        {
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }
        GetComponent<MeshRenderer>().sharedMaterial = material;
    }
    
    void OnDrawGizmos()
    {
        if (verticies == null || showVertexGizmos == false)
        {
            return;
        }

        for(int i = 0; i < verticies.Length; i++)
        {
            Gizmos.DrawSphere(transform.position + verticies[i], .1f);
        }
    }

    public void DestroyInEditMode()
    {
        DestroyImmediate(gameObject);
    }
}}
