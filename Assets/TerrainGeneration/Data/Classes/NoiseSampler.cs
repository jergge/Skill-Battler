using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainGeneration;
using System;


public abstract class NoiseSampler : UpdateableData
{

    public int seed;
    public float scale = 1;

    protected Vector2 minMaxNoise;
    public float minNoiseHeight => minMaxNoise.x;
    public float maxNoiseHeight => minMaxNoise.y;

    public enum ReplaceComponent { x, y, z, w };

    /// <summary>
    /// Sets up any RNG objects and some other things
    /// </summary>
    public abstract void Reset();

    public abstract float Sample(float input);
    public abstract float Sample(Vector2 input);
    public abstract float Sample(Vector3 input);
    public abstract float Sample(Vector4 input);

    public virtual float[] Sample(float[] input, float offset)
    {
        float[] rets = new float[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            rets[i] = Sample(input[i] + offset);
        }
        return rets;
    }
    public virtual float[] Sample(Vector2[] input, Vector2 offset)
    {
        float[] rets = new float[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            rets[i] = Sample(input[i] + offset);
        }
        return rets;
    }
    public virtual float[] Sample(Vector3[] input, Vector3 offset)
    {
        float[] rets = new float[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            rets[i] = Sample(input[i] + offset);
        }
        return rets;
    }
    public virtual float[] Sample(Vector4[] input, Vector4 offset)
    {
        float[] rets = new float[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            rets[i] = Sample(input[i] + offset);
        }
        return rets;
    }

    public abstract Vector2[] SampleOverride(Vector2[] input, ReplaceComponent replace, Vector2 offset);
    public abstract Vector3[] SampleOverride(Vector3[] input, ReplaceComponent replace, Vector3 offset);
    public abstract Vector4[] SampleOverride(Vector4[] input, ReplaceComponent replace, Vector4 offset);


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
