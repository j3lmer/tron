using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerupPlacer : MonoBehaviour
{

	public GameObject Dot;

	private void Start()
	{
		Dot = Resources.Load<GameObject>("Prefabs/player");

		List<string> powerups = getPowerups();

		List<Color> colors = getColors();

		int timeStamp = randomTime();

		StartCoroutine(setRandomDot(timeStamp, powerups, colors));
	}

	IEnumerator setRandomDot(int t, List<string> pUps, List<Color> colors)
	{
        var i = 0;
        if (i == 0)
        {
            i++;
            yield return new WaitForSeconds(t);
        }
        var j = 0;


		List<GameObject> contestants = GameObject.FindGameObjectsWithTag("Player").ToList();
		var bots = GameObject.FindGameObjectsWithTag("Bot").ToList();
		contestants.AddRange(bots);

		while (contestants.Count > 1)
		{

			Vector3 p = new Vector3(Random.Range(-65, 70), Random.Range(60, -60), 0);
			GameObject thisdot = Instantiate(Dot, p, Quaternion.identity);
			thisdot.transform.localScale = thisdot.transform.localScale * 2;

			Collider2D[] overlap = Physics2D.OverlapBoxAll(p, thisdot.transform.localScale, 0);

			if(overlap.Length > 1 )
			{
				foreach(Collider2D o in overlap)
				{
					if(o != thisdot)
					{
						thisdot.transform.position = new Vector3(Random.Range(-65, 70), Random.Range(60, -60), 0);
					}
				}				
			}

			thisdot.tag = "Powerup";
			thisdot.AddComponent<Powerup>();
			j++;

			int powerupNr = Random.Range(0, 5);

			if (j % 10 == 0)
			{
				thisdot.name = pUps[5];
				thisdot.GetComponent<SpriteRenderer>().color = colors[5];
			}
			else
			{
				thisdot.name = pUps[powerupNr];
				thisdot.GetComponent<SpriteRenderer>().color = colors[powerupNr];
			}


			//print($"<color=blue>{thisdot.name}</color> instansiated @ <color=red>{p}</color>");
			yield return new WaitForSeconds(t);
		}

		yield return null;
		print("finished");

		enabled = false;
	}


	int randomTime()
	{
		int rndRange = Random.Range(2, 10);
		return rndRange;
	}

	List<Color> getColors()
	{
		Color32 speedColor = new Color32(0, 134, 227, 255);
		Color32 invinciColor = new Color32(96, 40, 250, 255);
		Color32 poison = new Color32(147, 229, 30, 255);
		Color stopColor = Color.red;
		List<Color> colors = new List<Color>();

		colors.Add(speedColor);
		colors.Add(invinciColor);
		colors.Add(stopColor);
		colors.Add(poison);
		colors.Add(Color.yellow);
		colors.Add(Color.magenta);

		return colors;
	}

	List<string> getPowerups()
	{
		List<string> powerupNames = new List<string>();
		powerupNames.Add("SpeedBoost");
		powerupNames.Add("Invincible");
		powerupNames.Add("stopPlayer");
		powerupNames.Add("poison");
		powerupNames.Add("points");
		powerupNames.Add("RemoveWalls");
		return powerupNames;
	}
}
