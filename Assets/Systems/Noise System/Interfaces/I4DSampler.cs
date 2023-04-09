using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoiseSystem
{
    /// <summary>
    /// Returns the result from a 4 dimensional sample space
    /// </summary>
    public interface INoiseSampler4D : INoiseSampler
    {
        public float Sample(Vector4 input);
        public float[] Sample(Vector4[] inputs);
    }
}