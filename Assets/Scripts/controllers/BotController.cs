
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
    Vector3 targetpos;


    float randomTime;

    Vector3 newdir;

    //GETTERS/SETTERS
    public Vector3 NewDir
	{
        get { return newdir; }
        set { newdir = value; }
    
    }

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

        checkObtrusions();

        //agent = gameObject.AddComponent<NavMeshAgent>();
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

	async void checkObtrusions()
	{
        while (GetComponent<Speler>().Alive)
        {
            

            var thisLastDir = GetComponent<Speler>().lastdir;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + thisLastDir * 2, thisLastDir, 5);
            if (hit.collider != null)
            {
                

                print($"botray heeft <color=yellow>{hit.collider.name}</color> geraakt");
                Debug.DrawRay(transform.position + thisLastDir * 2, thisLastDir, Color.red, 500f);

                if( thisLastDir.x != 0)
				{
                    print("onze orientatie is <color=pink>LINKS/RECHTS</color>");
                    NavMeshPath HypoPathUp = new NavMeshPath();
                    NavMeshPath HypoPathDown = new NavMeshPath();

                    var boolUp = NavMesh.CalculatePath(transform.position + Vector3.left, targetpos, NavMesh.AllAreas, HypoPathUp);
                    var boolDown = NavMesh.CalculatePath(transform.position + Vector3.right, targetpos, NavMesh.AllAreas, HypoPathDown);

                    if (boolUp && !boolDown)
                        NewDir = Vector3.up;

                    else if (boolDown && !boolUp)
                        NewDir = Vector3.down;

                    if (boolUp && boolDown)
                    {
                        if (HypoPathUp.corners.Length > HypoPathDown.corners.Length)
                            NewDir = Vector3.down;

                        if (HypoPathUp.corners.Length < HypoPathDown.corners.Length)
                            NewDir = Vector3.up;
                    }
                }
            
                if(thisLastDir.y != 0)
				{
                    print("onze orientatie is <color=pink>BOVEN/BENEDEN</color>");


                    NavMeshPath HypoPathLeft = new NavMeshPath();
                    NavMeshPath HypoPathRight = new NavMeshPath();

                    var boolLeft = NavMesh.CalculatePath(transform.position + Vector3.left, targetpos, NavMesh.AllAreas, HypoPathLeft);
                    var boolRight= NavMesh.CalculatePath(transform.position + Vector3.right, targetpos, NavMesh.AllAreas, HypoPathRight);

                    if (boolLeft && !boolRight)
                        NewDir = Vector3.left;

                    else if (boolRight && !boolLeft)
                        NewDir = Vector3.right;

                    if (boolLeft && boolRight)
					{
                        if(HypoPathLeft.corners.Length > HypoPathRight.corners.Length)
                            NewDir = Vector3.right;

                        if (HypoPathLeft.corners.Length < HypoPathRight.corners.Length)
                            NewDir = Vector3.left;
                    }
                }

                if (NewDir != null || NewDir != new Vector3())
                {
                    GetComponent<Speler>().directionChanger(NewDir);
                    print($"{GetComponent<Speler>().name} is moving in direction {NewDir}");
                }             
            }

            await new WaitForSeconds(0.1f);
        }
    }



	public async void findPath()
    {
       while (GetComponent<Speler>().Alive)           
       {
            targetpos = Target.position + target.GetComponent<Speler>().lastdir * 20;

           if (thisBot != null)
           {
              foundPath = NavMesh.CalculatePath(transform.position, targetpos, NavMesh.AllAreas, Path);
				if (foundPath)
				{
					if (GetComponent<Speler>().Alive)
                    {
                        moveToObjective();
                    }
				}
               await new WaitForSeconds(0.22f);
           }           
       }
    }

	

	async void moveToObjective()
    {
        hoekenLengte = Path.corners.Length;

        //TODO: DOE RAYCASTS NAAR LINKS EN NAAR RECHTS IN KLEINE AFSTAND OM TE KIJKEN OF HET EEN ZELFMOORDMISSIE IS OM DAAR HEEN TE GAAN


        if (Path.corners != null && hoekenLengte > 0)
        {

            offset = (Path.corners[currentPathIndex] - transform.position);
            offset.y = 0;

            var t = transform.position;

            var o = offset;

            var ld = gameObject.GetComponent<Speler>().lastdir;


            if (ld.x != 0)
			{
                //links/rechts

                
            }
            if (ld.y != 0)
			{
                //boven/beneden
			}

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
                if (ld != Vector3.up && ld != Vector3.down)
                {
                    thisBot.directionChanger(Vector3.up);
                    await new WaitForSeconds(RandomTime);
                }
            }



            //left&right
            else if (o.x <= o.y && Path.corners[currentPathIndex].x < t.x)
            {
                if (ld != Vector3.left && ld != Vector3.right)
                {
                    thisBot.directionChanger(Vector3.left);
                    await new WaitForSeconds(RandomTime);
                }
            }

            else if (o.x > o.y && Path.corners[currentPathIndex].x >= t.x)
            {
                if (ld != Vector3.right && ld != Vector3.left)
                {
                    thisBot.directionChanger(Vector3.right);
                    await new WaitForSeconds(RandomTime);
                }
            }            
        }
    }
}

>>>>>>> parent of a69b9f9 (Rewriting loadfinal system)
