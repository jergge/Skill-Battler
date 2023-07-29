using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NoiseSystem
{
    /// <summary>
    /// A Wrapper object to track the results of multiple Sample functions -
    /// Useful if you want to normalize sample data from multiple calls.
    /// </summary>
    public abstract class NoiseWrapper : ScriptableObject, INoiseSampler
    {
        float minReturnedValue = float.MaxValue;
        float maxReturnedValue = float.MinValue;
        
        protected void UpdateMinMax(float result)
        {
            // Debug.Log(result);
            minReturnedValue = (result < minReturnedValue) ? result : minReturnedValue;
            maxReturnedValue = (result > maxReturnedValue) ? result : maxReturnedValue;  
        }
        
        public void Reset()
        {
            minReturnedValue = float.MaxValue;
            maxReturnedValue = float.MinValue;
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

        public (float lower, float upper) GetNoiseBounds()
        {
            return (minReturnedValue, maxReturnedValue);
        }

        float NormalizeToMinMaxResults(float input)
        {
            float normalized = Mathf.InverseLerp(minReturnedValue, maxReturnedValue, input);
            float scaled = Mathf.Lerp(0, 1, normalized);
            return scaled;
        }
    }


}