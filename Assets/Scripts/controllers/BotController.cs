using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BotController : MonoBehaviour, IBotControllable
{
    Speler thisBot;
    Transform target;
    Speler targetPlayer;

    Vector3 targetpos;

    NavMeshPath path;

    bool checkActive;

    float randomTime;

    Vector3 ld;

    Vector3 dirRight;
    Vector3 dirLeft;
    RaycastHit2D hitLeft;
    RaycastHit2D hitRight;


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
        targetPlayer = Target.GetComponent<Speler>();

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

        while (thisBot.Alive)
        {
            targetpos = Target.position + targetPlayer.lastdir * 10;
            ld = thisBot.lastdir;


            RaycastHit2D hitMid = Physics2D.Raycast(transform.position + ld * 2, ld, 2);

            if (hitMid.collider != null)
            {
                var a = 45 * Mathf.Deg2Rad;
                var o = transform.up;

                if (ld.y != 0)
                {
                    o = transform.right;
                }
                dirRight = (ld * Mathf.Cos(a) + o * Mathf.Sin(a)).normalized;
                dirLeft = (ld * Mathf.Cos(a) - o * Mathf.Sin(a)).normalized;

                hitLeft = Physics2D.Raycast(transform.position + ld * 2, dirLeft, 2);
                hitRight = Physics2D.Raycast(transform.position + ld * 2, dirRight, 2);

                var hm = hitMid.collider;
                var hr = hitRight.collider;
                var hl = hitLeft.collider;

                print($"{hm.name} geraakt");


                if (hm != null && hr == null && hl == null || hm != null && hr != null && hl != null)
                {
                    print($"voor {thisBot.name} {hm.name} geraakt");
                    //check de orientatie
                    if (ld.x != 0)
                    {
                        //print("onze orientatie is <color=pink>HORIZONTAAL</color>");
                        moveOutOfTheWay(Vector3.up, Vector3.down, "HORIZONTAL");
                    }
                    else if (ld.y != 0)
                    {
                        //print("onze orientatie is <color=pink>VERTICAAL</color>");
                        moveOutOfTheWay(Vector3.left, Vector3.right, "VERTICAL");
                    }
                }

                else if (hm != null && hr != null && hl == null)
                {
                    print($"voor {thisBot.name} {hm.name} geraakt en schuin rechts {hr.name} geraakt");
                    thisBot.directionChanger(Vector3.left);
                }

                else if (hm != null && hr == null && hl != null)
                {
                    print($"voor {thisBot.name} {hm.name} geraakt en schuin links {hl.name} geraakt");
                    thisBot.directionChanger(Vector3.right);
                }



            }
            else
            {
                print("hit nothing");

                if (ld.x != 0)
                {
                    ////print("onze orientatie is <color=pink>HORIZONTAAL</color>");
                    checkLeftAndRight(Vector3.up, Vector3.down);
                }
                else if (ld.y != 0)
                {
                    ////print("onze orientatie is <color=pink>VERTICAAL</color>");
                    checkLeftAndRight(Vector3.left, Vector3.right);
                }
            }

            await new WaitForSeconds(0.15f);
        }

    }

    async void checkLeftAndRight(Vector3 kant1, Vector3 kant2)
    {

        RaycastHit2D hitOne = Physics2D.Raycast(transform.position + kant1 * 2, kant1, 2);
        RaycastHit2D hitTwo = Physics2D.Raycast(transform.position + kant2 * 2, kant2, 2);

        if (!hitOne && !hitTwo)
        {
            //soms zijn de 2 normale raycasts net wat te weinig info voor de bot om te zien, dus heb ik er 2 extra toegevoegd die iets achter hem zijn
            hitLeft = Physics2D.Raycast(transform.position + ld * 2, dirLeft, 2);
            hitRight = Physics2D.Raycast(transform.position + ld * 2, dirRight, 2);

            if (!hitLeft && !hitRight)
            {
                //print("Ik zie links en rechts niks van me, ik ga door als normaal");
                NavMesh.CalculatePath(transform.position + ld, targetpos, NavMesh.AllAreas, path);
                move();
                print("moving");
            }
        }
        while (hitOne || hitTwo)
        {
            await new WaitUntil(() => !hitOne && !hitTwo);
            NavMesh.CalculatePath(transform.position + ld, targetpos, NavMesh.AllAreas, path);
            move();
            print("moving again");
        }
    }



    void moveOutOfTheWay(Vector3 sideOne, Vector3 sideTwo, string orientation)
    {
        //print("Ik ga aan de kant");
        //maak nieuwe paden aan beide kanten van de bot om te kijken welke efficienter is
        var s = thisBot;

        //kijk links en rechts
        RaycastHit2D hitOne = Physics2D.Raycast(transform.position + sideOne * 2, sideOne, 2);
        RaycastHit2D hitTwo = Physics2D.Raycast(transform.position + sideTwo * 2, sideTwo, 2);

        //zie je niks links en rechts
        if (!hitOne && !hitTwo)
        {

            print("ik zie niks links en rechts van me");
            //calculeer de paden als je naar links&naar rechts zou gaan
            NavMeshPath side1 = new NavMeshPath();
            NavMeshPath side2 = new NavMeshPath();

            var boolOne = NavMesh.CalculatePath(transform.position + sideOne, targetpos, NavMesh.AllAreas, side1);
            var boolTwo = NavMesh.CalculatePath(transform.position + sideTwo, targetpos, NavMesh.AllAreas, side2);

            //als beide paden geldig zijn
            if (boolOne && boolTwo)
            {
                //haal nieuwe target positie op voor de zekerheid
                var tp = Target.position;
                var tbp = thisBot.transform.position;

                switch (orientation)
                {
                    case "HORIZONTAL":
                        if (tp.y > tbp.y)
                            thisBot.directionChanger(Vector3.up);
                        else if (tp.y < tbp.y)
                            thisBot.directionChanger(Vector3.down);
                        else
                        {
                            if (side1.corners.Length < side2.corners.Length)
                                s.directionChanger(sideOne);
                            else
                                s.directionChanger(sideTwo);
                        }

                        break;

                    case "VERTICAL":
                        if (tp.x > tbp.x)
                            thisBot.directionChanger(Vector3.right);
                        else if (tp.x < tbp.x)
                            thisBot.directionChanger(Vector3.left);
                        else
                        {
                            if (side1.corners.Length < side2.corners.Length)
                                s.directionChanger(sideOne);
                            else
                                s.directionChanger(sideTwo);                            
                        }
                        break;
                }
            }

            //als pad 1 alleen geldig is neem pad 1
            else if (boolOne && !boolTwo)
            {
                s.directionChanger(sideOne);
                print("alleen pad 1 is geldig");
            }

            //als pad 2 alleen geldig is neem pad 2
            else if (!boolOne && boolTwo)
            {
                s.directionChanger(sideTwo);
                print("alleen pad 2 is geldig");
            }
        }

        //als je aan 1 kant iets ziet en de andere kant niet ga dan de kant op waar je niets ziet
        else if (!hitOne && hitTwo)
        {

            s.directionChanger(sideOne);
            print("ik zie alleen aan kant 1 niks");
        }

        else if (hitOne && !hitTwo)
        {
            s.directionChanger(sideTwo);
            print("ik zie alleen aan kant 2 niks");
        }
    }



    void move()
    {
        if (path.corners != null && path.corners.Length > 0)
        {
            Vector3 afstandTussenBotEnVolgendeCorner = path.corners[1] - transform.position;

            Vector3 t = transform.position;
            Vector3 o = afstandTussenBotEnVolgendeCorner;

            //als je laatste kant niet niks is (dus is geinitieerd)
            if (ld != null && ld != new Vector3())
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

                    else if (o.x < o.y && path.corners[1].y > t.y)
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

