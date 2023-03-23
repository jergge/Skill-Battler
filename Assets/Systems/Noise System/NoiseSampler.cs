using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TerrainGeneration;
using UnityEngine;

namespace NoiseSystem
{
    /// <summary>
    /// Contains a noise function to sample from.
    /// The results from the sampling should be stored as values from 0 to 1 (normalised)
    /// </summary>
    public abstract class NoiseSampler : UpdateableData
    {

        [SerializeField] protected int seed;
        [SerializeField] protected float scale = 1;
        [SerializeField] protected Vector2 offset;

        protected Vector2 minMaxNoise;
        public float minNoiseHeight => minMaxNoise.x;
        public float maxNoiseHeight => minMaxNoise.y;

        [Obsolete]
        public enum ReplaceComponent { x, y, z, w };

        /// <summary>
        /// Stores the last result that was called from Sample with an array input
        /// </summary>
        public float[] noiseResult { get; protected set; }

        /// <summary>
        /// Sets up any RNG objects and some other things
        /// </summary>
        public abstract void Reset();

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
            minMaxNoise.x = (noiseHeight < minMaxNoise.x) ? noiseHeight : minMaxNoise.x;
            minMaxNoise.y = (noiseHeight > minMaxNoise.y) ? noiseHeight : minMaxNoise.y;
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
}
