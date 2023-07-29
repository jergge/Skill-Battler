using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NoiseSystem;
using UnityEngine;

namespace TerrainGeneration{

    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider))]
    public class TerrainChunk : MonoBehaviour
    {
        public static int maxSideVertexCount = 250;

        //Vector3[,] verticies2DArray;
        //Vector2[,] verticies2DArrayXZ;
        //float[,] noise2DArray;

        //Things for the mesh
        public Mesh mesh;

        Vector3[] verticies;
        /// <summary>
        /// a 2D (x and z) array of verticies in world space used for the sampling function
        /// </summary>
        Vector2[] verticiesForSample;
        List<Vector2> uvs = new List<Vector2>();

        int[] triangles;
        //Things for the height (noise and calculations)
        //Vector2[] verticiesXZ;

        float[] noise;
        float[] normalizedNoise;

        public bool showVertexGizmos = false;
        bool useHeightCurve = true;
        public bool lowestHieghtAtZero = true;

        float spaceBetweenVerticiesX;
        float spaceBetweenVerticiesZ;

        int numVerticiesX;
        int numVerticiesZ;

        public bool bakeCollider = true;

        float heightScale;

        // NoiseSampler noiseSampler;
        NoiseWrapper2D noiseWrapper2D;
        Material material;
        public void SetMaterial(Material material)
        {
            this.material = material;
        }
        AnimationCurve heightCurve;

        public void Configure(int numVerticiesX, int numVerticiesZ, Vector3 position, float spaceBetweenVerticiesX, float spaceBetweenVerticiesZ, NoiseWrapper2D noiseWrapper2D, AnimationCurve heightCurve, float heightScale, Material material, bool useHeightCurve)
        {
            if (numVerticiesX > maxSideVertexCount || numVerticiesZ > maxSideVertexCount)
            {
                throw new System.ArgumentOutOfRangeException("You cannot assign more verticies to a chunk than the set limit of: " + maxSideVertexCount);
            }

            this.numVerticiesX = numVerticiesX;
            this.numVerticiesZ = numVerticiesZ;

            this.spaceBetweenVerticiesX = spaceBetweenVerticiesX;
            this.spaceBetweenVerticiesZ = spaceBetweenVerticiesZ;

            transform.position = position;
            this.noiseWrapper2D = noiseWrapper2D;
            this.heightCurve = heightCurve;
            this.heightScale = heightScale;
            this.material = material;
            this.useHeightCurve = useHeightCurve;

            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        [System.Obsolete("Set this in the Configure method instead")]
        public void SetNoiseSampler(NoiseWrapper2D sampler)
        {
            this.noiseWrapper2D = sampler;
        }

        public void CreateMeshData()
        {
            verticies = new Vector3[numVerticiesX * numVerticiesZ];
            verticiesForSample = new Vector2[verticies.Length];
            //verticies2DArray = new Vector3[numVerticiesX, numVerticiesZ];
            //verticies2DArrayXZ = new Vector2[numVerticiesX, numVerticiesZ];
            //verticiesXZ = new Vector2[verticies.Length];
            triangles = new int[numVerticiesX * numVerticiesZ * 6];

            for (int i = 0, z = 0; z < numVerticiesZ; z++)
            {
                for (int x = 0; x < numVerticiesX; x++)
                {
                    Vector3 meshCoord = new Vector3(x * spaceBetweenVerticiesX, 0, z * spaceBetweenVerticiesZ);
                    verticies[i] = meshCoord;

                    Vector2 sampleCoord = new Vector2(x * spaceBetweenVerticiesX + transform.position.x, z * spaceBetweenVerticiesZ + transform.position.z);
                    verticiesForSample[i] = sampleCoord;

                    Vector2 uv = new Vector2(Mathf.InverseLerp(0, numVerticiesX, x), Mathf.InverseLerp(0, numVerticiesZ, z));
                    uvs.Add(uv);
                    i++;
                }
            }

            int vert = 0, tri = 0;
            for (int z = 0; z < numVerticiesZ - 1; z++)
            {
                for (int x = 0; x < numVerticiesX - 1; x++)
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
        public void SetHeightAsNoiseValues()
        {
            //verticies = noiseSampler.SampleOverride(verticies, NoiseSampler.ReplaceComponent.y, transform.position);
            Parallel.For(0, noise.Length, (x) =>
            {
                verticies[x].y = noise[x];
            });
        }

        public void GenerateNoise()
        {
            noise = noiseWrapper2D.Sample(verticiesForSample);
            //noise2DArray = noiseSampler.Sample(verticies2DArrayXZ, new Vector2(transform.position.z, transform.position.z));
        }

    public void RescaleNoise()
    {
        // noise = noiseSampler.RescaleNoise(noise, lower, upper);
        normalizedNoise = noiseWrapper2D.Normalize(noise);
    }

    public void ApplyHeight()
    {
        // float minHeight = float.MaxValue;
        // float maxHeight = float.MinValue;
        for (int i = 0; i < verticies.Length; i++) 
        {
                //float newHeight = ((heightCurve.Evaluate((Mathf.InverseLerp(-1, 1, verticies[i].y))) * 2 -1 ) * heightScale);
                float newHeight;
                if (useHeightCurve)
                {
                    newHeight = ((heightCurve.Evaluate(noise[i])) * heightScale);
                } else 
                {
                    newHeight = noise[i] * heightScale;
                }
                verticies[i].y = newHeight;

            // if (newHeight > maxHeight)
            // {
            //     maxHeight = newHeight;
            // } else if (newHeight < minHeight)
            // {
            //     minHeight = newHeight;
            // }
        }

        // return (minHeight, maxHeight);
    }

    public void UpdateMesh() {
        mesh.Clear();

        mesh.vertices = verticies;
        mesh.triangles = triangles;

        mesh.SetUVs(0, uvs);

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
