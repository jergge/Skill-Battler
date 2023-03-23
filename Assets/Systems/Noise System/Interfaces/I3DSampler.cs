using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoiseSystem
{
    /// <summary>
    /// Returns the result from a 3 dimensional sample space
    /// </summary>
    public interface INoiseSampler3D : INoiseSampler
    {
        public float Sample(Vector3 input);
        public float[] Sample(Vector3[] inputs);
    }
}