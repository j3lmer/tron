using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class loadFinal : MonoBehaviour
{
	string winningName;

	bool[] bools;


	private void Start()
	{
		checkforFinal();
	}

	private void Update()
	{
		var AlivePlayers = FindObjectsOfType<Speler>();
		List<bool> b = new List<bool>();

		foreach (Speler s in AlivePlayers)
		{			
			b.Add(s.Alive);
		}



		bools = b.ToArray();

		print(bools.Length);
	}


	async void checkforFinal()
	{	
		
		while(bools.Length > 2)
        {
			print("routining");

			await new WaitForEndOfFrame();
        }

		NavMesh.RemoveAllNavMeshData();

		if(bools.Length == 1)
        {
			winningName = "Gelijkspel!";
			setWinner();
        }
        else
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


