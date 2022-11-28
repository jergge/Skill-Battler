using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jergge.Extensions;

[CreateAssetMenu()]
public class FlatNoise : NoiseSampler
{
    public float flatNoiseValue =0f;

    public override void Reset()
    {
       
    }

    public override float Sample(float input)
    {
        return flatNoiseValue;
    }

    public override float Sample(Vector2 input)
    {
        return flatNoiseValue;
    }

    public override float Sample(Vector3 input)
    {
        return flatNoiseValue;
    }

    public override float Sample(Vector4 input)
    {
        return flatNoiseValue;
    }

    public override Vector2[] SampleOverride(Vector2[] input, ReplaceComponent replace, Vector2 offset)
    {
        throw new System.NotImplementedException();
    }

    public override Vector3[] SampleOverride(Vector3[] input, ReplaceComponent replace, Vector3 offset)
    {
        Vector2[] ret = new Vector2[input.Length];
        switch(replace)
            {
                case ReplaceComponent.x:
                    for (int i = 0; i < input.Length; i++)
                        input[i] = new Vector3(Sample(input[i].YZ() + offset.YZ()), input[i].y, input[i].z);
                    break;

                case ReplaceComponent.y:
                    for (int i = 0; i < input.Length; i++)
                        input[i] = new Vector3(input[i].x, Sample(input[i].XZ() + offset.XZ()), input[i].z);
                    break;

                case ReplaceComponent.z:
                    for (int i = 0; i < input.Length; i++)
                        input[i] = new Vector3(input[i].x, input[i].y, Sample(input[i].YZ() + offset.YZ()));
                    break; 

                default:
                throw new System.AccessViolationException("soething went wrong here");
        }

            return input;
    }

    public override Vector4[] SampleOverride(Vector4[] input, ReplaceComponent replace, Vector4 offset)
    {
        throw new System.NotImplementedException();
    }
}
