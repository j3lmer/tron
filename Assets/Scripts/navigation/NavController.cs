using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavController : MonoBehaviour
{
    GameObject plane;
    BotController[] Bots;
    Transform Target;


    private void Start()
    {
        makeSurface();

        Bots = FindObjectsOfType<BotController>();

        Target = FindObjectOfType<SpelerController>().GetComponent<Transform>();

       
        foreach (BotController bot in Bots)
        {
            bot.Target = Target;
            bot.findPath();
        }
    }

    void makeSurface()
    {
        plane = GameObject.Find("surface");
        NavMeshSurface surface = plane.AddComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }
}
