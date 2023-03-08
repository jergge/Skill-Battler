using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TerrainGeneration;
using UnityEngine;

/// <summary>
/// Contains a noise function to sample from.
/// The results from the sampling should be stored as values from -1 to 1
/// </summary>
public abstract class NoiseSampler : UpdateableData
{

    public int seed;
    public float scale = 1;
    public Vector2 offset;

    protected Vector2 minMaxNoise;
    public float minNoiseHeight => minMaxNoise.x;
    public float maxNoiseHeight => minMaxNoise.y;

    public enum ReplaceComponent { x, y, z, w };

    /// <summary>
    /// Stores the last result that was called from Sample with an array input
    /// </summary>
    public float[] noiseResult { get; protected set; }

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
        noiseResult = rets;
        return rets;
    }

    public virtual float[] Sample(Vector2[] input, Vector2 offset)
    {
        float[] rets = new float[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            rets[i] = Sample(input[i] + offset);
        }
        noiseResult = rets;
        return rets;
    }

    public virtual float[,] Sample(Vector2[,] input, Vector2 offset)
    {
        float[,] rets = new float[input.GetLength(0), input.GetLength(1)];
        for (int x = 0; x < input.GetLength(0); x++)
        {
            for (int z = 0; z < input.GetLength(1); z++)
            {
                rets[x, z] = Sample(input[x, z] + offset);
            }
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
        noiseResult = rets;
        return rets;
    }

    public virtual float[] Sample(Vector4[] input, Vector4 offset)
    {
        float[] rets = new float[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            rets[i] = Sample(input[i] + offset);
        }
        noiseResult = rets;
        return rets;
    }

    [System.Obsolete("This is being removed. Samplers will no longer be responsible for the combination in this way")]
    public abstract Vector2[] SampleOverride(Vector2[] input, ReplaceComponent replace, Vector2 offset);
    [System.Obsolete("This is being removed. Samplers will no longer be responsible for the combination in this way")]
    public abstract Vector3[] SampleOverride(Vector3[] input, ReplaceComponent replace, Vector3 offset);
    [System.Obsolete("This is being removed. Samplers will no longer be responsible for the combination in this way")]
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

    protected void FindMinMax(float[] array)
    {
        TrackMinMax(array.Max());
        TrackMinMax(array.Min());
        //Debug.Log("Min of sample: " + minMaxNoise.x + "    Max of sample: " + minMaxNoise.y);
    }

    public float[] RescaleNoise(float[] noise, float lower, float upper)
    {
        float[] newNoise = new float[noise.Length];

        Parallel.For(0, noise.Length, x =>
            {
                float normalized = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noise[x]);
                float scaled = Mathf.Lerp(lower, upper, normalized);
                newNoise[x] = scaled;
            });

        return newNoise;
    }
}
