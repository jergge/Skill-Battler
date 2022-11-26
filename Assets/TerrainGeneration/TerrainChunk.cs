using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration{

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider))]
public class TerrainChunk : MonoBehaviour
{
    public static int maxSideVertexCount = 255;
    
    public Mesh mesh;

    Vector3[] verticies;
    int[] triangles;

    public bool showVertexGizmos = false;

    public float spaceBetweenVerticies = 1;

    public int xSize = 50;
    public int zSize = 50;

    public bool bakeCollider = true;

    float heightScale;

    NoiseSampler pNoise;
    Material material;
    AnimationCurve heightCurve;

    int vertexCount => (xSize +1) * (zSize +1);
    
    public void Configure(int xSize, int zSize, Vector3 position,float spaceBetweenVerticies, NoiseSampler pNoise, AnimationCurve heightCurve, float heightScale, Material mat)
    {
        this.xSize = xSize;
        this.zSize = zSize;
        this.spaceBetweenVerticies = spaceBetweenVerticies;
        transform.position = position;
        this.pNoise = pNoise;
        this.heightCurve = heightCurve;
        this.heightScale = heightScale;
        this.material = mat;
        // StartUp();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetNoiseData(NoiseSampler data)
    {
        pNoise = data;
    }

    public void CreateMesh() {
        verticies = new Vector3[vertexCount];
        triangles = new int[xSize * zSize * 6];
        
        for (int i = 0, z = 0; z < zSize; z++) {
            for (int x = 0; x < xSize; x++)
            {
                verticies[i] = new Vector3(x*spaceBetweenVerticies, 0, z*spaceBetweenVerticies);
                i ++;
            }
        }
        
        int vert = 0, tri = 0;
        for (int z = 0; z < zSize-1; z++)
        {
            for (int x = 0; x < xSize-1; x++)
            {
                triangles[tri + 0] = vert + 0;
                triangles[tri + 1] = vert + 0 + xSize;
                triangles[tri + 2] = vert + 1;
                triangles[tri + 3] = vert + 1;
                triangles[tri + 4] = vert + 0 + xSize;
                triangles[tri + 5] = vert + 1 + xSize;

                vert++;
                tri += 6;
            }
            vert++;
        }
    }

    public void SetNoiseToHeight()
    {
        verticies = pNoise.SampleOverride(verticies, NoiseSampler.ReplaceComponent.y, transform.position);
    }

    public (float minHeight, float maxHeight) ApplyHeight(float minNoise, float maxNoise)
    {
        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;
        for (int i = 0, z = 0; z < zSize; z++) {
            for (int x = 0; x < xSize; x++)
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
