using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavController : MonoBehaviour
{
    GameObject plane;
    GameObject[] Bots;



    private void Start()
    {
        makeSurface();

        getBots();

    }

    void makeSurface()
    {
        plane = GameObject.Find("surface");
        NavMeshSurface surface = plane.AddComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }

    void getBots()
    {

    }

}
