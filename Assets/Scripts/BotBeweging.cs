using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class BotBeweging : MonoBehaviour
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
        initDirection();

        spawnWall();
    }

    void initDirection()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Vector3 vDir = rb.transform.InverseTransformDirection(rb.velocity);

        if (vDir.x > 0)
        {
            //rechts
            //print($"{rb.name} vdir.x groter = {vDir.x}");
            lastDirection = Vector3.right;
        }
        else if (vDir.x < 0)
        {
            //links
            //print($"{rb.name} vdir.x groter = {vDir.x}");
            lastDirection = Vector3.left;
        }
        else if (vDir.y > 0)
        {
            //omhoog
            //print($"{rb.name} vdir.y groter = {vDir.y}");
            lastDirection = Vector3.up;
        }
        else if (vDir.y < 0)
        {
            //beneden
            //print($"{rb.name} vdir.y kleiner = {vDir.y}");
            lastDirection = Vector3.down;
        }
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


    void directionChanger(Vector3 direction)
    {
        //code om van richting te veranderen, richting word veranderd naar de aangegeven richting
        rb.velocity = direction * speed;
        lastDirection = direction;
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
                    StartCoroutine(LoadAudio());
                }
                //als de speler wel ondoodbaar is op dit moment
                else
                {
                    if (co.tag == "wall")
                    {
                        print("Player lost: " + name);
                        StartCoroutine(LoadAudio());
                    }
                }
            }
        }
    }

    IEnumerator LoadAudio()
    {
        string link = "file://" + Application.dataPath + "/Resources/music/derezz.ogg";
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(link, AudioType.OGGVORBIS))
        {
            yield return www.SendWebRequest();

            if (www.error != null)
            {
                Debug.Log(www.error);

            }
            else
            {
                SoundManager.Instance.MusicSource.Stop();
                derezz = DownloadHandlerAudioClip.GetContent(www);
                print(derezz);
                SoundManager.Instance.Play(derezz);

                //haal deze speler op, en de lightwall die daar aan vast zit, en zet de lightwalls op inactive en de speler invisible
                GameObject player = gameObject;
                Color o = player.GetComponent<SpriteRenderer>().color;
                o.a = 0;
                player.GetComponent<SpriteRenderer>().color = o;

                string prefabname = wallprefab.name;
                var walls = GameObject.FindGameObjectsWithTag("playerWall");

                foreach (GameObject wall in walls)
                {
                    if (wall.name.Contains(prefabname))
                    {
                        wall.SetActive(false);
                    }
                }

                yield return new WaitForSeconds(1);
                Destroy(gameObject);
            }
        }
    }


    async void removeWalls()
    {
        //WIP: remove all walls in current scene for fresh start
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("playerWall"))
        {
            Color color = wall.GetComponent<SpriteRenderer>().color;
            color.a = 0;
            wall.GetComponent<SpriteRenderer>().color = color;
        }
        await new WaitForSeconds(2);
    }

    void killPlayer()
    {
        SoundManager.Instance.Play(derezz);
        //destroys this player when hit
        Destroy(gameObject);
    }

    async void stopRandomPlayer()
    {
        //tries to select a random player, if random player is this player, do thisplayer +1, or thisplayer -1
        GameObject thisPlayer = gameObject;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        var random = UnityEngine.Random.Range(0, players.Length - 1);

        var selectedplayer = players[random];

        if (selectedplayer == thisPlayer)
        {
            for (var i = 0; i < players.Length; i++)
            {
                var t = players[i];
                if (t == selectedplayer)
                {
                    selectedplayer = players[i + 1];
                    if (selectedplayer == null)
                    {
                        selectedplayer = players[i - 1];
                    }
                    break;
                }
            }
        }

        var selectedVelocity = selectedplayer.GetComponent<Rigidbody2D>().velocity;
        selectedplayer.GetComponent<Rigidbody2D>().velocity = new Vector3();

        await new WaitForSeconds(2);

        selectedplayer.GetComponent<Rigidbody2D>().velocity = selectedVelocity;
    }

    async void speedboost()
    {
        //temporarily set speed variable higher and call directionchanger so this is actually done
        //then set speed to original variable
        speed = 30f;
        directionChanger(lastDirection);
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
