using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class finalScreen : MonoBehaviour
{
	public Canvas canvas;
	public TMP_FontAsset winfont;

	// Start is called before the first frame update
	private void Start()
	{
		List<GameObject> mainCh = getMainChildren();
		setWinner(mainCh);
	}


	private void setWinner(List<GameObject> UI)
	{
		//var winner = StaticClass.CrossSceneInformation;
		var winner = PlayerPrefs.GetString("winner");
		print(winner);

		//message ophalenen hier uit wintext
		var message = listLoop(UI, "Message");
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
	}

	private GameObject listLoop(List<GameObject> menuL, string stop)
	{
		for (var i = 0; i < menuL.Count; i++)
		{
			var thisitem = menuL[i];
			if (thisitem.name == stop)
			{
				return thisitem;
			}
		}
		return null;
	}

	private List<GameObject> getMainChildren()
	{
		// List<GameObject> canvasKids = getCanvasChildren();
		List<GameObject> mainCh = new List<GameObject>();
		var mainObj = GameObject.Find("MainMenu");

		for (var i = 0; i < mainObj.transform.childCount; i++)
		{
			var thisGObj = mainObj.transform.GetChild(i).gameObject;
			mainCh.Add(thisGObj);
			//print(thisGObj);
		}
		return mainCh;
	}
}