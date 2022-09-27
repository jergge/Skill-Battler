using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainGeneration;


public abstract class NoiseData : UpdateableData
{

    public int seed;
    public float scale = 1;

    protected Vector2 minMaxNoise;
    public float minNoiseHeight => minMaxNoise.x;
    public float maxNoiseHeight => minMaxNoise.y;

    public ComputeShader noiseGPUShader;

    public abstract float GetNoiseAtPoint(Vector3 input);

    public abstract float GetNoiseAtPointGPU(Vector3 input);

    public Vector3[] EditNoiseAtPoints(Vector3[] input)
    {
        Vector3[] ret = new Vector3[input.Length];

        for(int i=0; i<input.Length; i++)
        {
            ret[i] = new Vector3(input[i].x, GetNoiseAtPoint(input[i]), input[i].z);
        }

        return ret;
    }

    public Vector3[] EditNoiseAtPoints(Vector3[] input, Vector3 sampleOffset)
    {
        Vector3[] ret = new Vector3[input.Length];

        for(int i=0; i<input.Length; i++)
        {
            ret[i] = new Vector3(input[i].x, GetNoiseAtPoint(input[i]+sampleOffset), input[i].z);
        }

        return ret; 
    }

    public abstract Vector3[] EditNoiseAtPointsGPU(Vector3[] input, Vector3 sampleOffset);

    public (Vector3[] points, float min, float max) ApplyHeightAtPointGPU(Vector3[] input, float minNoiseHeight, float maxNoiseHeight)
    {
        Vector3[] points = new Vector3[input.Length];
        float[] min = new float[]{float.MaxValue};
        float[] max = new float[]{float.MinValue};

        ComputeBuffer heightBuffer = new ComputeBuffer(input.Length, sizeof(float)*3);
        heightBuffer.SetData(input);

        ComputeBuffer minBuffer = new ComputeBuffer(1, sizeof(float));
        minBuffer.SetData(min);
        ComputeBuffer maxBuffer = new ComputeBuffer(1, sizeof(float));
        maxBuffer.SetData(max);

        noiseGPUShader.SetFloat("minNoise", float.MaxValue);
        noiseGPUShader.SetFloat("maxNoise", float.MinValue);

        noiseGPUShader.SetBuffer(1, "vertexss", heightBuffer);
        noiseGPUShader.Dispatch(1, input.Length/2, 1, 1);

        heightBuffer.GetData(points);
        heightBuffer.Dispose();

        float[] minOut = new float[1];
        float[] maxOut = new float[1];

        minBuffer.GetData(minOut);
        maxBuffer.GetData(maxOut);

        return (points, minOut[0], maxOut[0]);
    }

    public abstract void Configure();

    protected void ClearMinMax()
    {
        minMaxNoise = new Vector2(float.MaxValue, float.MinValue);
    }

    protected void TrackMinMax(float noiseHeight)
    {
        minMaxNoise.x = (noiseHeight<minMaxNoise.x) ? noiseHeight : minMaxNoise.x;
        minMaxNoise.y = (noiseHeight>minMaxNoise.y) ? noiseHeight : minMaxNoise.y;
    }
}
