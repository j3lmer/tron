using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using controllers;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D co)
    {
        Speler speler = co.gameObject.GetComponent<Speler>();
        if (!speler) return;
        switch (gameObject.name)
        {
            case "SpeedBoost":
                Speedboost(speler);
                break;

            case "Invincible":
                SetInvincible(speler);
                break;

            case "stopPlayer":
                StopRandomPlayer(speler);
                break;

            case "poison":
                speler.die();
                break;

            case "points":                   
                var coins = PlayerPrefs.GetInt(speler.name + "CollectedCoins");
                coins += 1;
                PlayerPrefs.SetInt(speler.name + "CollectedCoins", coins);
                break;

            case "RemoveWalls":
                RemoveWalls();
                break;
        }

        if (sm.Instance != null && gameObject.name != "poison")
        {
            AudioClip powerUp = Resources.Load<AudioClip>("music/powerup");
            sm.Instance.Play(powerUp);
        }

        Destroy(gameObject);
    }


	async void Speedboost(Speler speler)
    {
		speler.Speed = 30;
		speler.directionChanger(speler.lastdir);

		await new WaitForSeconds(4);

		speler.Speed = 16;
		speler.directionChanger(speler.lastdir);
	}    

    async void SetInvincible(Speler speler)
    {
        speler.Invincible = true;
        DoInvincible(speler);
        await new WaitForSeconds(4);
        speler.Invincible = false;
    }

    async void DoInvincible(Speler speler)
    {
        string wallname = speler.Wall.name;
        while (speler.Invincible)
        {
          
            List<GameObject> allwalls = GameObject.FindGameObjectsWithTag("playerWall").ToList();

            foreach (GameObject wall in allwalls)
            {
                if (wall.name == wallname)
                {
                    wall.GetComponent<SpriteRenderer>().color = Color.gray;
                }
            }
            await Task.Delay(1);
        }

        List<GameObject> newwalls = GameObject.FindGameObjectsWithTag("playerWall").ToList();
        foreach (GameObject wall in newwalls)
        {
            if (wall.name == wallname)
            {
                wall.GetComponent<SpriteRenderer>().color = speler.wallprefab.GetComponent<SpriteRenderer>().color;
            }
        }
    }

    async void StopRandomPlayer(Speler speler)
    {
        GameObject thisPlayer = speler.gameObject;

        GameObject[] spelers = GameObject.FindGameObjectsWithTag("Player");

        int random = Random.Range(0, spelers.Length - 1);

        GameObject selectedplayer = spelers[random];

        if (selectedplayer == thisPlayer)
        {
            for (var i = 0; i < spelers.Length; i++)
            {
                var t = spelers[i];
                if (t == selectedplayer)
                {
                    try
                    {
                        if (spelers[i + 1] != null)
                        {
                            selectedplayer = spelers[i + 1];
                        }
                    }
                    catch
                    {
                        selectedplayer = spelers[i - 1];
                    }
                    break;
                }
            }
        }

        Vector2 selectedVelocity = selectedplayer.GetComponent<Rigidbody2D>().velocity;
        selectedplayer.GetComponent<Rigidbody2D>().velocity = new Vector2();
        selectedplayer.GetComponent<SpelerController>().enabled = false;

        await new WaitForSeconds(2);

        selectedplayer.GetComponent<Rigidbody2D>().velocity = selectedVelocity;
        selectedplayer.GetComponent<SpelerController>().enabled = true;

    }

    void RemoveWalls()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("playerWall");

        foreach(GameObject wall in walls)
        {
            Destroy(wall);
        }

        Speler[] spelers = FindObjectsOfType<MonoBehaviour>().OfType<Speler>().ToArray();

        foreach(Speler speler in spelers)
        {
            speler.directionChanger(speler.lastdir);
        }
    }
}
