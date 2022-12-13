using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GPUSimplexNoise : NoiseSampler
{
    public Vector2 perlinOffset = Vector2.zero;
    [Range(1,5)]    
    public int octaves = 1;
    [Range(0,1)]
    public float persistence = 0;
    [Range(1,10)]
    public float lacunarity = .2f;

    //public bool useGPUShader = false;
    public ComputeShader GPUShader;

    Vector2[] octaveOffsets;
    System.Random prng;

    public override void Reset()
    {
        //throw new System.NotImplementedException();
    }

    public override float Sample(float input)
    {
        throw new System.NotImplementedException();
    }

    public override float Sample(Vector2 input)
    {
        float[] output = new float[10];

        GPUShader.SetFloat("scale", scale);
        GPUShader.SetInt("octaves", octaves);
        GPUShader.SetFloat("persistence", persistence);
        GPUShader.SetFloat("lacunarity", lacunarity);
        GPUShader.SetVector("sampleOffset", Vector2.zero);
        GPUShader.SetVector("samplePoint", input);

        ComputeBuffer pointBuffer = new ComputeBuffer(output.Length, sizeof(float));
        pointBuffer.SetData(output);

        GPUShader.SetBuffer(0, "noise", pointBuffer);
        GPUShader.Dispatch(0, 10, 1, 1);

        pointBuffer.GetData(output);
        pointBuffer.Dispose();

        return output[0];
    }

    public override float[] Sample(Vector2[] input, Vector2 offset)
    {
        int kernalID = GPUShader.FindKernel("Sample");

        float[] output = new float[input.Length];

        GPUShader.SetFloat("scale", scale);
        GPUShader.SetInt("octaves", octaves);
        GPUShader.SetFloat("persistence", persistence);
        GPUShader.SetFloat("lacunarity", lacunarity);
        GPUShader.SetVector("sampleOffset", offset);

        ComputeBuffer inputBuffer = new ComputeBuffer(input.Length, sizeof(float)*2);
        ComputeBuffer outputBuffer = new ComputeBuffer(output.Length, sizeof(float));
        
        inputBuffer.SetData(input);

        GPUShader.SetBuffer(kernalID, "inputs2", inputBuffer);
        GPUShader.SetBuffer(kernalID, "outputs1", outputBuffer);

        GPUShader.Dispatch(kernalID, input.Length / 2, 1, 1);

        outputBuffer.GetData(output);

        inputBuffer.Dispose();
        outputBuffer.Dispose();

        return output;
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
        int kernalID = GPUShader.FindKernel("OverrideV3");
        
        Vector3[] output = new Vector3[input.Length];

        GPUShader.SetFloat("scale", scale);
        GPUShader.SetInt("octaves", octaves);
        GPUShader.SetFloat("persistence", persistence);
        GPUShader.SetFloat("lacunarity", lacunarity);
        GPUShader.SetVector("sampleOffset", new Vector2(offset.x, offset.z));

        ComputeBuffer inputBuffer = new ComputeBuffer(input.Length, sizeof(float) * 3);
        ComputeBuffer outputBuffer = new ComputeBuffer(output.Length, sizeof(float) * 3);

        inputBuffer.SetData(input);

        GPUShader.SetBuffer(kernalID, "inputs3", inputBuffer);
        GPUShader.SetBuffer(kernalID, "outputs3", outputBuffer);

        GPUShader.Dispatch(kernalID, Mathf.CeilToInt(input.Length / 4), 1, 1);

        outputBuffer.GetData(output);

        inputBuffer.Dispose();
        outputBuffer.Dispose();

        return output;
    }

    public override Vector4[] SampleOverride(Vector4[] input, ReplaceComponent replace, Vector4 offset)
    {
        throw new System.NotImplementedException();
    }
}
