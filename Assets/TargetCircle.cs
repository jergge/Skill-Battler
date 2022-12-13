using System.Collections;
using System.Collections.Generic;
using TerrainGeneration;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TargetCircle : ITerrainGenerator
{
    public float radius;
    public float maxRange;
    [Range(3, 100)]
    public int pointsOnCircle = 20;
    public float startingHeightForRayDown = 200f;

    public GameObject source;
    public Material material;

    public LayerMask terrainLayer;

    List<Vector3> hitPoints = new List<Vector3>();

    Vector3[] verticies;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        verticies = GenerateVerticies();
        triangles = GenerateTriangles();
        GenerateMesh();
    }

    Vector3[] GenerateVerticies()
    {
        Vector3[] ver = new Vector3[1+pointsOnCircle];
        ver[0] = Vector3.zero;

        hitPoints.Clear();

        float angle = 0;

        for (int i = 1; i< ver.Length; i++)
        {
            angle = (2 * Mathf.PI)/pointsOnCircle * i;

            float xCord = Mathf.Sin(angle) * radius;
            float zCord = Mathf.Cos(angle) * radius;
            float yCord = 0;

            RaycastHit hit;
            if(Physics.Raycast(new Vector3(xCord, startingHeightForRayDown, zCord), Vector3.down, out hit, 2000, terrainLayer))
            {
                yCord = hit.point.y;
                Debug.Log(hit.point);
                hitPoints.Add(hit.point);
            }

            ver[i] = new Vector3(xCord, yCord-transform.position.y, zCord);
        }

        return ver;
    }

    int[] GenerateTriangles()
    {
        int[] tri = new int[pointsOnCircle*3];
        for ( int i = 0; i < pointsOnCircle; i++)
        {
            //Start at Centre
            tri[i*3]      = 0;
            tri[i*3+1]    = i;
            tri[i*3+2]    = (i+1);
        }

        return tri;

    }

    void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(verticies);
        mesh.SetTriangles(triangles, 0);

        gameObject.GetComponent<MeshRenderer>().material = material;
        gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void GenerateAll()
    {
        verticies = GenerateVerticies();
        triangles = GenerateTriangles();
        GenerateMesh();
    }

    void OnDrawGizmos()
    {
        foreach ( Vector3 v in verticies)
        {
           // Gizmos.DrawCube(v, Vector3.one);
        }

        foreach (Vector3 v in hitPoints)
        {
            Gizmos.DrawCube(v, Vector3.one);
        }
    }
}
