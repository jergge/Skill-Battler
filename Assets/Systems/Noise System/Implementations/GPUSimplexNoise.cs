using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoiseSystem
{
    [CreateAssetMenu(menuName = "Noise/GPU Simplex 2D")]
    public class GPUSimplexNoise : NoiseSampler, INoiseSampler2D
    {
        [Range(1, 5)]
        public int octaves = 1;
        [Range(0, 1)]
        public float persistence = 0;
        [Range(1, 10)]
        public float lacunarity = .2f;

        public ComputeShader Shader;

        Vector2[] octaveOffsets;
        System.Random prng;

        public float Sample(Vector2 input)
        {
            float[] output = new float[10];

            Shader.SetFloat("scale", scale);
            Shader.SetInt("octaves", octaves);
            Shader.SetFloat("persistence", persistence);
            Shader.SetFloat("lacunarity", lacunarity);
            Shader.SetVector("sampleOffset", offset);
            Shader.SetVector("samplePoint", input);

            ComputeBuffer pointBuffer = new ComputeBuffer(output.Length, sizeof(float));
            pointBuffer.SetData(output);

            Shader.SetBuffer(0, "noise", pointBuffer);
            Shader.Dispatch(0, 10, 1, 1);

            pointBuffer.GetData(output);
            pointBuffer.Dispose();

            return output[0];
        }

        public float[] Sample(Vector2[] input)
        {
            int kernalID = Shader.FindKernel("Sample2D");

            float[] output = new float[input.Length];

            Shader.SetFloat("scale", scale);
            Shader.SetInt("octaves", octaves);
            Shader.SetFloat("persistence", persistence);
            Shader.SetFloat("lacunarity", lacunarity);
            // Shader.SetVector("sampleOffset", manualOffset + offset);
            Shader.SetVector("sampleOffset", Vector2.zero + offset);

            ComputeBuffer inputBuffer = new ComputeBuffer(input.Length, sizeof(float) * 2);
            ComputeBuffer outputBuffer = new ComputeBuffer(output.Length, sizeof(float));

            inputBuffer.SetData(input);

            Shader.SetBuffer(kernalID, "inputs2", inputBuffer);
            Shader.SetBuffer(kernalID, "outputs1", outputBuffer);

            Shader.Dispatch(kernalID, input.Length / 2, 1, 1);

            outputBuffer.GetData(output);

            inputBuffer.Dispose();
            outputBuffer.Dispose();

            // for (int i = 0; i < 100; i++)
            // {
            //     Debug.Log(output[1 * 100]);
            // }
            return output;
        }
    }
}