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

		makeButtonSet("green", new Vector3()); 
	}


	void makeButtonSet(string color, Vector3 location)
	{

		makeButton(color,location);

		var left = location.x	- 10;
		var right = location.x	+ 10;
		var down = location.y	- 10;


		var lowerLeft = location + new Vector3(left, down, 0);
		var lowerRight = location + new Vector3(right, down, 0);
		var lower = location + new Vector3(0, down, 0);

		var ll = makeButton(color, lowerLeft);
		ll.transform.Rotate(0, 0, 90);

		var lr = makeButton(color, lowerRight);
		lr.transform.Rotate(0, 0, -90);

		var d = makeButton(color, lower);
		d.transform.Rotate(0 , 0, 180);

	}


	Button makeButton(string color, Vector3 location)
	{
		var selected = arrows[color];
		Button button = Instantiate(selected, location, Quaternion.identity, touchscreeners.transform);
		button.gameObject.SetActive(true);
		return button;
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

		locations.Add(new Vector3(-100, 70, 0));
		locations.Add(new Vector3(100, 70, 0));
		locations.Add(new Vector3(100, -70, 0));
		locations.Add(new Vector3(-100,-70,0));
	}
}
