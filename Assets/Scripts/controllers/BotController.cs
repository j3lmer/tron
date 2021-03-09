using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BotController : MonoBehaviour, IBotControllable
{
    Transform target;
    NavMeshPath path;

    private int currentPathIndex = 1;

    Vector3 offset;

    int hoekenLengte = 0;


    Speler thisBot;

    bool foundPath;



    //GETTERS/SETTERS
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }
    NavMeshPath Path
    {
        get { return path; }
        set { path = value; }
    }
    //END GETTER/SETTERS

    private void Start()
    {
        thisBot = GetComponent<Speler>();
        Path = new NavMeshPath();
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }



    public async void findPath()
    {
        while (true)
        {
            if(thisBot != null)
            {
              ;
                foundPath = NavMesh.CalculatePath(transform.position, Target.position, NavMesh.AllAreas, Path);
                //print($"path: {foundPath}");
                //print(Path.corners.Length);
                await new WaitForSeconds(1);
            }           
        }
    }




    private void Update()
    {

        if (Path.status != NavMeshPathStatus.PathPartial)
        {
            hoekenLengte = Path.corners.Length;

            for (int i = 0; i < Path.corners.Length - 1; i++)
            {
                Debug.DrawLine(Path.corners[i], Path.corners[i + 1], Color.red);
            }

            if (Path.corners != null && Path.corners.Length > 0)
            {
                offset = (Path.corners[currentPathIndex] - transform.position);
                offset.y = 0;

                var t = transform.position;

                var o = offset;

                //up&down
                if (o.x > o.y && Path.corners[currentPathIndex].y < t.y)
                {
                    thisBot.directionChanger(Vector3.down);
                    //yield return new WaitForSeconds(randomTime);
                }

                else if (o.x <= o.y && Path.corners[currentPathIndex].y >= t.y)
                {
                    thisBot.directionChanger(Vector3.up);
                    //yield return new WaitForSeconds(randomTime);
                }

                //left&right
                else if (o.x <= o.y && Path.corners[currentPathIndex].x < t.x)
                {
                    thisBot.directionChanger(Vector3.left);
                    //yield return new WaitForSeconds(randomTime);
                }

                else if (o.x > o.y && Path.corners[currentPathIndex].x >= t.x)
                {
                    thisBot.directionChanger(Vector3.right);
                    //yield return new WaitForSeconds(randomTime);
                }

                //randomTime = Random.Range(0, 0.75f);
                Debug.DrawLine(transform.position, transform.position + offset, Color.green);
            }
        }
    }
}
