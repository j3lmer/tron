using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //------------------------------------end player variables



    //------------------------------------Wall variables
    //THIS PLAYERS WALLPREFAB
    GameObject wallPrefab;

    //THIS WALLS COLLIDER
    Collider2D wall;

    //LOCATION WHERE THE LAST WALL ENDED
    Vector2 lastWallEnd;

    //GETTER/SETTERS
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
    //------------------------------------end wall variables




    private void Awake()
    {
        //fill in some local variables
        rb = GetComponent<Rigidbody2D>();
        speed = 16;
    }    

    public void directionChanger(Vector3 direction)
    {        
        rb.velocity = direction * speed;
        LastDirection = direction;
        spawnWall();             
    }


    private void Update()
    {
        fitColliderBetween(wall, lastWallEnd, transform.position);
    }

    public void spawnWall()
    {
        lastWallEnd = transform.position;
        GameObject w = Instantiate(wallPrefab, transform.position, Quaternion.identity);
        wall = w.GetComponent<Collider2D>();
        w.tag = "playerWall";
    }

    public void fitColliderBetween(Collider2D co, Vector2 a, Vector2 b)
    {
        // Calculate the Center Position
        co.transform.position = a + (b - a) * 0.5f;

        // Scale it (horizontally or vertically)
        float dist = Vector2.Distance(a, b);
        if (a.x != b.x)
            co.transform.localScale = new Vector2(dist+1, 1);
        else
            co.transform.localScale = new Vector2(1, dist+1);
    }





    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != wall)
        {
            if (!Invincible)
            {
                if(collider.tag != "Powerup")
                {
                    print("Player lost: " + name);
                    Destroy(gameObject);
                }               
            }
                    
        }
    }
}
