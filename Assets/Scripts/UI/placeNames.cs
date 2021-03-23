﻿using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class placeNames : MonoBehaviour
{
    //textvariabelen
    public TextMeshProUGUI pink;
    public TextMeshProUGUI cyan;
    public TextMeshProUGUI yellow;
    public TextMeshProUGUI green;

    //font voor spelers en bots
    public TMP_FontAsset particiFont;

    //speciale kleur voor roze
    private Color Pink = new Color(232, 0, 254, 1);

	List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();

	bool setnames = false;


    // Start is called before the first frame update.
    void Start()
    {


		//set font colors to correct colors.
		setColors();
		//set the right font.
		setFont();

       		
    }


	private void Update()
	{
		if(PlayerPrefs.GetInt("placedPlayers") == 1)
		{
			if (!setnames)
			{
				//set the text to the player names.
				setNames();
				setnames = true;
			}
		}
	}


	void setNames()
	{
		//grab all players and the length of the array
		List<GameObject> contestants = GameObject.FindGameObjectsWithTag("Player").ToList();
		var bots = GameObject.FindGameObjectsWithTag("Bot");
		foreach (GameObject bot in bots)
		{
			contestants.Add(bot);
		}

		int contLength = contestants.Count;


		//make list of texts and fill it
		texts.Add(pink);
		texts.Add(cyan);
		texts.Add(yellow);
		texts.Add(green);

		/*
		 * loop door de deelnemers heen.
		 * zet deelnemer[i] naar texts[i].	 
		 */
		for (var i = 0; i < contLength; i++)
		{
			//grab contestant name.
			var thisContName = contestants[i].name;
			//grab text index.
			var thisText = texts[i];
			//set text to contestant name
			thisText.text = thisContName;
		}

		//check if there are non used text entrys and empty them
		foreach (TextMeshProUGUI text in texts)
		{
			if (text.text == "New Text" || text.text == null)
			{
				text.text = "";
			}
		}	
	}

	void setColors()//zet de juiste font kleur
	{ 

		pink.color = Pink;
        cyan.color = Color.cyan;
        yellow.color = Color.yellow;
        green.color = Color.green;
    }


	void setFont()//zet font naar juiste font
	{ pink.font = cyan.font = yellow.font = green.font = particiFont; }
}
