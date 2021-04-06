using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class Speler : MonoBehaviour, IMovable
{
    //------------------------------------this player variables
    //THIS PLAYERS LAST DIRECTION CHANGE
    Vector3 LastDirection;
    
    //THIS PLAYERS RIGIDBODY
    Rigidbody2D rb;

    //THIS PLAYERS CURRENT SPEED
    int speed;

    //CURRENT INVINCIBILITY LEVEL
    bool invincible = false;

    //GETTER/SETTERS
    public Vector3 lastdir
    {
        get { return LastDirection; }
        set { LastDirection = value; }
    }
    public int Speed 
    {
        get { return speed; }
        set { speed = value; }
    }
    public bool Invincible
    {
        get { return invincible; }
        set { invincible = value; }
    }

    bool alive;
   

    //------------------------------------end player variables



    //------------------------------------Wall variables
    //THIS PLAYERS WALLPREFAB
    GameObject wallPrefab;

    //THIS WALLS COLLIDER
    Collider2D wall;

    //LOCATION WHERE THE LAST WALL ENDED
    Vector2 lastWallEnd;

    //GETTER/SETTERS
    public bool Alive
	{
		get { return alive; }
		set { alive = value; }
	}

    public GameObject wallprefab
    {
        get { return wallPrefab; }
        set { wallPrefab = value; }
    }
    public Collider2D Wall
    {
        get { return wall; }
        set { wall = value; }
    }
    //------------------------------------end wall variables\


    private void Awake()
    {
        //fill in some local variables
        rb = GetComponent<Rigidbody2D>();
        speed = 16;
        Alive = true;
    }    

    public void directionChanger(Vector3 direction)
    {
		if (Alive)
		{
            rb.velocity = direction * speed;
            LastDirection = direction;
            spawnWall();
        }
    }


    private void Update()
    {                                                              //CRASHES IF MOVING TO OTHER DIRECTION
                                                                   //also not nessecary?
        fitColliderBetween(wall, lastWallEnd, transform.position /*- lastdir * 1.205f*/ );
    }

    public void spawnWall()
    {
        if(Alive)
        {
            lastWallEnd = transform.position;
            GameObject w = Instantiate(wallPrefab, transform.position , Quaternion.identity);
            wall = w.GetComponent<Collider2D>();
            w.tag = "playerWall";
            w.layer = 0;
			var obs = w.AddComponent<NavMeshObstacle>();
			obs.carving = true;
			obs.carveOnlyStationary = false;
		}
	}

    public void fitColliderBetween(Collider2D co, Vector3 a, Vector3 b)
    {
        // Calculate the Center Position
        co.transform.position = a + (b - a) * 0.5f;

        // Scale it (horizontally or vertically)
        float dist = Vector2.Distance(a, b);
        if (a.x != b.x)
            co.transform.localScale = new Vector2(dist+0.99f, 1);
        else
            co.transform.localScale = new Vector2(1, dist+0.99f);
    }



    public void OnTriggerEnter2D(Collider2D collider)
    {
		if (collider != wall)
		{
			if (!Invincible)
			{
				if (collider.tag != "Powerup")
				{
					die();
                    print(gameObject + " being killed by " + collider.name);
                }
			}
		}
	}


    public async void die()
    {
        if (gameObject)
        {
           
            AudioClip derezz = Resources.Load<AudioClip>("music/derezz");

            if (sm.Instance != null)
            {
                sm.Instance.MusicSource.Stop();
                sm.Instance.Play(derezz);
            }

            try
            {
                GetComponent<SpelerController>().enabled = false;
            }
            catch
            {
                GetComponent<BotController>().enabled = false;
                GetComponent<BotController>().gameObject.SetActive(false);
            }


            Color c = GetComponent<SpriteRenderer>().color;
            c.a = 0;
            GetComponent<SpriteRenderer>().color = c;

            string wallname = wallprefab.name;
            GameObject[] walls = GameObject.FindGameObjectsWithTag("playerWall");

            foreach (GameObject wall in walls)
            {
                if (wall.name.Contains(wallname))
                {
                    wall.SetActive(false);
                }
            }

            await new WaitForSeconds(1);

            PlayerPrefs.SetInt("AlivePlayers", PlayerPrefs.GetInt("AlivePlayers") - 1);
            Alive = false;
        }		
    }
}
