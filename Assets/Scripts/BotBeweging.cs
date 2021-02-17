using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class BotBeweging : common
{
    public Beweging target;

    private NavMeshPath path;

    private float elapsed = 0.0f;




    ///Speler variables	
        //snelheid variabel
        public float speed;

        //rigidbody deze speler
        Rigidbody2D rb;

        //speler op dit moment ondoodbaar variabel
        bool Invincible;

        //laatste directie op bewogen
        Vector3 lastDirection;
    ///end speler variables
   

    ///wall variables
    //wall prefab
    public GameObject wallprefab;

    //the wall that is currently being dragged along by the player
    Collider2D wall;

    //laatste stukje muur
    Vector2 lastWallEnd;
    ///end wall variables


    //derezz sound
    private AudioClip derezz;



    void Start()
    {
        path = new NavMeshPath();
        elapsed = 0.0f;
        target = FindObjectOfType<Beweging>();

        rb = gameObject.GetComponent<Rigidbody2D>();

        speed = 16;

        //zet lastdirection naar de initieel opbewogen directie-vector
        lastDirection = initDirection();

        wallPrefab = wallprefab;

        spawnWall();
    }

   

    void spawnWall()
    {
        //sla laatste muur positie op
        lastWallEnd = transform.position;

        //spawn een nieuwe muur
        GameObject w = Instantiate(wallprefab, transform.position, Quaternion.identity);
        wall = w.GetComponent<Collider2D>();
        w.tag = "playerWall";
    }


   

    void fitColliderBetween(Collider2D co, Vector2 a, Vector2 b)
    {
        // reken het midden uit
        co.transform.position = a + (b - a) * 0.5f;

        // trek hem (horizontaal of verticaal) uit elkaar
        float dist = Vector2.Distance(a, b);
        if (a.x != b.x)
            co.transform.localScale = new Vector2(dist + 1f, 1f);
        else
            co.transform.localScale = new Vector2(1f, dist + 1f);
    }

    void Update()
    {
        fitColliderBetween(wall, lastWallEnd, transform.position);

        doInvincible();

        // Update the way to the goal every second.
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;
            NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, path);
        }
       // Debug.Log("Corners:" + path.corners.Length);

        Debug.DrawLine(transform.position, target.transform.position, Color.green);

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            
        }
    }

    void OnTriggerEnter2D(Collider2D co)
    {
        //print(co.name);
        // Not the current wall(die je achter je aan trekt)?
        if (co != wall)
        {
            //als het de tag "Powerup" draagt
            if (co.tag == "Powerup")
            {
                //vernietig de powerup
                Destroy(co.gameObject);

                //check de naam voor de type powerup
                switch (co.name)
                {
                    case "SpeedBoost":
                        speedboost();
                        break;

                    case "Invincible":
                        setInvincible();
                        break;

                    case "stopPlayer":
                        stopRandomPlayer();
                        break;

                    case "poison":
                        killPlayer();
                        break;

                    case "RemoveWalls":
                        removeWalls();
                        break;
                }
            }
            //als het niet de tag powerup draagt
            else
            {
                //en als de speler niet ondoodbaar is op dit moment
                if (!Invincible)
                {
                    //vernietig speler
                    print("Player lost: " + name);
                    StartCoroutine(deathAudio());
                }
                //als de speler wel ondoodbaar is op dit moment
                else
                {
                    if (co.tag == "wall")
                    {
                        print("Player lost: " + name);
                        StartCoroutine(deathAudio());
                    }
                }
            }
        }
    }

   
    void killPlayer()
    {
        SoundManager.Instance.Play(derezz);
        //destroys this player when hit
        Destroy(gameObject);
    }

    

    async void speedboost()
    {
        //temporarily set speed variable higher and call directionchanger so this is actually done
        //then set speed to original variable
        speed = 30f;
        lastDirection = directionChanger(lastDirection);
        await new WaitForSeconds(4);
        speed = 16f;
    }

    async void setInvincible()
    {
        //set bool Invincible to true for four seconds
        Invincible = true;
        await new WaitForSeconds(4);
        Invincible = false;
    }

    void doInvincible()
    {
        //if Invincible is true, grab all walls and set color to gray
        if (Invincible)
        {
            string wallname = wall.name;
            List<GameObject> allwalls = GameObject.FindGameObjectsWithTag("playerWall").ToList();

            foreach (GameObject wall in allwalls)
            {
                if (wall.name == wallname)
                {
                    wall.GetComponent<SpriteRenderer>().color = Color.gray;
                }
            }
        }
        //if not, if the wall color is still gray, set all walls to the original color
        else
        {
            if (wall.GetComponent<SpriteRenderer>().color == Color.gray)
            {
                string wallname = wall.name;
                List<GameObject> allwalls = GameObject.FindGameObjectsWithTag("playerWall").ToList();

                foreach (GameObject wall in allwalls)
                {
                    if (wall.name == wallname)
                    {
                        wall.GetComponent<SpriteRenderer>().color = wallprefab.GetComponent<SpriteRenderer>().color;
                    }
                }
            }
        }
    }





}
