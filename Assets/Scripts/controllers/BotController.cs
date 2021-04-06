using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BotController : MonoBehaviour, IBotControllable
{
    Speler thisBot;
    Transform target;

    Vector3 targetpos;

    NavMeshPath path;  
    

    float randomTime;

    Vector3 ld;


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
    //END GETTER/SETTERS






    private void Start()
    {
        thisBot = GetComponent<Speler>();
        path = new NavMeshPath();
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



	

	public void findPath()
    {
		if (this.enabled)
		{
			checkObtrusions();
		}
    
	}



    async void checkObtrusions()
	{
		//haal lastdirection op
		//kijk een klein stukje voor je of je iets raakt en vertel het

		while (GetComponent<Speler>().Alive)
		{
            targetpos = Target.position + target.GetComponent<Speler>().lastdir * 10;

            ld = GetComponent<Speler>().lastdir;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + ld * 2, ld, 5);
            if (hit.collider != null)
            {
                print($"<color=yellow>{gameObject} will hit {hit.collider.name}</color>");

                //check de orientatie
                if (ld.x != 0)
                {
                    //print("onze orientatie is <color=pink>HORIZONTAAL</color>");
                    moveOutOfTheWay(Vector3.up, Vector3.down, hit);
                }
                else if (ld.y != 0)
                {
                    //print("onze orientatie is <color=pink>VERTICAAL</color>");
                    moveOutOfTheWay(Vector3.left, Vector3.right, hit);
                }
            }
            else
            {
                print("hit nothing");

                NavMesh.CalculatePath(transform.position + ld, targetpos, NavMesh.AllAreas, path);
                move();
            }

            await new WaitForSeconds(0.15f);
        }        
    }



    void moveOutOfTheWay(Vector3 sideOne, Vector3 sideTwo, RaycastHit2D hit)
	{
		

        //maak nieuwe paden aan beide kanten van de bot om te kijken welke efficienter is
        var s = GetComponent<Speler>();


        //kijk links en rechts
        RaycastHit2D hitOne = Physics2D.Raycast(transform.position + sideOne * 2, sideOne, 5);
        RaycastHit2D hitTwo = Physics2D.Raycast(transform.position + sideTwo * 2, sideTwo, 5);

        //zie je niks links en rechts
        if (!hitOne && !hitTwo)
		{
            //calculeer de paden als je naar links&naar rechts zou gaan
            NavMeshPath side1 = new NavMeshPath();
            NavMeshPath side2 = new NavMeshPath();

            var boolOne = NavMesh.CalculatePath(transform.position + sideOne, targetpos, NavMesh.AllAreas, side1);
            var boolTwo = NavMesh.CalculatePath(transform.position + sideTwo, targetpos, NavMesh.AllAreas, side2);

            //als beide paden geldig zijn
            if (boolOne && boolTwo)
			{
                //neem het pad met de minste hoeveelheid afslagen
                if (side1.corners.Length < side2.corners.Length)
                    s.directionChanger(sideOne);

                else
                    s.directionChanger(sideTwo);
			}

            //als pad 1 alleen geldig is neem pad 1
            else if (boolOne && !boolTwo)
                s.directionChanger(sideOne);

            //als pad 2 alleen geldig is neem pad 2
            else if (!boolOne && boolTwo)
                s.directionChanger(sideTwo);
        }

        //als je aan 1 kant iets ziet en de andere kant niet ga dan de kant op waar je niets ziet
        else if(!hitOne && hitTwo)
            s.directionChanger(sideOne);

        else if(hitOne && !hitTwo)
            s.directionChanger(sideTwo);

    }





    void move()
	{
        if(path.corners != null && path.corners.Length > 0)
		{
            Vector3 afstandTussenBotEnVolgendeCorner = path.corners[1] - transform.position;

            Vector3 t = transform.position;
            Vector3 o = afstandTussenBotEnVolgendeCorner;

            //als je laatste kant niet niks is (dus is geinitieerd)
            if(ld != null && ld != new Vector3())
			{
                //horizontaal
                if (ld.x != 0)
                {                   
                    //als de afstand tussen de bot en de eerstvolgende afslag op de x as groter is dan die op de afstand bot-afslag op de y as
                    //EN 
                    //als de locatiewaarde van de afslag op de y as kleiner is mijn locatiewaarde op de y as
                    if (o.x > o.y && path.corners[1].y < t.y)
                    {
                        
                        thisBot.directionChanger(Vector3.down);
                    }

                    else if (o.x <= o.y && path.corners[1].y > t.y)
                    {
                        thisBot.directionChanger(Vector3.up);
                    }
                }

                //verticaal 
                else if (ld.y != 0)
                {    
                    if (o.x < o.y && path.corners[1].x < t.x)
                        thisBot.directionChanger(Vector3.left);

                    else if (o.x > o.y && path.corners[1].x > t.x)
                        thisBot.directionChanger(Vector3.right);
                }
            }
		}        
    }
}

