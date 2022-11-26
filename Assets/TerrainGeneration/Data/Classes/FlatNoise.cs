using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FlatNoise : NoiseSampler
{
    public float flatNoiseValue =0f;

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }

    public override float Sample(float input)
    {
        throw new System.NotImplementedException();
    }

    public override float Sample(Vector2 input)
    {
        throw new System.NotImplementedException();
    }

    public override float Sample(Vector3 input)
    {
        throw new System.NotImplementedException();
    }

    public override float Sample(Vector4 input)
    {
        throw new System.NotImplementedException();
    }

    public override Vector2[] SampleOverride(Vector2[] input, ReplaceComponent replace, Vector2 offset)
    {
        throw new System.NotImplementedException();
    }

    public override Vector3[] SampleOverride(Vector3[] input, ReplaceComponent replace, Vector3 offset)
    {
        throw new System.NotImplementedException();
    }

    public override Vector4[] SampleOverride(Vector4[] input, ReplaceComponent replace, Vector4 offset)
    {
        throw new System.NotImplementedException();
    }
}
