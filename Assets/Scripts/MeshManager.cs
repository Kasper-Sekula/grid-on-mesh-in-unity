using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager : MonoBehaviour
{
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    [SerializeField] private Material groundMaterial;
    [SerializeField] private Material subMaterial;

    [SerializeField] private bool createSingleMesh = true;

    private bool hasBeenGenerated = false;

    private Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;

    private void Update()
    {
        if (createSingleMesh)
        {
            GenerateInitialMesh();
            GenerateVertices();
            GenerateTriangles();

            UpdateMesh();
        }
        else
        {
            GenerateVerticesForSubMeshes();
        }

    }

    // SINGLE MESH

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

        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    // MULTIPLE SUB-MESHES

    private void GenerateVerticesForSubMeshes()
    {

        for (int z = 0; z <= gridHeight; z++)
        {
            for (int x = 0; x <= gridWidth; x++)
            { 
                Vector3[] vertex = new Vector3[4];
                vertex[0] = new Vector3(x, 0 , z);
                vertex[1] = new Vector3(x + 1, 0, z);
                vertex[2] = new Vector3(x, 0, z);
                vertex[3] = new Vector3(x + 1, 0, z +1);
                GenerateSingleTile(vertex);

            }
        }
        hasBeenGenerated = true;
    }

    private void CreateAllSubMeshes()
    {
        mesh.subMeshCount = gridHeight * gridWidth;

        int[] subMeshTriangles = new int[6];

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

    private Mesh GenerateSingleSubMesh()
    {
        Mesh subMesh = new Mesh();

        Vector3[] subVerts = new Vector3[4];
        int[] subTriangles = new int[6];

        subVerts[0] = new Vector3(0, 0, 0);
        subVerts[1] = new Vector3(0, 0, -1);
        subVerts[2] = new Vector3(-1, 0, -1);
        subVerts[3] = new Vector3(-1, 0, 0);

        subTriangles[0] = 0;
        subTriangles[1] = 1;
        subTriangles[2] = 2;
        subTriangles[3] = 2;
        subTriangles[4] = 3;
        subTriangles[5] = 0;

        subMesh.Clear();
        subMesh.vertices = subVerts;
        subMesh.triangles = subTriangles;

        return subMesh;
    }

    private void GenerateSingleTile(Vector3[] tileVerts)
    {
        if (hasBeenGenerated)
            return;

        GameObject newTile = new GameObject();

        newTile.transform.position = tileVerts[0];
        newTile.transform.name = $"{tileVerts[0].x} : {tileVerts[0].z}";

        newTile.transform.parent = gameObject.transform;
        newTile.AddComponent<MeshFilter>();
        newTile.AddComponent<MeshRenderer>();
        Mesh tileMesh = new Mesh();
        newTile.GetComponent<MeshFilter>().mesh = tileMesh;
        tileMesh.vertices = tileVerts;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Transform child = gameObject.transform.GetChild(i);
                Gizmos.DrawSphere(child.transform.position , .1f);
            }
            return;
        }

        for (int i=0; i< vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
