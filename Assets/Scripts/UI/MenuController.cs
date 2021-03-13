using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	//variables for accessing menus + canvas
	GameObject menuObject;
	Canvas canvas;
	List<GameObject> menuList = new List<GameObject>();
	//end menu blocks

	//audioclips
	public AudioClip MenuMusic;
	public AudioClip buttonClick;
	//end clips


	Slider slider;
	private bool sliderActive = false;

	private void Start()
	{
		/* 
		 * retrieves all menu gameobjects + canvas.
		 * then sets onclickListeners to all buttons inside.
		 */
		getMenuObjects();
		playMusic();
	}


	async void playMusic()
	{
		await new WaitForSeconds(0.1f);
		if (sm.Instance.MusicSource.isPlaying == false)
		{
			sm.Instance.PlayMusic(MenuMusic);
		}
	}

	void getMenuObjects()
	{
		menuObject = GameObject.Find("MenuObject");		

		canvas = menuObject.GetComponentInChildren<Canvas>();
		var canvasTransform = canvas.transform;

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
		var transform = menu.transform;
		
		for(var i=0; i<transform.childCount; i++)
		{
			Transform thisObj = transform.GetChild(i);
			if(thisObj == null)
			{
				print("breaking");
				break;
			}
			
			//GameObject thisGameObject = thisObj.gameObject;
			Button thisBtn = thisObj.GetComponent<Button>();	

			//add onclicklistener to any non-untagged button
			if (thisBtn != null && thisBtn.tag != "Untagged")
			{
				thisBtn.onClick.AddListener(delegate { checkBtnTag(thisBtn); });
			}			
		}		
	}

	void checkBtnTag(Button thisBtn)
	{
		sm.Instance.Play(buttonClick);
		switch (thisBtn.tag)
		{
			case "leaderboardTrigger":
				invokeLeaderboard();
				break;

			case "crossScene":
				sm.Instance.MusicSource.Stop();
				CrossScene(thisBtn);
				break;

			case "menuActivator":
				menuActivator(thisBtn);
				try
				{
					Toggle mute = GameObject.Find("Toggle").GetComponent<Toggle>();					
					mute.isOn = sm.Instance.MusicSource.mute;					
					slider = GameObject.Find("Slider").GetComponent<Slider>();
					slider.value = sm.Instance.MusicSource.volume;
					slider.value = sm.Instance.MusicSource.volume;
					sliderActive = true;
					//DontDestroySlider dds = new DontDestroySlider(slider);
				}
				catch
				{
					break;
				}
				break;

			case "backBtn":
				home(thisBtn);
				sliderActive = false;
				break;

			case "exit":
				sliderActive = false;
				exit();
				break;
		}
	}

	private void Update()
	{
		if (sliderActive)
		{
			GameObject.Find("SoundManager").GetComponent<VolumeValueChange>().SetVolume(slider.value);
		}
	}

	void menuActivator(Button button)
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
						listLoop(menuList, "SpelersKiezen");
						break;

					case 0:
						listLoop(menuList, "BotsKiezen");
						break;
				}

				break;

			case "OptionsKnop":
				listLoop(menuList, "OptionsMenu");
				break;

			case "ChooseStyle":
				listLoop(menuList, "chooseStyleMenu");
				break;

			case "Muziek":
				listLoop(menuList, "MusicMenu");
				break;

			case "PVP":
				PVP();
				listLoop(menuList, "SpelersKiezen");
				break;

			case "PVE":
				PVE();
				listLoop(menuList, "BotsKiezen");
				break;

			case "home":
				SceneManager.LoadScene(0);
				break;

		}
	}

	void listLoop(List<GameObject> menuL, string stop)
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

	public void mute()
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

	public void TouchScreen()
	{
		var tmode = PlayerPrefs.GetInt("Touch");

		switch (tmode) 
		{
			case 0:
				PlayerPrefs.SetInt("Touch", 1);
				break;

			case 1:
				PlayerPrefs.SetInt("Touch", 0);	
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

	void invokeLeaderboard()
	{
		SceneManager.LoadScene(2);
	}

	void home(Button button)
	{
		//check what menu this back button is part of
		GameObject thisMenu = button.transform.parent.gameObject;

		//loop through menu list, stop when this menu is found, set this menu inactive, set previous menu active(WIP)
		for (var i = 0; i < menuList.Count; i++)
		{
			var thisItem = menuList[i];
			if (thisItem.name == "MainMenu")
			{
				thisMenu.SetActive(false);
				thisItem.SetActive(true);
			}
		}

	}

	void exit()
	{
		Application.Quit();
	}

}