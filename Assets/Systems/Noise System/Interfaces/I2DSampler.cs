using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoiseSystem
{
    /// <summary>
    /// Returns the result from a 2 dimensional sample space
    /// </summary>
    public interface INoiseSampler2D : INoiseSampler
    {
        public float Sample(Vector2 input);
        public float[] Sample(Vector2[] inputs);
    }
}