using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Jergge.Extensions;
using UnityEngine;

namespace NoiseSystem
{
    [CreateAssetMenu(menuName = "Terrian System/Noise/Perlin")]
    public class PerlinNoise2D : NoiseSampler, INoiseSampler2D
    {

        public Vector2 perlinOffset = Vector2.zero;
        [Range(1, 5)]
        public int octaves;
        [Range(0, 1)]
        public float persistence;
        [Range(1, 10)]
        public float lacunarity;

        Vector2[] octaveOffsets;
        System.Random prng;

        public float Sample(float input)
        {
            return Sample(new Vector2(input, 1));
        }

        public float Sample(Vector2 input)
        {
            float amplitude = 1f;
            float frequency = 1f;
            float noiseHeight = 0;

            for (int i = 0; i < octaves; i++)
            {
                float sampleX = (input.x + perlinOffset.x) / scale * frequency + octaveOffsets[i].x;
                float sampleY = (input.y + perlinOffset.y) / scale * frequency + octaveOffsets[i].y;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);

                noiseHeight += perlinValue * amplitude;

                amplitude *= persistence;
                frequency *= lacunarity;
            }
            TrackMinMax(noiseHeight);
            return noiseHeight;
        }

        public float[] Sample(Vector2[] inputs)
        {
            float[] results = new float[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                results[i] = Sample(inputs[i]);
            }

            return results;
        }

        public override Vector2[] SampleOverride(Vector2[] input, ReplaceComponent replace, Vector2 offset)
        {
            if (replace == ReplaceComponent.w || replace == ReplaceComponent.z)
            {
                throw new System.InvalidOperationException("Components Z and W do not exist in Vector2");
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }

        public override Vector3[] SampleOverride(Vector3[] input, ReplaceComponent replace, Vector3 offset)
        {
            if (replace == ReplaceComponent.w)
            {
                throw new System.InvalidOperationException("Component 'w' does not exist in Vector3");
            }
            else
            {
                switch (replace)
                {
                    case ReplaceComponent.x:
                        for (int i = 0; i < input.Length; i++)
                            input[i] = new Vector3(Sample(input[i].YZ() + offset.YZ()), input[i].y, input[i].z);
                        break;

                    case ReplaceComponent.y:
                        for (int i = 0; i < input.Length; i++)
                            input[i] = new Vector3(input[i].x, Sample(input[i].XZ() + offset.XZ()), input[i].z);
                        break;

                    case ReplaceComponent.z:
                        for (int i = 0; i < input.Length; i++)
                            input[i] = new Vector3(input[i].x, input[i].y, Sample(input[i].YZ() + offset.YZ()));
                        break;
                }

                return input;
            }
        }

        public override Vector4[] SampleOverride(Vector4[] input, ReplaceComponent replace, Vector4 offset)
        {
            throw new System.NotImplementedException();
        }

        public override void Reset()
        {
            prng = new System.Random(seed);
            octaveOffsets = new Vector2[octaves];

            for (int i = 0; i < octaves; i++)
            {
                octaveOffsets[i] = new Vector2(prng.Next(-1000, 1000), prng.Next(-1000, 1000));
            }
            ClearMinMax();
        }

    }
}