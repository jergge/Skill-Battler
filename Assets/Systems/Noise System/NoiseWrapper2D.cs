using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace NoiseSystem
{
    /// <summary>
    /// A Wrapper object to track the results of multiple Sample functions -
    /// Useful if you want to normalize sample data from multiple calls.
    /// </summary>
    [CreateAssetMenu(menuName = "NoiseWrapper2D")]
    public class NoiseWrapper2D : ScriptableObject, INoiseSampler2D
    {
        [SerializeField] [RequireInterface(typeof(INoiseSampler2D))] Object _sampler;
        INoiseSampler2D sampler => _sampler as INoiseSampler2D;
        float minReturnedValue = float.MaxValue;
        float maxReturnedValue = float.MinValue;

        public float Sample(Vector2 input)
        {
            float result = sampler.Sample(input);
            UpdateMinMax(result);
            
            return result;
        }

        public float[] Sample(Vector2[] inputs)
        {
            float[] results = sampler.Sample(inputs);
            UpdateMinMax(results.Max());
            UpdateMinMax(results.Min());

            return results;
        }

        void UpdateMinMax(float result)
        {
            minReturnedValue = (result < minReturnedValue) ? result : minReturnedValue;
            maxReturnedValue = (result > maxReturnedValue) ? result : maxReturnedValue;  
        }

        public void Reset()
        {
            minReturnedValue = float.MaxValue;
            minReturnedValue = float.MinValue;
        }

        public float[] Normalize(float[] inputs)
        {
            float[] normalized = new float[inputs.Length];

            Parallel.For(0, inputs.Length, (i) =>
            {
                normalized[i] = NormalizeToMinMaxResults(inputs[i]);
            });

            return normalized;
        }

        float NormalizeToMinMaxResults(float input)
        {
            float normalized = Mathf.InverseLerp(minReturnedValue, maxReturnedValue, input);
            float scaled = Mathf.Lerp(0, 1, normalized);
            return scaled;
        }
    }
}