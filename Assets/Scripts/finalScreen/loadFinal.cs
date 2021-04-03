using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class loadFinal : MonoBehaviour
{
	string winningName;

	Speler[] players;
	List<bool> Alives;
	List<bool> TempAlives;


	private void Update()
	{
		players = FindObjectsOfType<Speler>();
		Alives = new List<bool>();
		foreach (Speler player in players)
		{
			if(player.Alive == true)
			{
				Alives.Add(player.Alive);
			}
		}
		print(Alives.Count);

		
	}

	private void Start()
	{
		waituntilinitiated();
		Alives = new List<bool>();
	}

	async void waituntilinitiated()
	{
		while (Alives == null)
		{
			await new WaitForEndOfFrame();
		}
		checkforFinal();
	}


	async void checkforFinal()
	{
		

		while(Alives.Count > 2)
        {
			print("routining");
			await new WaitForEndOfFrame();			
        }

		NavMesh.RemoveAllNavMeshData();

		if(Alives.Count == 0)
        {
			winningName = "Gelijkspel!";
			setWinner();
        }
        else if(Alives.Count == 1)
        {			
			var player = GameObject.FindGameObjectWithTag("Player");
			var bot = GameObject.FindGameObjectWithTag("Bot");

			if (player)
            {
				winningName = player.name;
			}

            if (bot)
			{
				winningName = bot.name;
			}
			setWinner();
		}	
	}


	void setWinner()
    {
		PlayerPrefs.SetString("winner", winningName);		
		PlayerPrefs.SetInt("placedPlayers", 0);
		PlayerPrefs.SetInt("controls", 0);
		var cc = PlayerPrefs.GetInt(winningName + "CollectedCoins");
		PlayerPrefs.SetFloat("winnercoins", cc);

		foreach(Speler participant in FindObjectsOfType<Speler>())
		{
			PlayerPrefs.SetInt(participant.name + "CollectedCoins", 0);
		}


		if (sm.Instance != null)
		{
			if (sm.Instance.MusicSource.isPlaying)
			{
				sm.Instance.MusicSource.Stop();
			}
		}

		
		SceneManager.LoadScene("finalScreen");

	}
}


