using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetCoords();
        }
    }

    private void GetCoords()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100f))
        {
            Mesh mesh = hitInfo.transform.GetComponent<MeshFilter>().mesh;
            print(mesh.vertices[0]);
        }
    }
}
