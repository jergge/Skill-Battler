using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoiseSystem
{
    /// <summary>
    /// Returns the result from a 1 dimensional sample space
    /// </summary>
    public interface INoiseSampler1D : INoiseSampler
    {
        public float Sample(float input);
        public float[] Sample(float[] inputs);
    }
}