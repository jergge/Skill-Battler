using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jergge.Extensions;
using UnityEngine;

namespace NoiseSystem
{
    [CreateAssetMenu(menuName = "Noise/Flat")]
    public class FlatNoise : NoiseSampler, INoiseSamplerAllDimensions
    {
        [SerializeField]
        protected float flatNoiseValue = 0f;

        public float Sample(float input)
        {
            return flatNoiseValue;
        }

        public float[] Sample(float[] inputs)
        {
            return GenerateFlatNoise(inputs.Length);
        }

        public float[] Sample(Vector2[] inputs)
        {
            return GenerateFlatNoise(inputs.Length);
        }

        public float[] Sample(Vector3[] inputs)
        {
            return GenerateFlatNoise(inputs.Length);
        }

        public float[] Sample(Vector4[] inputs)
        {
            return GenerateFlatNoise(inputs.Length);
        }

        public float Sample(Vector2 input)
        {
            return flatNoiseValue;
        }

        public float Sample(Vector3 input)
        {
            return flatNoiseValue;
        }

        public float Sample(Vector4 input)
        {
            return flatNoiseValue;
        }

        protected float[] GenerateFlatNoise(int size)
        {
            float[] result = new float[size];

            Parallel.For(0, result.Length, (i) =>
            {
                result[i] = flatNoiseValue;
            });

            return result;
        }

        public float[] Sample(float[] input, float offset)
        {
            return GenerateFlatNoise(input.Length);
        }

        public float[] Sample(Vector2[] input, Vector2 offset)
        {
            return GenerateFlatNoise(input.Length);
        }

        public float[] Sample(Vector3[] input, Vector3 offset)
        {
            return GenerateFlatNoise(input.Length);
        }

        public float[] Sample(Vector4[] input, Vector4 offset)
        {
            return GenerateFlatNoise(input.Length);
        }
    }
}