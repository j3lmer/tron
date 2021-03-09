using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class loadFinal : MonoBehaviour
{
	string winningName;

	private void Start()
	{
		checkforFinal();
	}

	async void checkforFinal()
	{	
		
		while(PlayerPrefs.GetInt("AlivePlayers") > 1)
        {			
			await new WaitForEndOfFrame();
        }

		NavMesh.RemoveAllNavMeshData();

		if(PlayerPrefs.GetInt("AlivePlayers") == 0)
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


