using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration{

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider))]
public class TerrainChunk : MonoBehaviour
{
    public static int maxSideVertexCount = 10;
    
    public Mesh mesh;

    Vector3[] verticies;
    int[] triangles;

    public bool showVertexGizmos = false;

    float spaceBetweenVerticiesX;
    float spaceBetweenVerticiesZ;

    int numVerticiesX;
    int numVerticiesZ;

    public bool bakeCollider = true;

    float heightScale;

    NoiseSampler noiseSampler;
    Material material;
    AnimationCurve heightCurve;
    
    public void Configure(int numVerticiesX, int numVerticiesZ, Vector3 position,float spaceBetweenVerticiesX, float spaceBetweenVerticiesZ, NoiseSampler noiseSampler, AnimationCurve heightCurve, float heightScale, Material material)
    {
        this.numVerticiesX = numVerticiesX;
        this.numVerticiesZ = numVerticiesZ;

        if(numVerticiesX > maxSideVertexCount || numVerticiesZ > maxSideVertexCount)
        {
            throw new System.ArgumentOutOfRangeException("You cannot assign more verticies to a chunk that the set limit of: " + maxSideVertexCount);
        }

        this.spaceBetweenVerticiesX = spaceBetweenVerticiesX;
        this.spaceBetweenVerticiesZ = spaceBetweenVerticiesZ;

        transform.position = position;
        this.noiseSampler = noiseSampler;
        this.heightCurve = heightCurve;
        this.heightScale = heightScale;
        this.material = material;
        // StartUp();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetNoiseData(NoiseSampler sampler)
    {
        this.noiseSampler = sampler;
    }

    public void CreateMesh() {
        verticies = new Vector3[numVerticiesX * numVerticiesZ];
        triangles = new int[numVerticiesX * numVerticiesZ * 6];
        
        for (int i = 0, z = 0; z < numVerticiesZ; z++) {
            for (int x = 0; x < numVerticiesX; x++)
            {
                verticies[i] = new Vector3(x*spaceBetweenVerticiesX, 0, z*spaceBetweenVerticiesZ);
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

    public void SetNoiseToHeight()
    {
        verticies = noiseSampler.SampleOverride(verticies, NoiseSampler.ReplaceComponent.y, transform.position);
    }

    public (float minHeight, float maxHeight) ApplyHeight(float minNoise, float maxNoise)
    {
        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;
        for (int i = 0, z = 0; z < numVerticiesZ; z++) {
            for (int x = 0; x < numVerticiesX; x++)
            {
                float newHeight = ((heightCurve.Evaluate((Mathf.InverseLerp(-1, 1, verticies[i].y))) * 2 -1 ) * heightScale);
                verticies[i].y = newHeight;
                if (newHeight > maxHeight)
                {
                    maxHeight = newHeight;
                } else if (newHeight < minHeight)
                {
                    minHeight = newHeight;
                }
                i ++;
            }
        }

        return (minHeight, maxHeight);
    }

    public (float minHeight, float maxHeight) ApplyHeightGPU(float minNoise, float maxNoise)
    {
        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;

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
