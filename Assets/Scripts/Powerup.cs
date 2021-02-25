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

                //case "stopPlayer":
                //	stopRandomPlayer();
                //	break;

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
}
