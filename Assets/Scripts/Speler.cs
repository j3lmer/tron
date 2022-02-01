using System.Collections;
using System.Collections.Generic;
using controllers;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class Speler : MonoBehaviour, IMovable
{
    //------------------------------------this player variables
    //THIS PLAYERS LAST DIRECTION CHANGE
    Vector3 _lastDirection;
    
    //THIS PLAYERS RIGIDBODY
    Rigidbody2D _rb;

    //THIS PLAYERS CURRENT SPEED
    int _speed;

    //CURRENT INVINCIBILITY LEVEL
    bool _invincible = false;

    //GETTER/SETTERS
    public Vector3 lastdir
    {
        get { return _lastDirection; }
        set { _lastDirection = value; }
    }
    public int Speed 
    {
        get { return _speed; }
        set { _speed = value; }
    }
    public bool Invincible
    {
        get { return _invincible; }
        set { _invincible = value; }
    }

    bool _alive;
   

    //------------------------------------end player variables



    //------------------------------------Wall variables
    //THIS PLAYERS WALLPREFAB
    GameObject _wallPrefab;

    //THIS WALLS COLLIDER
    Collider2D _wall;

    //LOCATION WHERE THE LAST WALL ENDED
    Vector2 _lastWallEnd;

    //GETTER/SETTERS
    public bool Alive
	{
		get { return _alive; }
		set { _alive = value; }
	}

    public GameObject wallprefab
    {
        get { return _wallPrefab; }
        set { _wallPrefab = value; }
    }
    public Collider2D Wall
    {
        get { return _wall; }
        set { _wall = value; }
    }
    //------------------------------------end wall variables\


    private void Awake()
    {
        //fill in some local variables
        _rb = GetComponent<Rigidbody2D>();
        _speed = 16;
        Alive = true;
    }    

    public void directionChanger(Vector3 direction)
    {
        if (!Alive) return;
        _rb.velocity = direction * _speed;
        _lastDirection = direction;
        spawnWall();
    }


    private void Update()
    {                                                             
        fitColliderBetween(_wall, _lastWallEnd, transform.position);
    }

    public void spawnWall()
    {
        if (!Alive) return;
        var position = transform.position;
        _lastWallEnd = position;
        GameObject w = Instantiate(_wallPrefab, position , Quaternion.identity);
        _wall = w.GetComponent<Collider2D>();
        w.tag = "playerWall";
        w.layer = 0;
        var obs = w.AddComponent<NavMeshObstacle>();
        obs.carving = true;
        obs.carveOnlyStationary = false;
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



    public void OnTriggerEnter2D(Collider2D playerCollider)
    {
        if (playerCollider == _wall) return;
        if (Invincible) return;
        if (playerCollider.CompareTag("Powerup")) return;
        
        die();
        print(gameObject + " being killed by " + playerCollider.name);
    }


    public async void die()
    {
        if (!gameObject) return;
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
        Alive = false;
        PlayerPrefs.SetInt("AlivePlayers", PlayerPrefs.GetInt("AlivePlayers") - 1);
    }
}
