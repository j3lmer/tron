using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class finalScreen_old : MonoBehaviour
{
	//variabelen voor text
	public GameObject Message;

	public TextMeshProUGUI winText;
	public TextMeshProUGUI A;
	public TextMeshProUGUI B;
	public TextMeshProUGUI C;

	//ok knoppen
	public GameObject OkBtn, okBtn2;

	//kleur aanmaken voor als de roze speler wint
	private Color Pink = new Color(232, 0, 254, 1);

	//knoppen variabelen
	public GameObject nInput;

	public Button pbtr, nbtn, fbtn;

	//checken welke letter is geselecteerd
	private bool ASelected = true;

	private bool BSelected = false;
	private bool CSelected = false;

	//variabelen voor roulatie
	private readonly string letters = "ABCDEFGHIJKLMNOPQRSTUVWQXYZ0123456789!?";

	private int al = 0;
	private int bl = 0;
	private int cl = 0;

	//knipper variabelen
	public float timer;

	//bool voor letter knipperen
	private bool blinking;

	//font
	public TMP_FontAsset winFont;

	// Start is called before the first frame update
	private void Start()
	{
		//Winnernaam ophalen van loadFinal en text weergeven met juiste naam
		//Debug.Log(StaticClass.CrossSceneInformation);
		var winner = StaticClass.CrossSceneInformation;


		//kijk of er wel echt 1 winnaar is en zo niet geef alleen gelijkspel weer
		if (winner != "Gelijkspel!")
		{
			winText.text = winner + " heeft gewonnen!";
		}
		else
		{
			winText.text = winner;
		}
		winText.font = winFont;

		//Speler 1 is altijd roze, 2 cyaan, 3 geel, 4 groen.
		//kleur text aanpassen gebaseerd op kleur van de speler
		if (winner == "Speler 1")
		{
			//Roze
			winText.color = Pink;
		}
		else if (winner == "Speler 2" || winner == "bot 1")
		{
			//Cyaan
			winText.color = Color.cyan;
		}
		else if (winner == "Speler 3" || winner == "bot 2")
		{
			//Geel
			winText.color = Color.yellow;
		}
		else if (winner == "Speler 4" || winner == "bot 3")
		{
			//groen
			winText.color = Color.green;
		}

		var okB = OkBtn.GetComponent<Button>();
		var okB2 = okBtn2.GetComponent<Button>();

		//voeg onclick luisteraars toe voor ok en ok2 en executeer bijbehorende functie wanneer dit gebeurt
		okB.onClick.AddListener(ok);
		//okB2.onClick.AddListener(ok2);
	}

	// Update is called once per frame
	private void Update()
	{
		// als het toegestaan is om te knipperen, functie laden om te knipperen
		if (blinking == true)
		{
			showSelected();
		}
	}

	public void ok()
	{
		//wintext weghalen
		if (winText.text.Contains("heeft gewonnen!") || winText.text.Contains("Gelijkspel!"))
		{
			winText.text = "";
		}

		//okknop verwijderen
		OkBtn.SetActive(false);
		//lettergroep weergeven
		nInput.SetActive(true);
		//andere okknop aanroepen
		okBtn2.SetActive(true);
		//functie aanroepen om text te vullen
		makeInput();
	}

	public void ok2()
	{
		saveData();
		SceneManager.LoadScene(0);
	}

	private void makeInput()
	{
		//text vullen en toestaan om te knipperen
		A.text = "A";
		B.text = "A";
		C.text = "A";
		blinking = true;
	}

	public void nextLetter()
	{
		//wanneer op het pijltje naar rechts geklikt,zet deze letterselectie op false en de volgende op true

		if (ASelected == true && BSelected == false && CSelected == false)
		{
			ASelected = false;
			BSelected = true;
			Debug.Log("B selected");
			return;
		}

		if (BSelected == true && ASelected == false && CSelected == false)
		{
			BSelected = false;
			CSelected = true;
			Debug.Log("C selected");
			return;
		}

		if (CSelected == true && ASelected == false && BSelected == false)
		{
			CSelected = false;
			ASelected = true;
			Debug.Log("A selected");
			return;
		}
		else
		{
			ASelected = true;
			Debug.Log("else A selected");
			return;
		}
	}

	private void showSelected()
	{
		//functie om geselecteerde text te laten knipperen
		timer = timer + Time.deltaTime; //time.deltatime = the amount of seconds it took for the engine to process the previous frame

		if (ASelected == true)
		{
			//ervoor zorgen dat ze niet weg kunnen vallen door switch tijdens disabled
			B.enabled = true;
			C.enabled = true;
			if (timer >= 0.5)
			{
				A.enabled = true;
			}
			if (timer >= 1)
			{
				A.enabled = false;
				timer = 0;
			}
		}
		else if (BSelected == true)
		{
			A.enabled = true;
			C.enabled = true;
			if (timer >= 0.5)
			{
				B.enabled = true;
			}
			if (timer >= 1)
			{
				B.enabled = false;
				timer = 0;
			}
		}
		else if (CSelected == true)
		{
			B.enabled = true;
			A.enabled = true;
			if (timer >= 0.5)
			{
				C.enabled = true;
			}
			if (timer >= 1)
			{
				C.enabled = false;
				timer = 0;
			}
		}
		else
		{
			//a b en c knipperen
			if (timer >= 0.5)
			{
				A.enabled = true;
				B.enabled = true;
				C.enabled = true;
			}
			if (timer >= 1)
			{
				A.enabled = false;
				B.enabled = false;
				C.enabled = false;
				timer = 0;
			}
		}
	}

	public void letterUp()
	{
		//zet alle letters om in een array
		char[] iLetters = letters.ToCharArray();
		if (ASelected && !BSelected && !CSelected)
		{
			//als einde bereikt, begin opnieuw
			if (al == 39)
			{
				al = 0;
				al++;
			}
			else
			{
				//ga door naar volgende entry in array
				al++;
			}
			//neem de current karakter uit het alfabet, zet hem om naar een string en geef deze string aan de juiste text om te veranderen
			char currentChar = iLetters[al];
			string currentLetter = currentChar.ToString();
			A.text = currentLetter;
		}
		//herhaal voor B en C
		if (BSelected && !ASelected && !CSelected)
		{
			if (bl == 39)
			{
				bl = 0;
				bl++;
			}
			else
			{
				bl++;
			}
			char currentChar = iLetters[bl];
			string currentLetter = currentChar.ToString();
			B.text = currentLetter;
		}
		if (CSelected && !BSelected && !ASelected)
		{
			if (cl == 39)
			{
				cl = 0;
				cl++;
			}
			else
			{
				cl++;
			}
			char currentChar = iLetters[cl];
			string currentLetter = currentChar.ToString();
			C.text = currentLetter;
		}
	}

	public void letterDown()
	{
		//letterUp functie maar andersom
		char[] iLetters = letters.ToCharArray();

		if (ASelected && !BSelected && !CSelected)
		{
			if (al == 0)
			{
				al = 39;
				al--;
			}
			else
			{
				al--;
			}
			char currentChar = iLetters[al];
			string currentLetter = currentChar.ToString();
			A.text = currentLetter;
		}

		if (BSelected && !ASelected && !CSelected)
		{
			if (bl == 0)
			{
				bl = 39;
				bl--;
			}
			else
			{
				bl--;
			}
			char currentChar = iLetters[bl];
			string currentLetter = currentChar.ToString();
			B.text = currentLetter;
		}

		if (CSelected && !BSelected && !ASelected)
		{
			if (cl == 0)
			{
				cl = 39;
				cl--;
			}
			else
			{
				cl--;
			}
			char currentChar = iLetters[cl];
			string currentLetter = currentChar.ToString();
			C.text = currentLetter;
		}
	}

	/*
    lees bestand uit, sla winnende speler op
    converteer bestand naar jsonutility object (allspelers)
    check of de winnende spelernaam er al bij zit
    als de winnende spelernaam er bij zit, geef deze een extra punt.
    zet bool op true voor gevondenspeler
    als de loop voorbij is, en foundwinplayer is nog niet op true gezet, maak dan een nieuwe playerdata aan voor deze winnende speler
    manier vinden om deze in spelerObj te zetten
    terugconverteren naar string
    opslaan in bestand
    */

	private void saveData()
	{
		//Pak json bestand locatie en de naam van de winnende speler
		string jsonPath = Application.dataPath + "/json/saveFile.json";
		string thisWinName = A.text + B.text + C.text;
		bool foundWinPlayer = false;
		string json = "";
		spelers spelerObj = new spelers();

		if (File.Exists(jsonPath))
		{ //lees bestand uit als er een bestand al bestaat
			Debug.Log("File found, reading..");

			json = File.ReadAllText(jsonPath);
			spelerObj = JsonUtility.FromJson<spelers>(json);

			if (spelerObj != null)
			{
				//Debug.Log("Length: " + spelerObj.AllSpelersList.Count);

				for (var i = 0; i < spelerObj.AllSpelersList.Count; i++)
				{ //als het json object niet leeg is, loop door de spelers heen
					if (spelerObj.AllSpelersList != null && spelerObj.AllSpelersList[i] != null)
					{
						var thisObj = spelerObj.AllSpelersList[i];
						//Debug.Log(thisObj.naam + " - " + thisObj.score);

						if (thisObj != null)
						{
							if (thisObj.naam == thisWinName)
							{ //voor elke loop, kijk of de win naam er al tussen zit, wanneer dit zo is, geef een extra punt aan de score geassocieerd aan deze naam                                 ;
								spelerObj.AllSpelersList[i].score++;
								foundWinPlayer = true;
								//Debug.Log(foundWinPlayer);
								json = SaveToString(spelerObj);
								File.WriteAllText(jsonPath, json);
							}
						}
					}
				} //als de loop voorbij is, en foundwinplayer is nog niet op true gezet, maak dan een nieuwe playerdata aan voor deze winnende speler
				if (!foundWinPlayer)
				{
					playerData NieuweSpeler = new playerData();
					NieuweSpeler.naam = thisWinName;
					NieuweSpeler.score = NieuweSpeler.score + 1;

					spelerObj.AllSpelersList.Add(NieuweSpeler);
					json = SaveToString(spelerObj);
					File.WriteAllText(jsonPath, json);
				}
			}
			else
			{
				Debug.Log("SpelerObject is leeg");
			}
		}
		else if (!File.Exists(jsonPath))
		{
			Debug.Log("No file found, generating new file");
			FileStream file;

			if (jsonPath == null)
			{
				file = File.Create(jsonPath);
			}

			playerData playerdata = new playerData();

			playerdata.naam = thisWinName;
			playerdata.score = playerdata.score + 1;

			spelerObj.AllSpelersList.Add(playerdata);

			json = SaveToString(spelerObj);
		}

		File.WriteAllText(jsonPath, json);
		string SaveToString(spelers Spelers)
		{
			return JsonUtility.ToJson(Spelers);
		}
	}
}

[System.Serializable]
public class playerData
{
	public string naam;
	public int score;
}

[System.Serializable]
public class spelers
{
	public List<playerData> AllSpelersList = new List<playerData>();
}