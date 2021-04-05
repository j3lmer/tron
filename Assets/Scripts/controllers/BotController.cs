using UnityEngine;
using UnityEngine.AI;


public class BotController : MonoBehaviour, IBotControllable
{
    Speler thisBot;
    Transform target;

    NavMeshPath path;
    private int currentPathIndex = 1;
    Vector3 offset;
    int hoekenLengte = 0;
    bool foundPath;


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

    NavMeshAgent agent;

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
        print(diff);

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
              foundPath = NavMesh.CalculatePath(transform.position, targetpos, NavMesh.AllAreas, Path);
               print($"FOUND PATH: {foundPath}");
				if (foundPath)
				{
                   moveToObjective();
				}
               //print($"path: {foundPath}");
               print(Path.corners.Length);
               await new WaitForSeconds(0.22f);
           }           
       }
    }
	async void moveToObjective()
    {
        hoekenLengte = Path.corners.Length;


        if (Path.corners != null && hoekenLengte > 0)
        {

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



