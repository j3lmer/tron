using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchScreenModule : MonoBehaviour
{

	List<Button> arrows;
	int PVP;
	int CONT;

	private void Start()
	{
		getTemplates();

		gamemodeCheck();


	}


	void gamemodeCheck()
	{
		var PVP = PlayerPrefs.GetInt("PVP");
		var CONT = PlayerPrefs.GetInt("contenders");


		switch (PVP)
		{
			case 0:

				break;

			case 1:
				break;
		}

	}

	void makeButton(string color)
	{
		

	}


	void getTemplates()
	{
		var t = GameObject.Find("template");

		arrows = new List<Button>();

		for(var i=0; i< t.transform.childCount; i++)
		{
			var thisarrow = t.transform.GetChild(i);

			arrows.Add(thisarrow.GetComponent<Button>());

			thisarrow.gameObject.SetActive(false);

		}
	}
		

}
