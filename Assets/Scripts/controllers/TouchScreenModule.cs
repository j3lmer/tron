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
	Dictionary<string, Vector3> rotations;
	List<Speler> players;



	private void Start()
	{
		touchscreeners = GameObject.Find("touchscreen");
		getArrows();
		getLocations();
		getPlayers();
		getColors();

		//for(var i=0; i<players.Count; i++)
		//{
		//	makeButtonSet(colors[i], new Vector3(), players[i]);
		//}


		makeButtonSet("green", new Vector3(), players[0]); 
	}


	void makeButtonSet(string color, Vector3 location/*, string rotation,*/ ,Speler player)
	{

		var thiscomponent = new GameObject();
		thiscomponent.name = color;
		thiscomponent.transform.SetParent(touchscreeners.transform);


		var left = location.x - 10;
		var right = location.x + 10;
		var down = location.y - 10;


		var lowerLeft = location + new Vector3(left, down, 0);
		var lowerRight = location + new Vector3(right, down, 0);
		var lower = location + new Vector3(0, down, 0);



		var t = makeButton(color,location, thiscomponent.transform);
		t.onClick.AddListener(delegate {
			if(player.lastdir != Vector3.down)
			{
				player.directionChanger(Vector3.up);
			}
		});



		var ll = makeButton(color, lowerLeft, thiscomponent.transform);
		ll.transform.Rotate(0, 0, 90);
		ll.onClick.AddListener(delegate {
			if(player.lastdir != Vector3.right)
			{
				player.directionChanger(Vector3.left);
			}
		});


		var lr = makeButton(color, lowerRight, thiscomponent.transform);
		lr.transform.Rotate(0, 0, -90);
		lr.onClick.AddListener(delegate {
			if(player.lastdir != Vector3.left)
			{
				player.directionChanger(Vector3.right);
			}
		});


		var d = makeButton(color, lower, thiscomponent.transform);
		d.transform.Rotate(0 , 0, 180);
		d.onClick.AddListener(delegate {
			if(player.lastdir != Vector3.up)
			{
				player.directionChanger(Vector3.down);
			}
		});

	}


	Button makeButton(string color, Vector3 location, Transform parent)
	{
		var selected = arrows[color];
		Button button = Instantiate(selected, location, Quaternion.identity, touchscreeners.transform);
		button.gameObject.SetActive(true);
		button.transform.SetParent(parent);
		return button;
	}

	void getArrows()
	{
		arrows = new Dictionary<string, Button>();


		var temp = GameObject.Find("template");

		for(var i=0; i<temp.transform.childCount; i++)
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
		locations.Add(new Vector3(100, -70, 0));
		locations.Add(new Vector3(-100,-70,0));
	}

	void getRotations()
	{

	}

	void getPlayers()
	{
		players = new List<Speler>();

		players.AddRange(FindObjectsOfType<Speler>());
		print(players.Count);
	}

	void getColors()
	{
		colors = new List<string>();
		colors.Add("pink");
		colors.Add("cyan");
		colors.Add("green");
		colors.Add("yellow");
	}
}
