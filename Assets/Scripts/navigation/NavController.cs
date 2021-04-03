using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavController : MonoBehaviour
{
    GameObject plane;
    BotController[] Bots;
    Transform Target;
    NavMeshSurface surface;


    private void Start()
    {
        makeSurface();

        Bots = FindObjectsOfType<BotController>();

        Target = FindObjectOfType<SpelerController>().GetComponent<Transform>();

        WaitForSurface();
    }

    async void WaitForSurface()
    {
        while(surface.size.magnitude <= 0.0f)
        {
            await new WaitForEndOfFrame();
        }

        foreach (BotController bot in Bots)
        {
            bot.Target = Target;
            bot.findPath();
        }
    }

    void makeSurface()
    {
        print(plane);
        surface = plane.AddComponent<NavMeshSurface>();
        surface.BuildNavMesh()
            print(surface.BuildNavMesh());
    }
}
