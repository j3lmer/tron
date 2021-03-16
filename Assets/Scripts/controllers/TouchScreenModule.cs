using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TouchScreenModule : MonoBehaviour
{
	Dictionary<string, Button> arrows;
	List<Vector3> locations;
	GameObject touchscreeners;

	private void Start()
	{
		touchscreeners = GameObject.Find("touchscreen");
		getArrows();
		getLocations();

		makeButtonSet("green", new Vector3(0,0,0)); 
	}


	void makeButtonSet(string color, Vector3 location)
	{
		makeButton(color,location, Quaternion.identity);
	}


	void makeButton(string color, Vector3 location, Quaternion rotation)
	{
		var selected = arrows[color];
		Button button = Instantiate(selected, location, rotation, touchscreeners.transform);
		button.gameObject.SetActive(true);
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

	void getLocations()
	{
		locations = new List<Vector3>();
		locations.Add(Vector3.up);
		locations.Add(Vector3.down);
		locations.Add(Vector3.left);
		locations.Add(Vector3.right);
	}
}
