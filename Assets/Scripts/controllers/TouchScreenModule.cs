using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace controllers
{
	public class TouchScreenModule : MonoBehaviour
	{
		GameObject _touchscreeners;
		Dictionary<string, Button> _arrows;
		List<string> _colors;
		List<Vector3> _locations;
		List<Vector3> _rotations;
		List<Speler> _players;



		private void Start()
		{
			_touchscreeners = GameObject.Find("touchscreen");
			GetArrows();
			GetLocations();
			GetRotations();
			GetPlayers();
			GetColors();

			CreateSets();		
		}

		void CreateSets()
		{
			var index = 0;
			foreach (Speler player in _players)
			{
				//print($"setting Color:{colors[index]} to {locations[index]} and {players[index]}");
				if (!player.GetComponent<BotController>())
				{
					SetUpButtons(_colors[index], _locations[index], _players[index]);
					index += 1;
				}			
			}
		}

		void SetUpButtons(string color, Vector3 location, Speler player)
		{

			var thiscomponent = new GameObject();
			thiscomponent.name = color;
			thiscomponent.transform.SetParent(_touchscreeners.transform);
			thiscomponent.transform.position = location;

			var thisset = MakeButtonSet(thiscomponent, color, location, player);

			var tempObject = new GameObject();

			switch (color)
			{
				case "pink": //roteer rechts (sorry)
					tempObject.transform.parent = _touchscreeners.transform;
					foreach (Button b in thisset)
					{
						b.transform.parent = tempObject.transform;
						tempObject.transform.Rotate(new Vector3(0, 0, -90));
						tempObject.transform.Translate(new Vector3(_locations[0].y, _locations[0].x, 0) + new Vector3(-10,-95,0));
						b.transform.parent = thiscomponent.transform;
					}
					break;

				case "cyan":
					//roteer links (nog meer sorry)
					tempObject = new GameObject();
					tempObject.transform.parent = _touchscreeners.transform;
					foreach (Button b in thisset)
					{
						b.transform.parent = tempObject.transform;
						tempObject.transform.Rotate(new Vector3(0, 0, 90));
						tempObject.transform.Translate(new Vector3(_locations[0].y, _locations[0].x, 0) + new Vector3(-130, -95, 0));
						b.transform.parent = thiscomponent.transform;
					}
					break;

				default:
					//niks
					break;
			}

			Destroy(tempObject);
		}


		Button[] MakeButtonSet(GameObject thiscomponent, string color, Vector3 location, Speler player)
		{



			var t = MakeButton(color, location, thiscomponent.transform);
			t.onClick.AddListener(delegate
			{
				if(color != "pink" && color != "cyan")
				{
					if (player.lastdir != Vector3.down)
					{
						player.directionChanger(Vector3.up);
					}
				}
				else if(color == "pink")
				{
					if (player.lastdir != Vector3.left)
					{
						player.directionChanger(Vector3.right);
					}
				}
				else if(color == "cyan")
				{
					if (player.lastdir != Vector3.right)
					{
						player.directionChanger(Vector3.left);
					}
				}
			
			
			});

			var d = MakeButton(color, location + new Vector3(0, -10, 0), thiscomponent.transform);
			d.transform.Rotate(new Vector3(0, 0, 180));
			d.onClick.AddListener(delegate
			{
				if(color != "pink" && color != "cyan")
				{
					if (player.lastdir != Vector3.up)
					{
						player.directionChanger(Vector3.down);
					}
				}
				else if(color == "pink")
				{
					if (player.lastdir != Vector3.right)
					{
						player.directionChanger(Vector3.left);
					}
				}
				else if(color == "cyan")
				{
					if (player.lastdir != Vector3.left)
					{
						player.directionChanger(Vector3.right);
					}
				}
			
			});

			var l = MakeButton(color, location + new Vector3(-10, -10, 0), thiscomponent.transform);
			l.transform.Rotate(new Vector3(0, 0, 90));
			l.onClick.AddListener(delegate
			{
				if(color != "pink" && color != "cyan")
				{
					if (player.lastdir != Vector3.right)
					{
						player.directionChanger(Vector3.left);
					}
				}
				else if(color == "pink")
				{
					if (player.lastdir != Vector3.down)
					{
						player.directionChanger(Vector3.up);
					}
				}
				else if(color == "cyan")
				{
					if (player.lastdir != Vector3.up)
					{
						player.directionChanger(Vector3.down);
					}
				}
			
			});

			var r = MakeButton(color, location + new Vector3(+10, -10, 0), thiscomponent.transform);
			r.transform.Rotate(new Vector3(0, 0, -90));
			r.onClick.AddListener(delegate
			{
				if(color != "pink" && color != "cyan")
				{
					if (player.lastdir != Vector3.left)
					{
						player.directionChanger(Vector3.right);
					}
				}
				else if(color == "pink")
				{
					if (player.lastdir != Vector3.up)
					{
						player.directionChanger(Vector3.down);
					}
				}
				else if(color == "cyan")
				{
					if (player.lastdir != Vector3.down)
					{
						player.directionChanger(Vector3.up);
					}
				}
			
			});

			Button[] buttons = {t,d,l,r};
			return buttons;
		}	


		Button MakeButton(string color, Vector3 location, Transform parent)
		{
			var selected = _arrows[color];
			Button button = Instantiate(selected, location, Quaternion.identity, parent);
			button.gameObject.SetActive(true);
			return button;
		}




		void GetArrows()
		{
			_arrows = new Dictionary<string, Button>();


			var temp = GameObject.Find("template");

			for (var i = 0; i < temp.transform.childCount; i++)
			{
				var thisbtn = temp.transform.GetChild(i);

				if (thisbtn.name.ToLower().Contains("pink"))
				{
					_arrows.Add("pink", thisbtn.GetComponent<Button>());
				}

				else if (thisbtn.name.ToLower().Contains("cyan"))
				{
					_arrows.Add("cyan", thisbtn.GetComponent<Button>());
				}

				else if (thisbtn.name.ToLower().Contains("red"))
				{
					_arrows.Add("yellow", thisbtn.GetComponent<Button>());
				}

				else if (thisbtn.name.ToLower().Contains("green"))
				{
					_arrows.Add("green", thisbtn.GetComponent<Button>());
				}





				else
				{
					print("not a valid button");
				}
				thisbtn.gameObject.SetActive(false);
			}

		}

		void GetLocations()
		{
			_locations = new List<Vector3>();

			_locations.Add(new Vector3(-100, 70, 0));
			_locations.Add(new Vector3(100, 70, 0));
			_locations.Add(new Vector3(-100, -65, 0));
			_locations.Add(new Vector3(106, -65, 0));
		}

		void GetRotations()
		{
			_rotations = new List<Vector3>();
			_rotations.Add(new Vector3(0, 0, 90));
			_rotations.Add(new Vector3(0, 0, -90));
		}

		void GetPlayers()
		{
			_players = FindObjectsOfType<Speler>().ToList();
			_players.Reverse();
		}

		void GetColors()
		{
			_colors = new List<string>();
			_colors.Add("pink");
			_colors.Add("cyan");
			_colors.Add("yellow");
			_colors.Add("green");
		}
	}
}
