using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class finalScreen : MonoBehaviour
{
	public Canvas canvas;
	public TMP_FontAsset winfont;

	public AudioClip click;
	public AudioClip finalMusic;

	private bool blinking;
	private float timer;

	private GameObject Selected;
	private int al = 0;
	private int bl = 0;
	private int cl = 0;

	string Winner;

	private readonly string letters = "ABCDEFGHIJKLMNOPQRSTUVWQXYZ0123456789!?";

	List<GameObject> Invisible;

	// Start is called before the first frame update
	private void Start()
	{
		StopAllCoroutines();
		
		//canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		//setWinner();

		//init();
		//if(sm.Instance)
		//      {
		//	sm.Instance.MusicSource.clip = finalMusic;
		//	sm.Instance.EffectsSource.clip = click;
		//	sm.Instance.MusicSource.Play();
		//}		
	}

	private void Update()
	{
		//if (blinking)
		//{
		//	blinkLetter();
		//}
	}


	private void setWinner()
	{
		var winner = PlayerPrefs.GetString("winner");
		print(winner);

		//message ophalenen hier uit wintext
		var message = GameObject.Find("Message");
		var wintext = message.transform.Find("winText").gameObject.GetComponent<TMP_Text>();
		wintext.font = winfont;

		if (winner != "Gelijkspel!")
		{
			wintext.text = winner + " heeft gewonnen!";
		}
		else
		{
			wintext.text = winner;
		}

		Winner = winner;
		
	}



	private void init()
	{
		List<GameObject> invis = new List<GameObject>();
		Invisible = invis;

		var nInp = GameObject.Find("nameInput");
		var ok2 = GameObject.Find("okKnop2");
		

		Invisible.Add(nInp);
		Invisible.Add(ok2);

		nInp.SetActive(false);
		ok2.SetActive(false);


		GameObject.Find("okKnop").GetComponent<Button>().onClick.AddListener(delegate { setNameInput(); });
	}

	private void setNameInput()
	{
		
		if (Winner.ToLower().Contains("speler"))
		{			
            if (sm.Instance)
            {
				sm.Instance.EffectsSource.Play();
			}
			Invisible[0].SetActive(true);
			GameObject.Find("okKnop").SetActive(false);
			GameObject.Find("Message").transform.Find("winText").gameObject.SetActive(false);

			var ok2 = Invisible[1];
			ok2.SetActive(true);
			ok2.GetComponent<Button>().onClick.AddListener(saveAndTransition);



			Button nextLetterButton = GameObject.Find("nBtn").gameObject.GetComponent<Button>();
			nextLetterButton.onClick.AddListener(nextLetter);

			Button prevLetterButton = GameObject.Find("pBtn").gameObject.GetComponent<Button>();
			prevLetterButton.onClick.AddListener(prevLetter);

			Button selectNextButton = GameObject.Find("fBtn").gameObject.GetComponent<Button>();
			selectNextButton.onClick.AddListener(selectNext);



			Selected = GameObject.Find("Message").transform.Find("A").gameObject;
			blinking = true;
			fillLetters();
			
		}
        else
        {
			print("bot found, not setting");
			if (sm.Instance)
			{
				sm.Instance.MusicSource.Stop();
			}

			SceneManager.LoadScene(0);
		}		
	}	

	

	private void saveAndTransition()
	{
		if (sm.Instance)
		{
			sm.Instance.EffectsSource.Play();
		}
		TMP_Text A = GameObject.Find("A").GetComponent<TMP_Text>();
		TMP_Text B = GameObject.Find("B").GetComponent<TMP_Text>();
		TMP_Text C = GameObject.Find("C").GetComponent<TMP_Text>();
		var winnername = A.text + B.text + C.text;

		saveData(winnername);
		SceneManager.LoadScene(0);
	}

	private void saveData(string winner)
	{
		string jsonPath = Application.dataPath + "/json/saveFile.json";
		string json = "";
		spelers spelerObj = new spelers();
		bool foundWinner = false;

		if (File.Exists(jsonPath))
		{
			print($"File found @ {jsonPath}, Reading..");

			json = File.ReadAllText(jsonPath);
			spelerObj = JsonUtility.FromJson<spelers>(json);

			if (spelerObj != null)
			{
				for (var i = 0; i < spelerObj.AllSpelersList.Count; i++)
				{
					if (spelerObj.AllSpelersList != null && spelerObj.AllSpelersList[i] != null)
					{
						var thisObj = spelerObj.AllSpelersList[i];
						if(thisObj.naam == winner)
						{
							thisObj.score++;
							foundWinner = true;
						}
					}
				}
				if (!foundWinner)
				{
					playerData nieuweSpeler = new playerData();
					nieuweSpeler.naam = winner;
					nieuweSpeler.score++;

					spelerObj.AllSpelersList.Add(nieuweSpeler);
				}

				json = SaveToString(spelerObj);
			}
			else
			{
				Debug.Log("SpelerObject is leeg");
				playerData newPlayer = new playerData();
				newPlayer.naam = winner;
				newPlayer.score++;

				spelerObj.AllSpelersList.Add(newPlayer);
				json = SaveToString(spelerObj);
			}
		}
		else
		{
			print($"No file found @ {jsonPath}. Generating new file..");
			FileStream file;
			if (jsonPath == null)
			{
				file = File.Create(jsonPath);
			}

			playerData newPlayer = new playerData();
			newPlayer.naam = winner;
			newPlayer.score++;

			spelerObj.AllSpelersList.Add(newPlayer);
			json = SaveToString(spelerObj);

		}

		File.WriteAllText(jsonPath, json);

		string SaveToString(spelers Spelers)
		{
			return JsonUtility.ToJson(Spelers);
		}
	}


	private void blinkLetter()
	{
		timer = timer + Time.deltaTime;
		if (timer >= 0.5)
		{
			Color color = Selected.GetComponent<TMP_Text>().color;
			color.a = Mathf.Clamp(1, 0, 1);
			Selected.GetComponent<TMP_Text>().color = color;
		}
		if (timer >= 1)
		{
			Color color = Selected.GetComponent<TMP_Text>().color;
			color.a = Mathf.Clamp(0.5f, 0, 1);
			Selected.GetComponent<TMP_Text>().color = color;
			timer = 0;
		}
	}

	private void nextLetter()
	{
        if (sm.Instance)
        {
			sm.Instance.EffectsSource.Play();
		}		
		char[] Alphabet = letters.ToCharArray();
		GameObject A = GameObject.Find("A");
		GameObject B = GameObject.Find("B");
		GameObject C = GameObject.Find("C");

		if (Selected == A)
		{
			if (al == 39)
			{
				al = 0;
			}
			al++;

			char currentChar = Alphabet[al];
			string currentLetter = currentChar.ToString();
			A.GetComponent<TMP_Text>().text = currentLetter;
		}

		if (Selected == B)
		{
			if (bl == 39)
			{
				bl = 0;
			}
			bl++;

			char currentChar = Alphabet[bl];
			string currentLetter = currentChar.ToString();
			B.GetComponent<TMP_Text>().text = currentLetter;
		}

		if (Selected == C)
		{
			if (cl == 39)
			{
				cl = 0;
			}
			cl++;

			char currentChar = Alphabet[cl];
			string currentLetter = currentChar.ToString();
			C.GetComponent<TMP_Text>().text = currentLetter;
		}
	}

	private void prevLetter()
	{
		if (sm.Instance)
		{
			sm.Instance.EffectsSource.Play();
		}
		char[] Alphabet = letters.ToCharArray();
		GameObject A = GameObject.Find("A");
		GameObject B = GameObject.Find("B");
		GameObject C = GameObject.Find("C");

		if (Selected == A)
		{
			if (al == 0)
			{
				al = 39;
			}
			al--;

			char currentChar = Alphabet[al];
			string currentLetter = currentChar.ToString();
			A.GetComponent<TMP_Text>().text = currentLetter;
		}

		if (Selected == B)
		{
			if (bl == 0)
			{
				bl = 39;
			}
			bl--;

			char currentChar = Alphabet[bl];
			string currentLetter = currentChar.ToString();
			B.GetComponent<TMP_Text>().text = currentLetter;
		}

		if (Selected == C)
		{
			if (cl == 0)
			{
				cl = 39;
			}
			cl--;

			char currentChar = Alphabet[cl];
			string currentLetter = currentChar.ToString();
			C.GetComponent<TMP_Text>().text = currentLetter;
		}
	}

	private void selectNext()
	{
		if (sm.Instance)
		{
			sm.Instance.EffectsSource.Play();
		}
		GameObject A = GameObject.Find("A");
		GameObject B = GameObject.Find("B");
		GameObject C = GameObject.Find("C");

		var color = Selected.GetComponent<TMP_Text>().color;
		color.a = Mathf.Clamp(1, 0, 1);
		Selected.GetComponent<TMP_Text>().color = color;

		if (Selected == A)
		{
			Selected = B;
		}
		else if (Selected == B)
		{
			Selected = C;
		}
		else if (Selected == C)
		{
			Selected = A;
		}
	}

	private void fillLetters()
	{
		Transform messageTransform = GameObject.Find("Message").transform;

		for (var i = 0; i < messageTransform.childCount; i++)
		{
			var thisKid = messageTransform.GetChild(i);
			if (thisKid.name != "winText")
			{
				thisKid.GetComponent<TMP_Text>().text = "A";
			}
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