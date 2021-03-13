using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchScreenModule : MonoBehaviour
{
	Dictionary<string, Button> arrows;

	private void Start()
	{
		getArrows();

		makeButtonSet("green", new Vector3()); 
	}


	void makeButtonSet(string color, Vector3 location)
	{

	}

	void getArrows()
	{
		arrows = new Dictionary<string, Button>();


		var temp = GameObject.Find("template");

		for(var i=0; i<temp.transform.childCount; i++)
		{
			var thisbtn = temp.transform.GetChild(i);
			if (thisbtn.name.ToLower().Contains("cyan"))
			{
				arrows.Add("cyan", thisbtn.GetComponent<Button>());
			}
			
			else if (thisbtn.name.ToLower().Contains("green"))
			{
				arrows.Add("green", thisbtn.GetComponent<Button>());
			}
			
			else if (thisbtn.name.ToLower().Contains("red"))
			{
				arrows.Add("red", thisbtn.GetComponent<Button>());
			}
			
			else if (thisbtn.name.ToLower().Contains("pink"))
			{
				arrows.Add("pink", thisbtn.GetComponent<Button>());
			}
			else
			{
				print("not a valid button");
			}
			thisbtn.gameObject.SetActive(false);
		}

	}
}
