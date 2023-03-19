using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TerrainGeneration
{
[CreateAssetMenu(menuName = "Terrian System/Noise/Chain")]
    public class NoiseSamplerChain : NoiseSampler
    {
        public Vector2 noiseBounds;
        public List<NoiseSamplerLink> chain = new List<NoiseSamplerLink>();


        public override void Reset()
        {
            foreach (var link in chain)
            {
                if (link is not null)
                {
                    link.sampler.Reset();
                }
            }
            if (noiseResult is not null)
            {
                System.Array.Clear(noiseResult, 0, noiseResult.Length);

            }
            ClearMinMax();
        }

        public override float Sample(float input)
        {
            float noise = 0f;

            foreach (var link in chain)
            {
                switch (link.opperation)
                {
                    case NoiseSamplerLink.OpperationToPreviousLink.add:
                        noise += link.sampler.Sample(input);
                        break;

                    case NoiseSamplerLink.OpperationToPreviousLink.over:
                        noise = link.sampler.Sample(input);
                        break;

                    case NoiseSamplerLink.OpperationToPreviousLink.multi:
                        noise *= link.sampler.Sample(input);
                        break;

                    default:
                        break;
                }
            }

            return noise;
        }

        public override float Sample(Vector2 input)
        {
            float noise = 0f;

            foreach (var link in chain)
            {
                switch (link.opperation)
                {
                    case NoiseSamplerLink.OpperationToPreviousLink.add:
                        noise += link.sampler.Sample(input);
                        break;

                    case NoiseSamplerLink.OpperationToPreviousLink.over:
                        noise = link.sampler.Sample(input);
                        break;

                    case NoiseSamplerLink.OpperationToPreviousLink.multi:
                        noise *= link.sampler.Sample(input);
                        break;

                    default:
                        break;
                }
            }

            return noise;
        }

        public override float Sample(Vector3 input)
        {
            float noise = 0f;

            foreach (var link in chain)
            {
                switch (link.opperation)
                {
                    case NoiseSamplerLink.OpperationToPreviousLink.add:
                        noise += link.sampler.Sample(input);
                        break;

                    case NoiseSamplerLink.OpperationToPreviousLink.over:
                        noise = link.sampler.Sample(input);
                        break;

                    case NoiseSamplerLink.OpperationToPreviousLink.multi:
                        noise *= link.sampler.Sample(input);
                        break;

                    default:
                        break;
                }
            }

            return noise;
        }

        public override float Sample(Vector4 input)
        {
            float noise = 0f;

            foreach (var link in chain)
            {
                switch (link.opperation)
                {
                    case NoiseSamplerLink.OpperationToPreviousLink.add:
                        noise += link.sampler.Sample(input);
                        break;

                    case NoiseSamplerLink.OpperationToPreviousLink.over:
                        noise = link.sampler.Sample(input);
                        break;

                    case NoiseSamplerLink.OpperationToPreviousLink.multi:
                        noise *= link.sampler.Sample(input);
                        break;

                    default:
                        break;
                }
            }

            return noise;
        }

        public override float[] Sample(Vector2[] input, Vector2 offset)
        {
            float[] noise = new float[input.Length];
            float[] combinedNoise = new float[input.Length];

            noise = chain[0].sampler.Sample(input, offset);
            

            for (int i = 1; i < chain.Count; i++)
            {
                float[] newNoise = new float[input.Length];
                switch (chain[i].opperation)
                {
                    case NoiseSamplerLink.OpperationToPreviousLink.add:
                        newNoise = chain[i].sampler.Sample(input, offset);
                        noise = noise.Zip(newNoise, (x, y) => x + y).ToArray<float>();
                        break;

                    case NoiseSamplerLink.OpperationToPreviousLink.over:
                        noise = chain[i].sampler.Sample(input, offset);
                        break;

                    case NoiseSamplerLink.OpperationToPreviousLink.multi:
                        newNoise = chain[i].sampler.Sample(input, offset);
                        noise = noise.Zip(newNoise, (x, y) => x * y).ToArray<float>();
                        break;

                    default:
                        break;
                }
                
            }
            noiseResult = noise;
            FindMinMax(noise);
            return noise;
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

    [System.Serializable]
    public class NoiseSamplerLink
    {
        [ExposedScriptableObject]
        public NoiseSampler sampler;

        public enum OpperationToPreviousLink { add, over, multi, none };

        public OpperationToPreviousLink opperation;
    }

}
