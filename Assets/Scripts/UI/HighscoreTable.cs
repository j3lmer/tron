using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class HighscoreTable : MonoBehaviour
{
	public Transform entryContainer;
	public Transform entryTemplate;

	public Font AlienEncounter; // player
	public Font Broadway; // pos/score/naam
	public Font Streamster; // leaderboard?
	public Font VCR;


	private List<string> highscores = new List<string>();

	public Button backButton;

	public AudioClip backClip;

	private void Awake()
	{
		//haal standaard template weg
		//zet de fonts in op de juiste plekken
		//bouw de leaderboard van json( of maak een nieuwe)
		//luister naar clicks op de backbutton, en executeer dan de back functie
		entryTemplate.gameObject.SetActive(false);
		setFonts();
		makeLeaderboard();
		backButton.onClick.AddListener(back);
	}

	private void setFonts()
	{
		//maak array van alle texts binnen entrytemplate
		Text[] texts = entryTemplate.GetComponentsInChildren<Text>();

		//zet alle texts in de juiste font
		for (var i = 0; i < texts.Length; i++)
		{
			var thisText = texts[i];
			thisText.font = AlienEncounter;
		}

		//maak nieuwe lijst en voeg alle speler-lijn textobjecten toe
		List<Text> PSN = new List<Text>();
		PSN.Add(GameObject.Find("scoretext").GetComponent<Text>());
		PSN.Add(GameObject.Find("posText").GetComponent<Text>());
		PSN.Add(GameObject.Find("naamText").GetComponent<Text>());

		//loop doorheen en zet op vcr
		for (var j = 0; j < PSN.Count; j++)
		{
			var thisText = PSN[j];
			thisText.font = VCR;
		}
	}

	private void back()
	{
		SoundManager.Instance.Play(backClip);
		//laad mainscene wanneer getriggered
		SceneManager.LoadScene("MainMenu");
	}

	private void makeLeaderboard()
	{
		float templateHeight = 35f; // ruimte tussen entrys

		string jsonPath = Application.dataPath + "/json/saveFile.json"; // path waar de json file moet (gaan) staan
		string json; // initialised empty string voor json conversion

		spelers spelerObj; //initialised speler class object voor spelers

		Transform entryTransform; //CHECKEN WAT DEZE 2 SPECIAFIEK ZIJN
		RectTransform entryRectTransform;

		int pos = 0; //positie current speler

		if (File.Exists(jsonPath)) // Als er een file bestaat in de aangegeven path
		{
			//Lees deze dan uit, en sla op
			Debug.Log("File found, reading..");
			json = File.ReadAllText(jsonPath);

			//zet deze dan om naar c# compatible code
			spelerObj = JsonUtility.FromJson<spelers>(json);

			//als er niet niks in het bestand staat...
			if (spelerObj != null)
			{
				//maak dan een nieuwe lijst en sorteer deze op volgorde van de hoogste score, naar de laagste
				List<playerData> GesorteerdeLijst = spelerObj.AllSpelersList.OrderByDescending(o => o.score).ToList();

				//maak dan tijdelijk kleuren aan om mee te geven wanneer de eerste 3 entrys moeten veranderd worden naar deze kleuren
				Color32 gold = new Color32(255, 215, 0, 255);
				Color32 silver = new Color32(192, 192, 192, 255);
				Color32 bronze = new Color32(205, 127, 50, 255);

				//loop dan door de gesorteerde lijst heen...
				for (var j = 0; j < GesorteerdeLijst.Count; j++)
				{
					//en maak voor elke speler een nieuwe entry aan // TODO: MEER DOCUMENTATIE HIER
					entryTransform = Instantiate(entryTemplate, entryContainer);
					entryTransform.tag = "clone";
					entryRectTransform = entryTransform.GetComponent<RectTransform>();
					entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * j);
					entryTransform.gameObject.SetActive(true);

					//pak de texten speciafiek voor hun functie en sla als tijdelijk variabel op
					var scoretext = entryTransform.GetChild(0);
					var naamtext = entryTransform.GetChild(1);
					var postext = entryTransform.GetChild(2);
					pos++; // volgende positienummer (?) TODO: ZOEK UIT VOOR ZEKERHEID

					//zet de eerste 3 spelers hun fontkleur om naar de juiste kleur voor deze plek
					if (j == 0)
					{
						colorChanger(gold, entryTransform);
					}
					else if (j == 1)
					{
						colorChanger(silver, entryTransform);
					}
					else if (j == 2)
					{
						colorChanger(bronze, entryTransform);
					}

					//verhoog de score bij 10 zodat het meer waard lijkt
					var eindscore = GesorteerdeLijst[j].score * 10;

					//zet de score, naam en positietext neer waar het hoort in de entry
					var score = scoretext.GetComponent<UnityEngine.UI.Text>().text = eindscore.ToString();
					var name = naamtext.GetComponent<UnityEngine.UI.Text>().text = GesorteerdeLijst[j].naam;
					var posI = postext.GetComponent<UnityEngine.UI.Text>().text = pos.ToString();

					//stoppen wanneer max van 10 is bereikt
					if (j >= 9)
					{
						return;
					}
				}
			}
			else // Als er wel niks in het bestand staat...
			{
				entryTransform = Instantiate(entryTemplate, entryContainer);
				var scoretext = entryTransform.GetChild(0);

				// error weergeven
				scoretext.GetComponent<UnityEngine.UI.Text>().text = "Gefaald om spelerdata op te halen, Probeer een potje te spelen!";
			}
		}
	}

	private void colorChanger(Color color, Transform transform)
	{
		//wanneer aangeroepen, sla de corresponderende texten op dmv de juiste child te pakken van de meegegeven transform
		var scoretext = transform.GetChild(0);
		var naamtext = transform.GetChild(1);
		var postext = transform.GetChild(2);

		//zet deze alle 3 in de meegegeven kleur
		scoretext.GetComponent<Text>().color = color;
		naamtext.GetComponent<Text>().color = color;
		postext.GetComponent<Text>().color = color;
	}
}