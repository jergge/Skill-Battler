using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkillSystem{
public class FireWall : Skill, IActiveSkill
{
    public Material material;
    public float height;
    public float length;
    [Range(4,1000)]public int numPoints;
    float maxRayCastDist = 300;
    public LayerMask collisionLayer;

    void Update()
    {
        TickCooldown();
    }

    public void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        // GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        // wall.transform.position = source.transform.position;
        // wall.GetComponent<Renderer>().material = material;
        GenerateWall(targetInfo.direction);
    }

    void GenerateWall(Vector3 dir)
    {
        Vector3[] verticies = new Vector3[numPoints*2];
            // Debug.Log(verticies.Length + " verticies");
        int numTriangels = (numPoints-1)*2;
            // Debug.Log(numTriangels + " triangles");
        int[] triangles = new int[numTriangels*3];
            // Debug.Log(triangles.Length + " corners of triangles");
        Vector2[] uvs = new Vector2[verticies.Length];


        for (int i = 0; i < verticies.Length; i=i+2)
        {
            float pointsPercent = i/((float)numPoints-1);
            float step = length * pointsPercent;

            Physics.queriesHitBackfaces = true;

            //move the lower one, up first
            Vector3 ori = transform.position + dir * step;
            Vector3 castDir = Vector3.up;
            RaycastHit hitInfo;
            if (Physics.Raycast(ori, castDir, out hitInfo, maxRayCastDist, collisionLayer, QueryTriggerInteraction.UseGlobal))
            {
                //Debug.Log("casting up and hit " + hitInfo.point);
                verticies[i] = hitInfo.point - source.transform.position;
            } else if(Physics.Raycast(ori, -castDir, out hitInfo, maxRayCastDist, collisionLayer, QueryTriggerInteraction.UseGlobal))
            {
                //Debug.Log("casting down and hit " + hitInfo.point);
                verticies[i] = hitInfo.point - source.transform.position;
            }
            verticies[i+1] = verticies[i] + Vector3.up * height;
            uvs[i] = new Vector2(pointsPercent, 0);
            uvs[i+1] = new Vector2(pointsPercent, 1);
            //Debug.Log("Vertex " + i + " is at " + verticies[i]);
            //Debug.Log("Vertex " + (i+1) + " is at " + verticies[i+1]);
        }

        Physics.queriesHitBackfaces = false;
        for (int i = 0; i < numPoints -1; i++)
        {
            int triangleIndex = i*6;
            int vertexIndex = i*2;

            triangles[triangleIndex]    = vertexIndex; 
            triangles[triangleIndex+1]  = vertexIndex+1;
            triangles[triangleIndex+2]  = vertexIndex+2;

            triangles[triangleIndex+3]  = vertexIndex+1;
            triangles[triangleIndex+4]  = vertexIndex+3;
            triangles[triangleIndex+5]  = vertexIndex+2;
        }




        GameObject wall = new GameObject();
        wall.transform.position = source.transform.position;
        MeshRenderer renderer = wall.AddComponent<MeshRenderer>();
        MeshFilter filter =  wall.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();
        filter.mesh = mesh;

        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.SetUVs(0, uvs);
        mesh.RecalculateNormals();

        renderer.sharedMaterial = material;
    }
}}
