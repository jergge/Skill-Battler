using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Jergge.Extensions;
using UnityEngine;

namespace NoiseSystem
{
    [CreateAssetMenu(menuName = "Noise/Perlin 2D")]
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
    }
}