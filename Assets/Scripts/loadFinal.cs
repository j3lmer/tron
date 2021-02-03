using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadFinal : MonoBehaviour
{
	private string winningName;

	// Update is called once per frame
	void Update()
	{
		//elke frame kijken of het al is afgelopen
		checkforFinal();
	}

	void checkforFinal()
	{
		//alle gebruikers opslaan
		GameObject[] allUsers = GameObject.FindGameObjectsWithTag("Player");

		//als alle gebruikers kleiner zijn dan 2, geef de juiste informatie mee aan het finalscreen script
		//laad de final screen
		int i = 0;
		if (i == 0) 
		{ 
			if(allUsers.Length < 2 && allUsers.Length > 0)
			{			
				foreach (GameObject player in allUsers)
				{
					winningName = player.name;
					i++; 
					StaticClass.CrossSceneInformation = winningName;
					SceneManager.LoadScene("finalScreen");
					PlayerPrefs.SetInt("placedPlayers", 0);
					PlayerPrefs.GetInt("controls", 0);
					PlayerPrefs.SetString("winner", winningName);
				}
				if (SoundManager.Instance.MusicSource.isPlaying)
				{
					SoundManager.Instance.MusicSource.Stop();
				}
			} 
			//als beide spelers dood zijn, geef gelijkspel mee als winnaam
			else if(allUsers.Length == 0)
			{
				winningName = "Gelijkspel!";
				i++;
				StaticClass.CrossSceneInformation = winningName;
				SceneManager.LoadScene("finalScreen");
				PlayerPrefs.SetInt("placedPlayers", 0);
				PlayerPrefs.GetInt("controls", 0);
				PlayerPrefs.SetString("winner", winningName);
				if (SoundManager.Instance.MusicSource.isPlaying)
				{
					SoundManager.Instance.MusicSource.Stop();
				}
			}
		}
	}
}
public static class StaticClass
{
	//nodig om informatie cross scene mee te geven
	public static string CrossSceneInformation { get; set; }
}

