﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
	//haal muren op
	//kies een random positie op een random tijd BINNEN die muren
	GameObject[] holders;
	public GameObject Dot;


	private void Start()
	{
		holders = fillHolders();

		GameObject outerObj = getOuterWalls(holders);
		GameObject innerObj = getInnerWalls(holders);

		int timeStamp = randTime();

		List<string> powerups = getPowerups();

		List<Color> colors = getColors();
		
		StartCoroutine(setRandomDot(timeStamp, powerups, colors));
	}

	List<Color> getColors()
	{
		Color32 speedColor = new Color32(0, 134, 227, 255);
		Color32 invinciColor = new Color32(251, 184, 41, 255);
		Color32 poison = new Color32(147, 229, 30, 255);
		Color stopColor = Color.red;
		Color shoot = Color.yellow;
		List<Color> colors = new List<Color>();

		colors.Add(speedColor);
		colors.Add(invinciColor);		
		colors.Add(stopColor);
		colors.Add(poison);
		colors.Add(shoot);

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
		powerupNames.Add("shoot");

		powerupNames.Add("RemoveWalls");
		return powerupNames;
	}

	/*
	 * instansiate een dot op positie p en wacht t seconden voordat je dat opnieuw doet
	 */
	IEnumerator setRandomDot(int t, List<string> pUps, List<Color> colors)
	{
		var i = 0;
		if (i == 0)
		{
			i++;
			yield return new WaitForSeconds(t);
		}
		var j = 0;

		while (GameObject.FindGameObjectsWithTag("Player").Length > 1)
		{			
			
			Vector3 p = new Vector3(Random.Range(-65, 70), Random.Range(60, -60), 0);
			GameObject thisdot = Instantiate(Dot, p, Quaternion.identity);
			thisdot.transform.localScale = thisdot.transform.localScale * 2;
			thisdot.tag = "Powerup";
			j++;
			print(j);

			int powerupNr = Random.Range(0, 4);

			if(j % 10 == 0)
			{
				thisdot.name = pUps[4];
				thisdot.GetComponent<SpriteRenderer>().color = colors[4];
			}
			else
			{
				thisdot.name = pUps[powerupNr];
				thisdot.GetComponent<SpriteRenderer>().color = colors[powerupNr];
			}
		

			print($"<color=blue>{thisdot.name}</color> instansiated @ <color=red>{p}</color>");
			yield return new WaitForSeconds(t);
		}

		yield return null;
		print("finished");
	}

	int randTime()
	{
		int rndRange = Random.Range(2, 10);
		return rndRange;
	}

	GameObject[] fillHolders()
	{			
		GameObject[] holders = GameObject.FindGameObjectsWithTag("wallHolder");
		return holders;
	}
	GameObject getOuterWalls(GameObject[] holders)
	{
		var outerwalls = holders[1];
		return outerwalls;
	}
	GameObject getInnerWalls(GameObject[] holders)
	{
		var innerwalls = holders[0];
		return innerwalls;
	}
}