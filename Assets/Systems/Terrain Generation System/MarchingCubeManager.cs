using System.Collections;
using System.Collections.Generic;
using Jergge.Extensions;
using NoiseSystem;
using UnityEngine;

namespace TerrainGeneration
{
    public class MarchingCubeManager : MonoBehaviour
    {
        public Vector3Int size = new Vector3Int(5, 5, 5);
        [Range(.5f, 10f)] public float spaceBetweenPoints;

        Vector3[] samplePoints;
        MarchingCube[] marchingCubes;

        public NoiseSampler noiseData;

        void Update()
        {

        }

        public void CreateSamplePoints()
        {
            samplePoints = new Vector3[size.x * size.y * size.z];

            for (int z = 0, i = 0; z < size.z; z++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int x = 0; x < size.x; x++)
                    {
                        Vector3 point = new Vector3(x, y, z) * spaceBetweenPoints + transform.position;
                        samplePoints[i] = point;
                        i++;
                    }
                }
            }
        }

        public void CreateMarchingCubes()
        {
            marchingCubes = new MarchingCube[(size.x - 1) * (size.y - 1) * (size.z - 1)];

            for (int z = 0, i = 0; z < size.z - 1; z++)
            {
                for (int y = 0; y < size.y - 1; y++)
                {
                    for (int x = 0; x < size.x - 1; x++)
                    {
                        //Debug.Log(i);
                        Vector3 point = new Vector3(x, y, z) * spaceBetweenPoints + transform.position;
                        marchingCubes[i] = new MarchingCube(point + Vector3.one * spaceBetweenPoints / 2, spaceBetweenPoints, Quaternion.identity, noiseData);
                        i++;

                    }
                }
            }
        }

        void OnDrawGizmos()
        {
            if (samplePoints.Length > 0)
            {
                for (int i = 0; i < samplePoints.Length; i++)
                {
                    Gizmos.DrawSphere(samplePoints[i], .1f);
                }
            }

            if (marchingCubes.Length > 0)
            {
                for (int i = 0; i < marchingCubes.Length; i++)
                {
                    for (int z = 0; z < marchingCubes[i].GetCornersWithNoise().Length; z++)
                    {
                        Gizmos.color = (marchingCubes[i].GetCornersWithNoise()[z].w > .5) ? Color.black : Color.white;
                        Gizmos.DrawCube(marchingCubes[i].GetCornersWithNoise()[z].ToV3(), Vector3.one / 2);

                    }
                }
            }
        }

        public void ResetData()
        {
            marchingCubes = new MarchingCube[0];
            samplePoints = new Vector3[0];
        }

    }
}

namespace TerrainGeneration
{
    [System.Serializable]
    public class MarchingCube
    {
        private const int V = 0;
        Vector3 centrePosition;
        float sideLength = 1;
        Quaternion rotation;
        NoiseSampler noiseData;
        float cutoff = .5f;

        int activeCorners = 0;

        //corner in the bottom back left is 0,0,0 (like the unity transform gizmo)

        Vector3[,,] corners = new Vector3[2, 2, 2];

        Vector3[] singleCorners = new Vector3[8];
        Vector4[] singleCornersWithNoise = new Vector4[8];

        public MarchingCube(Vector3 centrePosition, float sideLength, Quaternion rotation, NoiseSampler noiseData)
        {
            this.centrePosition = centrePosition;
            this.sideLength = sideLength;
            this.rotation = rotation;
            this.noiseData = noiseData;

            SetCorners();
        }

        void SetCorners()
        {
            //ignore rotation for now lol...
            corners[0, 0, 0] = new Vector3(-1, -1, -1) * sideLength / 2 + centrePosition;
            corners[1, 0, 0] = new Vector3(1, -1, -1) * sideLength / 2 + centrePosition;
            corners[0, 1, 0] = new Vector3(-1, 1, -1) * sideLength / 2 + centrePosition;
            corners[1, 1, 0] = new Vector3(1, 1, -1) * sideLength / 2 + centrePosition;
            corners[0, 0, 1] = new Vector3(-1, -1, 1) * sideLength / 2 + centrePosition;
            corners[1, 0, 1] = new Vector3(1, -1, 1) * sideLength / 2 + centrePosition;
            corners[0, 1, 1] = new Vector3(-1, 1, 1) * sideLength / 2 + centrePosition;
            corners[1, 1, 1] = new Vector3(1, 1, 1) * sideLength / 2 + centrePosition;

            singleCorners = SetSingleCorners();
            SetCornerNoiseValue();
            SetActiveCorners();
        }

        //Active will be the ones that are GREATER than the cutoff value (will add an option to flip it later???)
        void SetActiveCorners()
        {
            for (int i = 0, p = 1; i < singleCornersWithNoise.Length; i++)
            {
                if (singleCornersWithNoise[i].w > cutoff)
                {
                    activeCorners = activeCorners | p;
                }
                p *= 2;
            }

            Debug.Log(activeCorners);
        }

        void SetCornerNoiseValue()
        {
            for (int i = 0; i < singleCorners.Length; i++)
            {
                Vector3 corner = singleCorners[i];
                // singleCornersWithNoise[i] = new Vector4(corner.x, corner.y, corner.z, noiseData.Sample(corner));
            }
        }

        public Vector3[] SetSingleCorners()
        {
            Vector3[] result = new Vector3[8];

            result[0] = corners[0, 0, 0];
            result[1] = corners[1, 0, 0];
            result[2] = corners[0, 1, 0];
            result[3] = corners[1, 1, 0];
            result[4] = corners[0, 0, 1];
            result[5] = corners[1, 0, 1];
            result[6] = corners[0, 1, 1];
            result[7] = corners[1, 1, 1];

            //Debug.Log(result.Length);

            return result;
        }

        public Vector3[] GetCorners()
        {
            return singleCorners;
        }

        public Vector4[] GetCornersWithNoise()
        {
            return singleCornersWithNoise;
        }

    }
}