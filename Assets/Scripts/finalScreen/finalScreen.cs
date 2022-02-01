using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace finalScreen
{
	public class FinalScreen : MonoBehaviour
	{
		public Canvas canvas;
		public TMP_FontAsset winfont;

		public AudioClip click;
		public AudioClip finalMusic;

		private bool _blinking;
		private float _timer;

		private GameObject _selected;
		private int _al = 0;
		private int _bl = 0;
		private int _cl = 0;

		string _winner;
		int _winCoins;

		private readonly string letters = "ABCDEFGHIJKLMNOPQRSTUVWQXYZ0123456789!?";

		List<GameObject> _invisible;

		// Start is called before the first frame update
		private void Start()
		{
			StopAllCoroutines();

			canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
			SetWinner();

			Init();
			if (sm.Instance)
			{
				sm.Instance.MusicSource.clip = finalMusic;
				sm.Instance.EffectsSource.clip = click;
				sm.Instance.MusicSource.Play();
			}
		}

		private void Update()
		{
			if (!_blinking) return;
			BlinkLetter();
		}


		private void SetWinner()
		{
			var winner = PlayerPrefs.GetString("winner");

			_winCoins = PlayerPrefs.GetInt("winnercoins");

			//message ophalenen hier uit wintext
			var message = GameObject.Find("Message");
			var wintext = message.transform.Find("winText").gameObject.GetComponent<TMP_Text>();
			wintext.font = winfont;

			wintext.text = winner != "Gelijkspel!" ? winner + " heeft gewonnen!" : winner;

			_winner = winner;
		}



		private void Init()
		{
			List<GameObject> invis = new List<GameObject>();
			_invisible = invis;

			var nInp = GameObject.Find("nameInput");
			var ok2 = GameObject.Find("okKnop2");
		

			_invisible.Add(nInp);
			_invisible.Add(ok2);

			nInp.SetActive(false);
			ok2.SetActive(false);


			GameObject.Find("okKnop").GetComponent<Button>().onClick.AddListener(delegate { SetNameInput(); });
		}

		private void SetNameInput()
		{
			if (!_winner.ToLower().Contains("speler"))
			{
				print("bot found, not setting");
				if (sm.Instance)
				{
					sm.Instance.MusicSource.Stop();
				}

				SceneManager.LoadScene(0);
				return;
			}
			
			if (sm.Instance)
			{
				sm.Instance.EffectsSource.Play();
			}

			_invisible[0].SetActive(true);
			GameObject.Find("okKnop").SetActive(false);
			GameObject.Find("Message").transform.Find("winText").gameObject.SetActive(false);

			var ok2 = _invisible[1];
			ok2.SetActive(true);
			ok2.GetComponent<Button>().onClick.AddListener(SaveAndTransition);


			Button nextLetterButton = GameObject.Find("nBtn").gameObject.GetComponent<Button>();
			nextLetterButton.onClick.AddListener(NextLetter);

			Button prevLetterButton = GameObject.Find("pBtn").gameObject.GetComponent<Button>();
			prevLetterButton.onClick.AddListener(PrevLetter);

			Button selectNextButton = GameObject.Find("fBtn").gameObject.GetComponent<Button>();
			selectNextButton.onClick.AddListener(SelectNext);


			_selected = GameObject.Find("Message").transform.Find("A").gameObject;
			_blinking = true;
			fillLetters();
		
		}	

	

		public void SaveAndTransition()
		{
			if (sm.Instance)
			{
				sm.Instance.EffectsSource.Play();
			}
			TMP_Text A = GameObject.Find("A").GetComponent<TMP_Text>();
			TMP_Text B = GameObject.Find("B").GetComponent<TMP_Text>();
			TMP_Text C = GameObject.Find("C").GetComponent<TMP_Text>();
			var winnername = A.text + B.text + C.text;

			SaveData(winnername);
			SceneManager.LoadScene(0);
		}

		private void SaveData(string winner)
		{
			string jsonPath = Application.dataPath + "/json/saveFile.json";
			string json = "";
			Spelers spelerObj = new Spelers();
			bool foundWinner = false;

			if (!Directory.Exists(Application.dataPath + "/json/"))
			{
				Directory.CreateDirectory(Application.dataPath + "/json/");
			}


			if (File.Exists(jsonPath))
			{
				print($"File found @ {jsonPath}, Reading..");

				json = File.ReadAllText(jsonPath);
				spelerObj = JsonUtility.FromJson<Spelers>(json);

				if (spelerObj != null)
				{
					for (var i = 0; i < spelerObj.AllSpelersList.Count; i++)
					{
						if (spelerObj.AllSpelersList != null && spelerObj.AllSpelersList[i] != null)
						{
							var thisObj = spelerObj.AllSpelersList[i];
							if(thisObj.naam == winner)
							{
								thisObj.score += 10;
								thisObj.score += _winCoins;
								foundWinner = true;
							}
						}
					}
					if (!foundWinner)
					{
						PlayerData nieuweSpeler = new PlayerData();
						nieuweSpeler.naam = winner;
						nieuweSpeler.score += 10;
						nieuweSpeler.score += _winCoins;

						spelerObj.AllSpelersList.Add(nieuweSpeler);
					}

					json = SaveToString(spelerObj);
				}
				else
				{
					Debug.Log("SpelerObject is leeg");
					PlayerData newPlayer = new PlayerData();
					newPlayer.naam = winner;
					newPlayer.score += 10;
					newPlayer.score += _winCoins;

					spelerObj.AllSpelersList.Add(newPlayer);
					json = SaveToString(spelerObj);
				}
			}
			else
			{
				print($"No file found @ {jsonPath}. Generating new file..");
				if (!File.Exists(jsonPath))
				{
					var fs = new FileStream(jsonPath, FileMode.Create);
					fs.Dispose();
				}

				PlayerData newPlayer = new PlayerData();
				newPlayer.naam = winner;
				newPlayer.score += 10;
				newPlayer.score += _winCoins;

				spelerObj.AllSpelersList.Add(newPlayer);
				json = JsonUtility.ToJson(spelerObj);

			}

			File.WriteAllText(jsonPath, json);

			string SaveToString(Spelers spelers)
			{
				return JsonUtility.ToJson(spelers);
			}
		}


		private void BlinkLetter()
		{
			_timer = _timer + Time.deltaTime;
			if (_timer >= 0.5)
			{
				Color color = _selected.GetComponent<TMP_Text>().color;
				color.a = Mathf.Clamp(1, 0, 1);
				_selected.GetComponent<TMP_Text>().color = color;
			}
			if (_timer >= 1)
			{
				Color color = _selected.GetComponent<TMP_Text>().color;
				color.a = Mathf.Clamp(0.5f, 0, 1);
				_selected.GetComponent<TMP_Text>().color = color;
				_timer = 0;
			}
		}

		private void NextLetter()
		{
			if (sm.Instance)
			{
				sm.Instance.EffectsSource.Play();
			}		
			char[] alphabet = letters.ToCharArray();
			GameObject A = GameObject.Find("A");
			GameObject B = GameObject.Find("B");
			GameObject C = GameObject.Find("C");

			if (_selected == A)
			{
				if (_al == 39)
				{
					_al = 0;
				}
				_al++;

				char currentChar = alphabet[_al];
				string currentLetter = currentChar.ToString();
				A.GetComponent<TMP_Text>().text = currentLetter;
			}

			if (_selected == B)
			{
				if (_bl == 39)
				{
					_bl = 0;
				}
				_bl++;

				char currentChar = alphabet[_bl];
				string currentLetter = currentChar.ToString();
				B.GetComponent<TMP_Text>().text = currentLetter;
			}

			if (_selected == C)
			{
				if (_cl == 39)
				{
					_cl = 0;
				}
				_cl++;

				char currentChar = alphabet[_cl];
				string currentLetter = currentChar.ToString();
				C.GetComponent<TMP_Text>().text = currentLetter;
			}
		}

		private void PrevLetter()
		{
			if (sm.Instance)
			{
				sm.Instance.EffectsSource.Play();
			}
			char[] alphabet = letters.ToCharArray();
			GameObject A = GameObject.Find("A");
			GameObject B = GameObject.Find("B");
			GameObject C = GameObject.Find("C");

			if (_selected == A)
			{
				if (_al == 0)
				{
					_al = 39;
				}
				_al--;

				char currentChar = alphabet[_al];
				string currentLetter = currentChar.ToString();
				A.GetComponent<TMP_Text>().text = currentLetter;
			}

			if (_selected == B)
			{
				if (_bl == 0)
				{
					_bl = 39;
				}
				_bl--;

				char currentChar = alphabet[_bl];
				string currentLetter = currentChar.ToString();
				B.GetComponent<TMP_Text>().text = currentLetter;
			}

			if (_selected == C)
			{
				if (_cl == 0)
				{
					_cl = 39;
				}
				_cl--;

				char currentChar = alphabet[_cl];
				string currentLetter = currentChar.ToString();
				C.GetComponent<TMP_Text>().text = currentLetter;
			}
		}

		private void SelectNext()
		{
			if (sm.Instance)
			{
				sm.Instance.EffectsSource.Play();
			}
			GameObject A = GameObject.Find("A");
			GameObject B = GameObject.Find("B");
			GameObject C = GameObject.Find("C");

			var color = _selected.GetComponent<TMP_Text>().color;
			color.a = Mathf.Clamp(1, 0, 1);
			_selected.GetComponent<TMP_Text>().color = color;

			if (_selected == A)
			{
				_selected = B;
			}
			else if (_selected == B)
			{
				_selected = C;
			}
			else if (_selected == C)
			{
				_selected = A;
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
	public class PlayerData
	{
		public string naam;
		public float score;
	}

	[System.Serializable]
	public class Spelers
	{
		public List<PlayerData> AllSpelersList = new List<PlayerData>();
	}
}