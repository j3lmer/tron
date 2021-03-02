﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadFinal : MonoBehaviour
{
	string winningName;

	private void Start()
	{
		checkforFinal();
	}

	// Update is called once per frame

	async void checkforFinal()
	{
		while(PlayerPrefs.GetInt("AlivePlayers") != 1)
        {
			print("test tijdens");
			await new WaitForSeconds(0.2f);
        }

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


		SceneManager.LoadScene("finalScreen");
		PlayerPrefs.SetInt("placedPlayers", 0);
		PlayerPrefs.GetInt("controls", 0);

		if (sm.Instance != null)
		{
			if (sm.Instance.MusicSource.isPlaying)
			{
				sm.Instance.MusicSource.Stop();
			}
		}
	}
}


