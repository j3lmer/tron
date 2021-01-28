using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
	/*
	 * haal variabelen op, pvp/pve, hoeveel bots/spelers (wie heeft wat voor kleuren)
	 * maak nieuwe spelers aan volgens deze variabelen
	 * zet ze op de juiste plek en geef ze de juiste initiele velocity mee
	 */

	public GameObject pinkwall;
	public GameObject cyanwall;
	public GameObject yellowwall;
	public GameObject greenwall;


	void Start()
	{
		List<int> values = getPlayerPrefs();

		List<Vector3> startPositions = getStartPositions();

		GameObject player = getPlayerObject();

		useVariables(values, startPositions, player);

		List<GameObject> wallP = getWalls();
		setWalls(wallP);

		PlayerPrefs.SetInt("placedPlayers", 1);
		PlayerPrefs.SetInt("Controls", 0);
	}

	List<int> getPlayerPrefs()
	{
		int PVP = 0;
		int cont = 0;
		//check if there even is a pvp variable
		if (!PlayerPrefs.HasKey("PVP") | !PlayerPrefs.HasKey("contestants"))
		{
			print("going to default mode. either no PVP or contestants value");

			PVP = 1;
			cont = 2;
		}
		else
		{
			PVP = PlayerPrefs.GetInt("PVP");
			cont = PlayerPrefs.GetInt("contestants");		
		}

		var list = new List<int>();
		list.Add(PVP);
		list.Add(cont);
		return list;
	}

	List<Vector3> getStartPositions()
	{
		List<Vector3> startPositions = new List<Vector3>();
		startPositions.Add(new Vector3(-60.5f, 52.2f, 0));
		startPositions.Add(new Vector3(65.687f, 52.23f, 0));
		startPositions.Add(new Vector3(65.639f, -56.006f, 0));
		startPositions.Add(new Vector3(-60.49f, -55.95f, 0));		

		return startPositions;
	}


	GameObject getPlayerObject()
	{
		GameObject player = GameObject.FindGameObjectWithTag("playerTemp");
		player.SetActive(false);
		return player;
	}

	void useVariables(List<int> values, List<Vector3> startPos, GameObject player)
	{
		//values[0] = pvp.
		//values[1] = (amount of) contestants

		//startpos in order = linksboven, rechtsboven, linksonder, rechtsonder.

		int pvp = values[0];
		int cont = values[1];
		List<string> names = new List<string>();

		switch (pvp)
		{
			case 1:// PVP
				switch (cont)
				{
					case 2:	
						for (var i = 0; i < cont; i++)
						{
							GameObject thisPlayer = Instantiate<GameObject>(player);
							thisPlayer.transform.position = startPos[i];
							thisPlayer.SetActive(true);
							names.Add($"Speler {i+1}");
							thisPlayer.name = names[i];
							thisPlayer.tag = "Player";
							setInitVelocity(thisPlayer, startPos);
						}
						break;

					case 3:
						for (var i = 0; i < cont; i++)
						{
							GameObject thisPlayer = Instantiate<GameObject>(player);
							thisPlayer.transform.position = startPos[i];
							thisPlayer.SetActive(true);
							names.Add($"Speler {i + 1}");
							thisPlayer.name = names[i];
							thisPlayer.tag = "Player";
							setInitVelocity(thisPlayer, startPos);
						}
						break;

					case 4:
						for (var i = 0; i < cont; i++)
						{
							GameObject thisPlayer = Instantiate<GameObject>(player);
							thisPlayer.transform.position = startPos[i];
							thisPlayer.SetActive(true);
							names.Add($"Speler {i + 1}");
							thisPlayer.name = names[i];
							thisPlayer.tag = "Player";
							setInitVelocity(thisPlayer, startPos);

						}
						break;
				}
				break;

			case 0: //PVE
				switch (cont)
				{
					case 2:
						for (var i = 0; i < cont; i++)
						{
							if(i == 0)
							{
								GameObject thisPlayer = Instantiate<GameObject>(player);
								thisPlayer.transform.position = startPos[i];
								thisPlayer.SetActive(true);
								names.Add($"Speler {i + 1}");
								thisPlayer.name = names[i];
								thisPlayer.tag = "Player";
								setInitVelocity(thisPlayer, startPos);
							}
							else
							{
								GameObject thisBot = Instantiate<GameObject>(player);
								thisBot.transform.position = startPos[i];
								thisBot.SetActive(true);
								names.Add($"Bot {i + 1}");
								thisBot.name = names[i];
								thisBot.tag = "Player";
								setInitVelocity(thisBot, startPos);
							}
						}
						break;
					case 3:
						for (var i = 0; i < cont; i++)
						{
							if (i == 0)
							{
								GameObject thisPlayer = Instantiate<GameObject>(player);
								thisPlayer.transform.position = startPos[i];
								thisPlayer.SetActive(true);
								names.Add($"Speler {i + 1}");
								thisPlayer.name = names[i];
								thisPlayer.tag = "Player";
								setInitVelocity(thisPlayer, startPos);
							}
							else
							{
								GameObject thisBot = Instantiate<GameObject>(player);
								thisBot.transform.position = startPos[i];
								thisBot.SetActive(true);
								names.Add($"Bot {i + 1}");
								thisBot.name = names[i];
								thisBot.tag = "Player";
								setInitVelocity(thisBot, startPos);
							}
						}
						break;
					case 4:
						for (var i = 0; i < cont; i++)
						{
							if (i == 0)
							{
								GameObject thisPlayer = Instantiate<GameObject>(player);
								thisPlayer.transform.position = startPos[i];
								thisPlayer.SetActive(true);
								names.Add($"Speler {i + 1}");
								thisPlayer.name = names[i];
								thisPlayer.tag = "Player";
								setInitVelocity(thisPlayer, startPos);
							}
							else
							{
								GameObject thisBot = Instantiate<GameObject>(player);
								thisBot.transform.position = startPos[i];
								thisBot.SetActive(true);
								names.Add($"Bot {i + 1}");
								thisBot.name = names[i];
								thisBot.tag = "Player";
								setInitVelocity(thisBot, startPos);
							}
						}
						break;
				}
				break;

			default://pvp, 2 contestants
				pvp = 1;
				cont = 2;
				for (var i = 0; i < cont; i++)
				{
					GameObject thisPlayer = Instantiate<GameObject>(player);
					thisPlayer.transform.position = startPos[i];
					thisPlayer.SetActive(true);
					names.Add($"Speler {i + 1}");
					thisPlayer.name = names[i];
					thisPlayer.tag = "Player";
					setInitVelocity(thisPlayer, startPos);
				}
				break;
		}

		GameObject[] pMAdder = GameObject.FindGameObjectsWithTag("Player");
		for (var j = 0; j < pMAdder.Length; j++)
		{
			var thisplayer = pMAdder[j];
			//var test = thisplayer.AddComponent<color>();
			switch (pvp)
			{
				case 1://PVP
					thisplayer.AddComponent<Beweging>();
					break;

				case 0://PVE
					if(thisplayer == pMAdder[0])
					{
						thisplayer.AddComponent<Beweging>();
					}
					else
					{
						//thisplayer.AddComponent<botBeweging>(); //UNCOMMENT WHEN NESSECARY
						print("hier moet je het botbeweging script aan toevoegen");
					}
					break;
			}
		}
	}
	
	void setInitVelocity(GameObject player, List<Vector3> startPos)
	{
		//player = the just instansiated + positioned player of tag Player
		Vector3 pos = player.transform.position;

		List<Vector3> dir = returnDir();		


		for(var i=0; i<startPos.Count; i++)
		{
			var thisSPos = startPos[i];
			if(pos == thisSPos)
			{
				//set velocity to dir[i]
				player.GetComponent<Rigidbody2D>().velocity = dir[i] * 16f;
			}
		}
	}

	List<GameObject> getWalls()
	{
		List<GameObject> walls = new List<GameObject>();
		walls.Add(pinkwall);
		walls.Add(cyanwall);
		walls.Add(yellowwall);
		walls.Add(greenwall);
		return walls;
	}
	void setWalls(List<GameObject> walls)
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		for(var i = 0; i< players.Length; i++)
		{
			var thisplayer = players[i];
			//MOGELIJK LATER DIT VERANDEREN ALS JE JE EIGEN KLEUR MOET KIEZEN
			var beweeg = thisplayer.GetComponent<Beweging>();
			beweeg.wallprefab = walls[i];
		}
	}

	List<Vector3> returnDir()
	{
		List<Vector3> dir = new List<Vector3>();
		dir.Add(Vector3.right);
		dir.Add(Vector3.down);
		dir.Add(Vector3.left);
		dir.Add(Vector3.up);
		return dir;
	}
	

}
