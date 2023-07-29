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
    [CreateAssetMenu(menuName = "Noise/Wrapper/2D")]
    public class NoiseWrapper2D : NoiseWrapper, INoiseSampler2D
    {
        [SerializeField] [RequireInterface(typeof(INoiseSampler2D))] Object _sampler;
        INoiseSampler2D sampler => _sampler as INoiseSampler2D;

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
    }
}