using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D co)
    {
        Speler speler = co.gameObject.GetComponent<Speler>();
        switch (gameObject.name)
        {
			case "SpeedBoost":
				speedboost(speler);
				break;

            case "Invincible":
                setInvincible(speler);

                break;

            case "stopPlayer":
                var cont = speler.GetComponent<SpelerController>();
                stopRandomPlayer(speler);
                break;

                //case "poison":
                //	killPlayer();
                //	break;

                //case "RemoveWalls":
                //	removeWalls();
                //	break;
        }

        Destroy(gameObject);
    }


	async void speedboost(Speler speler)
    {
		speler.Speed = 30;
		speler.directionChanger(speler.lastdir);

		await new WaitForSeconds(4);

		speler.Speed = 16;
		speler.directionChanger(speler.lastdir);
	}    

    async void setInvincible(Speler speler)
    {
        speler.Invincible = true;
        doInvincible(speler);
        await new WaitForSeconds(4);
        speler.Invincible = false;
    }

    async void doInvincible(Speler speler)
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

    async void stopRandomPlayer(Speler speler)
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
                    selectedplayer = spelers[i + 1];
                    if (selectedplayer == null)
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
}
