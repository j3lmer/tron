using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TouchScreenModule : MonoBehaviour
{
	Dictionary<string, Button> arrows;
	List<string> colors;
	List<Vector3> locations;
	GameObject touchscreeners;
	List<Vector3> rotations;
	List<Speler> players;
	GameObject set;



	private void Start()
	{
		touchscreeners = GameObject.Find("touchscreen");
		getArrows();
		getLocations();
		getRotations();
		getPlayers();
		getColors();

		createSets();		
	}

	void createSets()
	{
		var index = 0;
		foreach (Speler player in players)
		{
			//print($"setting Color:{colors[index]} to {locations[index]} and {players[index]}");
			setUpButtons(colors[index], locations[index], players[index]);
			index += 1;
		}
	}

	void setUpButtons(string color, Vector3 location, Speler player)
	{

		var thiscomponent = new GameObject();
		thiscomponent.name = color;
		thiscomponent.transform.SetParent(touchscreeners.transform);
		thiscomponent.transform.position = location;

		var thisset = makeButtonSet(thiscomponent, color, location, player);

		switch (color)
		{
			case "pink": //roteer rechts
				var tempObject = new GameObject();
				tempObject.transform.Rotate(rotations[0]);
				foreach (Button b in thisset)
				{
					b.transform.parent = tempObject.transform;
					b.transform.parent = thiscomponent.transform;
				}

				break;

			case "cyan":
				//roteer links
				break;

			default:
				//niks
				break;
		}
	}


	Button[] makeButtonSet(GameObject thiscomponent, string color, Vector3 location, Speler player)
	{
		


		var t = makeButton(color, location, thiscomponent.transform);
		t.onClick.AddListener(delegate
		{
			if (player.lastdir != Vector3.down)
			{
				player.directionChanger(Vector3.up);
			}
		});

		var d = makeButton(color, location + new Vector3(0, -10, 0), thiscomponent.transform);
		d.transform.Rotate(new Vector3(0, 0, 180));
		d.onClick.AddListener(delegate
		{
			if (player.lastdir != Vector3.up)
			{
				player.directionChanger(Vector3.down);
			}
		});

		var l = makeButton(color, location + new Vector3(-10, -10, 0), thiscomponent.transform);
		l.transform.Rotate(new Vector3(0, 0, 90));
		l.onClick.AddListener(delegate
		{
			if (player.lastdir != Vector3.right)
			{
				player.directionChanger(Vector3.left);
			}
		});

		var r = makeButton(color, location + new Vector3(+10, -10, 0), thiscomponent.transform);
		r.transform.Rotate(new Vector3(0, 0, -90));
		r.onClick.AddListener(delegate
		{
			if (player.lastdir != Vector3.left)
			{
				player.directionChanger(Vector3.right);
			}
		});

		Button[] buttons = {t,d,l,r};
		return buttons;
	}	


	Button makeButton(string color, Vector3 location, Transform parent)
	{
		var selected = arrows[color];
		Button button = Instantiate(selected, location, Quaternion.identity, parent);
		button.gameObject.SetActive(true);
		return button;
	}




	void getArrows()
	{
		arrows = new Dictionary<string, Button>();


		var temp = GameObject.Find("template");

		for (var i = 0; i < temp.transform.childCount; i++)
		{
			var thisbtn = temp.transform.GetChild(i);

			if (thisbtn.name.ToLower().Contains("pink"))
			{
				arrows.Add("pink", thisbtn.GetComponent<Button>());
			}

			else if (thisbtn.name.ToLower().Contains("cyan"))
			{
				arrows.Add("cyan", thisbtn.GetComponent<Button>());
			}

			else if (thisbtn.name.ToLower().Contains("red"))
			{
				arrows.Add("yellow", thisbtn.GetComponent<Button>());
			}

			else if (thisbtn.name.ToLower().Contains("green"))
			{
				arrows.Add("green", thisbtn.GetComponent<Button>());
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
		locations.Add(new Vector3(-100, -65, 0));
		locations.Add(new Vector3(106, -65, 0));
	}

	void getRotations()
	{
		rotations = new List<Vector3>();
		rotations.Add(new Vector3(0, 0, 90));
		rotations.Add(new Vector3(0, 0, -90));
	}

	void getPlayers()
	{
		players = FindObjectsOfType<Speler>().ToList();
		players.Reverse();
	}

	void getColors()
	{
		colors = new List<string>();
		colors.Add("pink");
		colors.Add("cyan");
		colors.Add("yellow");
		colors.Add("green");
	}
}
