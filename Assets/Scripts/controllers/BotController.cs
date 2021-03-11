using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BotController : MonoBehaviour, IBotControllable
{
    Speler thisBot;
    Transform target;

    NavMeshPath path;
    private int currentPathIndex = 1;
<<<<<<< HEAD
    int cornersLength = 0;
=======

    Vector3 offset;

    int hoekenLengte = 0;


    Speler thisBot;

>>>>>>> parent of a44c57e (basis3)
    bool foundPath;

    Vector3 offset;

    float randomTime;


    //GETTERS/SETTERS
    public float RandomTime
	{
		get { return randomTime; }
		set { randomTime = value; }
	}

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

        setRandomTime();
    }

    void setRandomTime()
	{
        int diff = PlayerPrefs.GetInt("difficulty");

		switch (diff)
		{
            case 0:
                RandomTime = Random.Range(0.75f, 1);
                break;

            case 1:
                RandomTime = Random.Range(0.5f, 0.75f);
                break;

            case 2:
                RandomTime = Random.Range(0, 0.25f);
                break;

            default:
                RandomTime = Random.Range(0.25f, 0.5f);
                break;
		}
	}



    public async void findPath()
    {
        while (PlayerPrefs.GetInt("AlivePlayers") > 1)           
        {
            var targetpos = Target.position + target.GetComponent<Speler>().lastdir * 10;

            if (thisBot != null)
            {
<<<<<<< HEAD
                foundPath = NavMesh.CalculatePath(transform.position, targetpos, NavMesh.AllAreas, Path);
=======
                foundPath = NavMesh.CalculatePath(transform.position, Target.position, NavMesh.AllAreas, Path);
>>>>>>> parent of a44c57e (basis3)
                //print($"path: {foundPath}");
                //print(Path.corners.Length);
                await new WaitForSeconds(1);
            }           
        }
    }



<<<<<<< HEAD
    async void moveToObjective()
    {
        if (Path.status != NavMeshPathStatus.PathInvalid)
        {
            cornersLength = Path.corners.Length;

            //print(cornersLength);



            if (Path.corners != null && cornersLength > 0)
            {
                for (int i = 0; i < cornersLength - 1; i++)
                {
                    // Debug.DrawLine(Path.corners[i], Path.corners[i + 1] + target.GetComponent<Speler>().lastdir * 10, Color.red);
                    //var t = Target.transform.position + target.GetComponent<Speler>().lastdir * 10;
                    Debug.DrawLine(Path.corners[i], Path.corners[i + 1], Color.red);
                }


                offset = (Path.corners[currentPathIndex] - transform.position);
                offset.y = 0;

                var t = transform.position;

                var o = offset;

                var ld = gameObject.GetComponent<Speler>().lastdir;

                //up&down
                if (o.x > o.y && Path.corners[currentPathIndex].y < t.y)
                {
                    if (ld != Vector3.down && ld != Vector3.up)
					{
                        thisBot.directionChanger(Vector3.down);

                        await new WaitForSeconds(RandomTime);
                    }
                }

                else if (o.x <= o.y && Path.corners[currentPathIndex].y >= t.y)
                {
                    if(ld != Vector3.up && ld != Vector3.down)
					{
                        thisBot.directionChanger(Vector3.up);
                        await new WaitForSeconds(RandomTime);
                    }
                }



                //left&right
                else if (o.x <= o.y && Path.corners[currentPathIndex].x < t.x)
                {
                    if(ld != Vector3.left && ld != Vector3.right)
					{
                        thisBot.directionChanger(Vector3.left);
                        await new WaitForSeconds(RandomTime);
                    }
                }

                else if (o.x > o.y && Path.corners[currentPathIndex].x >= t.x)
                {
                    if(ld != Vector3.right && ld != Vector3.left)
					{
                        thisBot.directionChanger(Vector3.right);
                        await new WaitForSeconds(RandomTime);
                    }
                }

            }
            
        }
    }


    private void Update()
    {
        moveToObjective();       
=======

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
>>>>>>> parent of a44c57e (basis3)
    }
}
