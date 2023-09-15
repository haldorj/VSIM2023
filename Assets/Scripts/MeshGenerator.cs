using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    public Vector3[] vertices;
    public int[] triangles;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }
    
    public float GetSurfaceHeight(Vector3 p)
    {
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p0 = vertices[triangles[i]];
            Vector3 p1 = vertices[triangles[i + 1]];
            Vector3 p2 = vertices[triangles[i + 2]];
            
            Vector3 baryCoords = BarycentricCoordinates(p0, p1, p2, p);

            // Check if the player's position is inside the triangle.
            if (baryCoords is { x: >= 0.0f, y: >= 0.0f, z: >= 0.0f })
            {
                // The player's position is inside the triangle.
                // Calculate the height of the surface at the player's position.
                float height = baryCoords.x * p0.z + baryCoords.y * p1.z + baryCoords.z * p2.z;

                // Return the height as the height of the surface at the player's position.
                return height;
            }
        }
        return 0.0f;
    }
    public static Vector3 BarycentricCoordinates(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 pt)
    {
        Vector2 p12 = p2 - p1;
        Vector2 p13 = p3 - p1;
        Vector3 n = Vector3.Cross(new Vector3(p12.x, 0.0f, p12.y), new Vector3(p13.x, 0.0f, p13.y));
        float areal123 = n.magnitude;
        Vector3 baryc = default;
        // u
        Vector2 p = p2 - pt;
        Vector2 q = p3 - pt;
        n = Vector3.Cross(new Vector3(p.x, 0.0f, p.y), new Vector3(q.x, 0.0f, q.y));
        baryc.x = n.y / areal123;
        // v
        p = p3 - pt;
        q = p1 - pt;
        n = Vector3.Cross(new Vector3(p.x, 0.0f, p.y), new Vector3(q.x, 0.0f, q.y));
        baryc.y = n.y / areal123;
        // w
        p = p1 - pt;
        q = p2 - pt;
        n = Vector3.Cross(new Vector3(p.x, 0.0f, p.y), new Vector3(q.x, 0.0f, q.y));
        baryc.z = n.y / areal123;

        return baryc;
    }

    void CreateShape()
    {
        vertices = new Vector3[]
        {
            new Vector3(0.0f, 0.097f, 0.0f),
            new Vector3(0.4f, 0.005f, 0.0f),
            new Vector3(0.0f, 0.005f, 0.4f),
            new Vector3(0.4f, 0.075f, 0.4f),
            new Vector3(0.8f, 0.007f, 0.4f),
            new Vector3(0.8f, 0.039f, 0.0f)
        };

        triangles = new int[]
        {
            2, 1, 0
            // 2, 3, 1,
            // 4, 1, 3,
            // 1, 4, 5
        };
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}