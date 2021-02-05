using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotBeweging : MonoBehaviour
{
    public GameObject wallprefab;

    public Beweging target;

    private NavMeshPath path;

    private float elapsed = 0.0f;


    void Start()
    {
        path = new NavMeshPath();
        elapsed = 0.0f;
        target = FindObjectOfType<Beweging>();
    }

    void Update()
    {
        // Update the way to the goal every second.
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;
            NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, path);
        }
        Debug.Log("Corners:" + path.corners.Length);

        Debug.DrawLine(transform.position, target.transform.position, Color.green);

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
    }
}
