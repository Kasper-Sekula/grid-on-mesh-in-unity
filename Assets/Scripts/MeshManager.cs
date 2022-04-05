using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager : MonoBehaviour
{
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    [SerializeField] private Material groundMaterial;

    private Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;

    private void Update()
    {
        GenerateInitialMesh();
        GenerateVertices();
        GenerateTriangles();

        UpdateMesh();
    }

    private void GenerateInitialMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void GenerateVertices()
    {
        vertices = new Vector3[(gridWidth + 1) * (gridHeight + 1)];

        for (int i=0, z = 0; z <= gridHeight; z++)
        {
            for (int x = 0; x <= gridWidth; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }
    }

    private void GenerateTriangles()
    {
        triangles = new int[gridWidth * gridHeight * 6];

        int vert = 0; 
        int tris = 0;
        for (int z = 0; z < gridHeight; z++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + gridWidth + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + gridWidth + 1;
                triangles[tris + 4] = vert + gridWidth + 2;
                triangles[tris + 5] = vert + 1;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        gameObject.GetComponent<MeshRenderer>().material = groundMaterial;

        //int[] testTriangles = new int[6];
        //print(mesh.subMeshCount);
        //mesh.SetTriangles(triangles, 6);
        mesh.subMeshCount = 2;
        print(mesh.subMeshCount);
        print(mesh.GetSubMesh(1));
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i=0; i< vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
