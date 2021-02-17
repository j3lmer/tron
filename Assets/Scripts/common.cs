using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class common : PowerUps
{
	//lijst aan shared code:
	/*
	 * spawnWall
	 * fitColliderBetween
	 * 
	 * powerups:
	 * removewalls
	 * killplayer
	 * stoprandomplayer
	 * speedboost
	 * setInvincible
	 * doInvincible 
	 * */


	int speed = 16; // default speed of every character

	//dit karakter variabelen
		Rigidbody2D rb; //dit karakters rigidbody
		GameObject wallprefab;// dit karakters wall prefab
		Vector3 lastDirection; // laatste kant die we op zijn gegaan
	//einde dit karakter variabelen


	//get/set variabelen
		public GameObject wallPrefab
		{
			set { wallprefab = value; }
		}
		
	//einde get/set variabelen





	protected Vector3 initDirection()
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

		return lastDirection;
	}

	protected Vector3 directionChanger(Vector3 direction)
	{
		//code om van richting te veranderen, richting word veranderd naar de aangegeven richting
		rb.velocity = direction * speed;
		lastDirection = direction;
		return lastDirection;
	}

	protected IEnumerator deathAudio()
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
				var derezz = DownloadHandlerAudioClip.GetContent(www);
				//print(derezz);
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







}
