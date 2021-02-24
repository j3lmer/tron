using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speler : MonoBehaviour, IMovable
{
    //                  this player variables
    Vector3 LastDirection;
    public Vector3 lastdir 
    {
        get { return LastDirection; }
        set { LastDirection = value; }
    }
    Rigidbody2D rb;
    int speed;
    //                  end player variables



    //                  Wall variables
    GameObject wallPrefab;
    public GameObject wallprefab
    {
        get { return wallPrefab; }
        set { wallPrefab = value; }
    }
    //THIS WALLS COLLIDER
    Collider2D wall;
    //LOCATION WHERE THE LAST WALL ENDED
    Vector2 lastWallEnd;
    //                  end wall variables




    private void Awake()
    {
        //fill in some local variables
        rb = GetComponent<Rigidbody2D>();
        speed = 16;
    }

    private void Update()
    {
        fitColliderBetween(wall, lastWallEnd, transform.position);
    }

    public void directionChanger(Vector3 direction)
    {
        rb.velocity = direction * speed;
		LastDirection = direction;
        spawnWall();
    }

    public void spawnWall()
    {
        lastWallEnd = transform.position;
        GameObject w = Instantiate(wallPrefab, transform.position, Quaternion.identity);
        wall = w.GetComponent<Collider2D>();
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
}
