using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TerrainGeneration;
using UnityEngine;

namespace NoiseSystem
{
    /// <summary>
    /// Contains a noise function to sample from.
    /// The results from the sampling should be stored as values from 0 to 1 (normalised)
    /// </summary>
    public abstract class NoiseSampler : UpdateableData
    {
        [SerializeField] protected int seed;
        [SerializeField] protected float scale = 1;
        [SerializeField] protected Vector2 offset;
    }
}
