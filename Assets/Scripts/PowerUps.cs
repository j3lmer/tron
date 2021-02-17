using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{


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
		lastDirection = directionChanger(lastDirection);
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
