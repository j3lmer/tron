using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		List<GameObject> allUsers = GameObject.FindGameObjectsWithTag("Player").ToList();
		var bots = GameObject.FindGameObjectsWithTag("Bot");
		foreach(GameObject bot in bots)
        {
			allUsers.Add(bot);
        }

		//als alle gebruikers kleiner zijn dan 2, geef de juiste informatie mee aan het finalscreen script
		//laad de final screen
		int i = 0;
		if (i == 0) 
		{ 
			if(allUsers.Count < 2 && allUsers.Count > 0)
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
				if (sm.Instance != null)
                {
					if (sm.Instance.MusicSource.isPlaying)
					{
						sm.Instance.MusicSource.Stop();
					}
				}				
			} 
			//als beide spelers dood zijn, geef gelijkspel mee als winnaam
			else if(allUsers.Count == 0)
			{
				print(allUsers.Count);
				winningName = "Gelijkspel!";
				StaticClass.CrossSceneInformation = winningName;
				SceneManager.LoadScene("finalScreen");
				PlayerPrefs.SetInt("placedPlayers", 0);
				PlayerPrefs.GetInt("controls", 0);
				PlayerPrefs.SetString("winner", winningName);
				if (sm.Instance.MusicSource.isPlaying)
				{
					sm.Instance.MusicSource.Stop();
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

