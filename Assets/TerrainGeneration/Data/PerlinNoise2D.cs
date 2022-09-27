using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PerlinNoise2D : NoiseData
{
    
    public Vector2 perlinOffset;
    [Range(1,5)]    
    public int octaves;
    [Range(0,1)]
    public float persistence;
    [Range(1,10)]
    public float lacunarity;

    Vector2 offset = Vector3.zero;
    Vector2[] octaveOffsets;
    System.Random prng;

    

    public override void Configure()
    {
        prng = new System.Random(seed);
        octaveOffsets = new Vector2[octaves];

        for (int i =0; i < octaves; i++)
        {
            octaveOffsets[i] = new Vector3(prng.Next(-1000, 1000), prng.Next(-1000, 1000), prng.Next(-1000, 1000));
        }
        ClearMinMax();
    }

    public override float GetNoiseAtPoint(Vector3 input)
    {
        float amplitude = 1f;
        float frequency = 1f;
        float noiseHeight = 0;

        for (int i = 0; i < octaves; i++)
        {
            float sampleX = (input.x + perlinOffset.x) / scale * frequency + octaveOffsets[i].x;
            float sampleZ = (input.z + perlinOffset.y) / scale * frequency + octaveOffsets[i].y;
            //float sampleZ = (input.z + offset.z) / scale * frequency + octaveOffsets[i].z;

            //Debug.Log(sampleX + "   " + sampleZ);

            float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ);
        
            //Debug.Log(perlinValue);
            noiseHeight += perlinValue * amplitude;

            amplitude *= persistence;
            frequency *= lacunarity;
        }

        //Debug.Log(noiseHeight * heightScale);
        TrackMinMax(noiseHeight);

        return noiseHeight;

    }

    public override float GetNoiseAtPointGPU(Vector3 input)
    {
        float[] output = new float[10];

        noiseGPUShader.SetFloat("scale", scale);
        noiseGPUShader.SetInt("octaves", octaves);
        noiseGPUShader.SetFloat("persistence", persistence);
        noiseGPUShader.SetFloat("lacunarity", lacunarity);
        noiseGPUShader.SetVector("sampleOffset", Vector3.zero);
        noiseGPUShader.SetVector("samplePoint", input);

        ComputeBuffer pointBuffer = new ComputeBuffer(output.Length, sizeof(float));
        pointBuffer.SetData(output);

        noiseGPUShader.SetBuffer(2, "noise", pointBuffer);
        noiseGPUShader.Dispatch(2, 10, 1, 1);

        pointBuffer.GetData(output);
        pointBuffer.Dispose();

        return output[0];
    }

    public override Vector3[] EditNoiseAtPointsGPU(Vector3[] input, Vector3 sampleOffset)
    {
        Vector3[] output = new Vector3[input.Length];

        noiseGPUShader.SetFloat("scale", scale);
        noiseGPUShader.SetInt("octaves", octaves);
        noiseGPUShader.SetFloat("persistence", persistence);
        noiseGPUShader.SetFloat("lacunarity", lacunarity);
        noiseGPUShader.SetVector("sampleOffset", sampleOffset);

        ComputeBuffer vertexBuffer = new ComputeBuffer(input.Length, sizeof(float)*3);
        vertexBuffer.SetData(input);

        noiseGPUShader.SetBuffer(0, "vertexs", vertexBuffer);
        noiseGPUShader.Dispatch(0, input.Length/2, 1, 1);

        vertexBuffer.GetData(output);
        vertexBuffer.Dispose();

        return output;
    }


}
