using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
	public class MenuController : MonoBehaviour
	{
		//TODO: MAKE BACK BTN INSTEAD OF HOME

		//variables for accessing menus + canvas
		GameObject _menuObject;
		Canvas _canvas;
		List<GameObject> menuList = new List<GameObject>();
		//end menu blocks

		//audioclips
		public AudioClip MenuMusic;
		public AudioClip buttonClick;
		//end clips

		//variables for settings
		Camera SecondaryCamera;
		RawImage _currentWallpaper;
		Texture original_wallpaper;
		Texture alternate_wallpaper;
		//end

		//controlfont
		public TMP_FontAsset fontAsset;


		Slider _slider;
		private bool _sliderActive = false;

		private bool _touchActive = false;

		private void Start()
		{
			/* 
		 * retrieves all menu gameobjects + canvas.
		 * then sets onclickListeners to all buttons inside.
		 */
			GetMenuObjects();
			PlayMusic();

			if (Debug.isDebugBuild)
			{
				Debug.Log("This is the (latest) debug build!");
			}

			_currentWallpaper = GameObject.Find("RawImage").GetComponent<RawImage>();
			original_wallpaper = Resources.Load<Texture>("Images/default_background");
			alternate_wallpaper = Resources.Load<Texture>("Images/alternate_background");

		
		}


		async void PlayMusic()
		{
			await new WaitForSeconds(0.1f);
			if (sm.Instance.MusicSource.isPlaying == false)
			{
				sm.Instance.PlayMusic(MenuMusic);
			}
		}

		void GetMenuObjects()
		{
			_menuObject = GameObject.Find("MenuObject");		

			_canvas = _menuObject.GetComponentInChildren<Canvas>();
			var canvasTransform = _canvas.transform;

			//loop through canvas children and if its part of the vertical layoutgroup, it must a menu gameobject. 
			//so then add it to the list, and set an onclicklistener for the specific button
			for (var i=0; i< canvasTransform.childCount; i++)
			{
				var thisObj = canvasTransform.GetChild(i);
			
				if (thisObj.GetComponent<VerticalLayoutGroup>())
				{
					var thisGameObj = thisObj.gameObject;
					menuList.Add(thisGameObj);
					setOnClickListeners(thisGameObj);
				}
			}
		}

		void setOnClickListeners(GameObject menu) 
		{
			//menu is one of the children on canvas.
			var menuTransform = menu.transform;
		
			for(var i=0; i<menuTransform.childCount; i++)
			{
				Transform thisObj = menuTransform.GetChild(i);
				if(thisObj == null)
				{
					print("breaking");
					break;
				}
			
				//GameObject thisGameObject = thisObj.gameObject;
				Button thisBtn = thisObj.GetComponent<Button>();	

				//add onclicklistener to any non-untagged button
				if (thisBtn != null && !thisBtn.CompareTag("Untagged"))
				{
					thisBtn.onClick.AddListener(delegate { CheckBtnTag(thisBtn); });
				}			
			}		
		}

		void CheckBtnTag(Button thisBtn)
		{
			sm.Instance.Play(buttonClick);
			switch (thisBtn.tag)
			{
				case "leaderboardTrigger":
					InvokeLeaderboard();
					break;

				case "crossScene":
					sm.Instance.MusicSource.Stop();
					CrossScene(thisBtn);
					break;

				case "menuActivator":
					MenuActivator(thisBtn);
					try
					{
						Toggle toggle = GameObject.Find("Toggle").GetComponent<Toggle>();

						switch (toggle.transform.parent.name)
						{
							case "MusicEnabler":
								Musicenabler(toggle);
								break;
							case "TouchScreen":
								touchscreenenabler(toggle);
								_touchActive = true;
								break;
						}
					}
					catch{break;}
					break;

				case "backBtn":
					Home(thisBtn);
					_sliderActive = false;
					_touchActive = false;
					break;


				case "exit":
					_sliderActive = false;
					_touchActive = false;
					Exit();
					break;
			}
		}

		void Musicenabler(Toggle toggle)
		{
			toggle.isOn = sm.Instance.MusicSource.mute;
			_slider = GameObject.Find("Slider").GetComponent<Slider>();
			_slider.value = sm.Instance.MusicSource.volume;
			_sliderActive = true;
		}

		void touchscreenenabler(Toggle toggle)
		{
			switch (PlayerPrefs.GetInt("Touch"))
			{
				case 0:
					toggle.isOn = false;
					print(toggle.isOn);
					break;

				case 1:
					toggle.isOn = true;
					print(toggle.isOn);
					break;
			}
		}



		private void Update()
		{
			if (_sliderActive)
			{
				GameObject.Find("SoundManager").GetComponent<VolumeValueChange>().SetVolume(_slider.value);
			}

			if (!_touchActive) return;
			if (!GameObject.Find("Toggle")) return;
			
			switch (GameObject.Find("Toggle").GetComponent<Toggle>().isOn)
			{
				case true:
					if (PlayerPrefs.GetInt("Touch") != 1)
					{
						print("touch set to true");
						PlayerPrefs.SetInt("Touch", 1);
					}
					break;

				case false:
					if (PlayerPrefs.GetInt("Touch") != 0)
					{
						print("touch set to false");
						PlayerPrefs.SetInt("Touch", 0);
					}
					break;
			}
		}

		void MenuActivator(Button button)
		{
			//button is the button clicked
			GameObject thisMenu = button.transform.parent.gameObject;
			thisMenu.SetActive(false);
		
			switch (button.name)
			{
				case "StartKnop":
					//failsafe for when there isnt a pvp pref stored
					if (!PlayerPrefs.HasKey("PVP"))
					{
						PlayerPrefs.SetInt("PVP", 1);
					}

					var pvp = checkGamemode();

					switch (pvp)
					{
						case 1:
							ListLoop(menuList, "SpelersKiezen");
							break;

						case 0:
							ListLoop(menuList, "moeilijkheidsgraad");
							break;
					}

					break;

				case "OptionsKnop":
					ListLoop(menuList, "OptionsMenu");
					break;

				case "ChooseStyle":
					ListLoop(menuList, "chooseStyleMenu");
					break;

				case "Muziek":
					ListLoop(menuList, "MusicMenu");
					break;

				case "PVP":
					PVP();
					ListLoop(menuList, "SpelersKiezen");
					break;

				case "PVE":
					PVE();
					ListLoop(menuList, "moeilijkheidsgraad");
					break;

				case "makkelijk":
					PlayerPrefs.SetInt("difficulty",0);
					ListLoop(menuList, "BotsKiezen");
					break;

				case "normaal":
					PlayerPrefs.SetInt("difficulty", 1);
					ListLoop(menuList, "BotsKiezen");
					break;

				case "moeilijk":
					PlayerPrefs.SetInt("difficulty", 2);
					ListLoop(menuList, "BotsKiezen");
					break;

				case "SchemeShower":
					ListLoop(menuList, "Schemes");
					break;

				case "Controls":
					ListLoop(menuList, "Controls");
					_currentWallpaper.texture = alternate_wallpaper;


					GameObject rows = GameObject.Find("rows");
					GameObject playerlist = GameObject.Find("players");

					Color[] colors = { new Color(232, 0, 254, 1), Color.cyan, Color.yellow, Color.green };

					for(var i=0; i<rows.transform.childCount; i++)
					{
						TextMeshProUGUI thischild = rows.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
						thischild.font = fontAsset;
						thischild.color = new Color32(128, 149, 255,255);
					}

					for (var j=0; j<playerlist.transform.childCount; j++)
					{
						TextMeshProUGUI thischild = playerlist.transform.GetChild(j).GetComponent<TextMeshProUGUI>();
						thischild.font = fontAsset;
						thischild.color = colors[j];

						for(var k=0; k<thischild.transform.childCount; k++)
						{
							thischild.transform.GetChild(k).GetComponent<TextMeshProUGUI>().font = fontAsset;
						}
					}
				
					break;

				case "Powerups":
					ListLoop(menuList, "Powerups");
					_currentWallpaper.texture = alternate_wallpaper;

					GameObject blocks = GameObject.Find("blocks");
					GameObject desc = GameObject.Find("descriptions");

					Color[] playercolors = { new Color32(0, 134, 227, 255), new Color32(96, 40, 250, 255), Color.red, new Color32(147, 229, 30, 255), Color.yellow, Color.magenta };

					for(var i=0; i< blocks.transform.childCount; i++)
					{
						SpriteRenderer thisBlock = blocks.transform.GetChild(i).GetComponent<SpriteRenderer>();
						thisBlock.color = playercolors[i];
					}

					for(var j=0; j< desc.transform.childCount; j++)
					{
						TextMeshProUGUI thisText = desc.transform.GetChild(j).GetComponent<TextMeshProUGUI>();
						thisText.font = fontAsset;
					} 


					break;


				case "home":
					SceneManager.LoadScene(0);
					break;
			}		
		}

		void ListLoop(List<GameObject> menuL, string stop)
		{
			for (var i = 0; i < menuL.Count; i++)
			{
				var thisitem = menuList[i];
				if (thisitem.name == stop)
				{
					thisitem.SetActive(true);
				}
			}
		}	

		int checkGamemode()
		{
			return PlayerPrefs.GetInt("PVP");
		}

		void PVP()
		{
			PlayerPrefs.SetInt("PVP", 1);
		}

		void PVE()
		{
			PlayerPrefs.SetInt("PVP", 0);
		}

		public void Mute()
		{
			Toggle mute = GameObject.Find("Toggle").GetComponent<Toggle>();				
			switch (mute.isOn)
			{
				case true:
					sm.Instance.MusicSource.mute = true;
					break;

				case false:
					sm.Instance.MusicSource.mute = false;
					break;
			}
		}

		void CrossScene(Button button)
		{
			//button is button clicked
			//ik moet hier weten welke button dit is, zodat ik een playerpref kan zetten gebaseerd op welke ze klikken
			//dus eerst checken of pvp ja of nee want dan weet je in welk menu ze zitten
			//checken hoeveel spelers/bots ze hebben aangeklikt dmv checken van de naam van de knop
			char buttonChar = button.name[0];

			switch (PlayerPrefs.GetInt("PVP"))
			{
				case 1://PVP
					switch (buttonChar)
					{
						case '2':
							PlayerPrefs.SetInt("contestants", 2);
							break;

						case '3':
							PlayerPrefs.SetInt("contestants", 3);
							break;

						case '4':
							PlayerPrefs.SetInt("contestants", 4);
							break;
					}
					break;

				case 0://PVE
					switch (buttonChar)
					{
						case '1':
							PlayerPrefs.SetInt("contestants", 2);
							break;

						case '2':
							PlayerPrefs.SetInt("contestants", 3);
							break;

						case '3':
							PlayerPrefs.SetInt("contestants", 4);
							break;
					}
					break;			
			}

			//print("<color=yellow>PVP = " + PlayerPrefs.GetInt("PVP")+"</color>");
			//print("<color=yellow>contestants = " + PlayerPrefs.GetInt("contestants") +"</color>");
			SceneManager.LoadScene(1);
		}

		void InvokeLeaderboard()
		{
			SceneManager.LoadScene(2);
		}

		void Home(Button button)
		{
			//check what menu this back button is part of
			GameObject thisMenu = button.transform.parent.gameObject;
		
			//set background to default if needed
			if (_currentWallpaper.texture != original_wallpaper)
				_currentWallpaper.texture = original_wallpaper;

			//loop through menu list, stop when this menu is found, set this menu inactive, set previous menu active(WIP)
			foreach (var thisItem in menuList)
			{
				if (thisItem.name != "MainMenu") continue;
				thisMenu.SetActive(false);
				thisItem.SetActive(true);
			}

		}

		public void Exit()
		{
			Application.Quit();
		}

	}
}