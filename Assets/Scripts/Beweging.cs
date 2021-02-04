using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Beweging : MonoBehaviour
{
	///keycodes voor deze speler
		public KeyCode upKey;
		public KeyCode downKey;
		public KeyCode rightKey;
		public KeyCode leftKey;
	///einde keycodes
	

	///Speler variables	
		//snelheid variabel
		public float speed;

		//rigidbody deze speler
		Rigidbody2D rb;

		//speler op dit moment ondoodbaar variabel
		bool Invincible;	

		//laatste directie op bewogen
		Vector3 lastDirection;
	///end speler variables


	///wall variables
		//wall prefab
		public GameObject wallprefab;

		//the wall that is currently being dragged along by the player
		Collider2D wall;

		//laatste stukje muur
		Vector2 lastWallEnd;
	///end wall variables



	// Start is called before the first frame update
	void Start()
	{
		//init default speed naar 16
		speed = 16;

		//haal lijst met controls op en zet de juiste bij deze speler
		List<KeyCode[]> controls = Controls();
		setControls(controls);

		//zet lastdirection naar de initieel opbewogen directie-vector
		initDirection();

		//begin met het spawnen vam een muur
		spawnWall();
	}

	void initDirection()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		Vector3 vDir = rb.transform.InverseTransformDirection(rb.velocity);

		if(vDir.x > 0)
		{
			//rechts
			//print($"{rb.name} vdir.x groter = {vDir.x}");
			lastDirection = Vector3.right;
		}
		else if(vDir.x < 0)
		{
			//links
			//print($"{rb.name} vdir.x groter = {vDir.x}");
			lastDirection = Vector3.left;
		}
		else if(vDir.y > 0) 
		{
			//omhoog
			//print($"{rb.name} vdir.y groter = {vDir.y}");
			lastDirection = Vector3.up;
		}
		else if(vDir.y < 0) 
		{
			//beneden
			//print($"{rb.name} vdir.y kleiner = {vDir.y}");
			lastDirection = Vector3.down;
		}
	}

	void FixedUpdate()
	{
		//checken naar keyboard inputs
		checkInputs();		
	}

	void checkInputs()
	{
		//check of er een keyboard input van de lokale keyset word gedaan
		//als deze input niet het tegenovergestelde is van lastdirection, laat deze speler dan in deze richting bewegen
		
		if (Input.GetKey(upKey))
		{
			if(lastDirection != Vector3.down)
			{
				directionChanger(Vector3.up);
				spawnWall();
			}			
		}
		else if (Input.GetKey(leftKey))
		{
			if(lastDirection != Vector3.right)
			{
				directionChanger(Vector3.left);
				spawnWall();
			}			
		}
		else if (Input.GetKey(downKey))
		{
			if(lastDirection != Vector3.up)
			{
				directionChanger(Vector3.down);
				spawnWall();
			}			
		}		
		else if (Input.GetKey(rightKey))
		{
			if(lastDirection != Vector3.left)
			{
				directionChanger(Vector3.right);
				spawnWall();
			}		
		}

		//reken uit en zet neer waar de collider moet zijn (tussen hier en het einde van de laatste muur)
		fitColliderBetween(wall, lastWallEnd, transform.position);
	}

	void directionChanger(Vector3 direction)
	{
		//code om van richting te veranderen, richting word veranderd naar de aangegeven richting
		rb.velocity = direction * speed;
		lastDirection = direction;
	}

	List<KeyCode[]> Controls()
	{
		//returns a list of key presets
		KeyCode[] keyset1 = new KeyCode[] {KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D};
		KeyCode[] keyset2 = new KeyCode[] {KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow};
		KeyCode[] keyset3 = new KeyCode[] {KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L};
		KeyCode[] keyset4 = new KeyCode[] {KeyCode.Keypad8, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6};

		List<KeyCode[]> keysets = new List<KeyCode[]>();
		keysets.Add(keyset1);
		keysets.Add(keyset2);
		keysets.Add(keyset3);
		keysets.Add(keyset4);

		return keysets;
	}

	void setControls(List<KeyCode[]> controls)
	{
		var contInt = PlayerPrefs.GetInt("Controls");
		KeyCode[] thisKeyset = controls[0];

		switch (contInt)
		{
			case 1:
				thisKeyset = controls[1];
				break;

			case 2:
				thisKeyset = controls[2];
				break;

			case 3:
				thisKeyset = controls[3];
				break;
		}
	
		upKey = thisKeyset[0];
		leftKey = thisKeyset[1];
		downKey = thisKeyset[2];
		rightKey = thisKeyset[3];
		PlayerPrefs.SetInt("Controls", contInt + 1);
	}

	void spawnWall()
	{
		//sla laatste muur positie op
		lastWallEnd = transform.position;

		//spawn een nieuwe muur
		GameObject w = Instantiate(wallprefab, transform.position, Quaternion.identity);
		wall = w.GetComponent<Collider2D>();
		w.tag = "playerWall";
	}

	void fitColliderBetween(Collider2D co, Vector2 a, Vector2 b)
	{
		// reken het midden uit
		co.transform.position = a + (b - a) * 0.5f;

		// trek hem (horizontaal of verticaal) uit elkaar
		float dist = Vector2.Distance(a, b);
		if (a.x != b.x)
			co.transform.localScale = new Vector2(dist + 1f, 1f);
		else
			co.transform.localScale = new Vector2(1f, dist + 1f);
	}

	void OnTriggerEnter2D(Collider2D co)
	{
		//print(co.name);
		// Not the current wall(die je achter je aan trekt)?
		if (co != wall)
		{
			//als het de tag "Powerup" draagt
			if(co.tag == "Powerup")
			{
				//vernietig de powerup
				Destroy(co.gameObject);

				//check de naam voor de type powerup
				switch (co.name)
				{
					case "SpeedBoost":
						speedboost();
						break;

					case "Invincible":
						setInvincible();
							break;					

					case "stopPlayer":
						stopRandomPlayer();
						break;

					case "poison":
						killPlayer();
						break;

					case "RemoveWalls":
						removeWalls();
						break;
				}
			}
			//als het niet de tag powerup draagt
			else
			{
				//en als de speler niet ondoodbaar is op dit moment
				if (!Invincible)
				{
					//vernietig speler
					print("Player lost: " + name);
					Destroy(gameObject);
				}
				//als de speler wel ondoodbaar is op dit moment
				else
				{
					if(co.tag == "wall")
					{
						print("Player lost: " + name);
						Destroy(gameObject);
					}
				}				
			}			
		}
	}
	

	async void removeWalls()
	{
		//WIP: remove all walls in current scene for fresh start
		foreach(GameObject wall in GameObject.FindGameObjectsWithTag("playerWall"))
		{
			Color color = wall.GetComponent<SpriteRenderer>().color;
			color.a = 0;
			wall.GetComponent<SpriteRenderer>().color = color;
		}
		await new WaitForSeconds(2);
	}

	void killPlayer()
	{
		//destroys this player when hit
		Destroy(gameObject);
	}

	async void stopRandomPlayer()
	{
		//tries to select a random player, if random player is this player, do thisplayer +1, or thisplayer -1
		GameObject thisPlayer = gameObject;

		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

		var random = Random.Range(0, players.Length-1);

		var selectedplayer = players[random];

		if(selectedplayer == thisPlayer)
		{
			for(var i=0; i< players.Length; i++)
			{
				var t = players[i];
				if(t == selectedplayer)
				{
					selectedplayer = players[i + 1];
					if(selectedplayer == null)
					{
						selectedplayer = players[i - 1];
					}
					break;
				}
			}
		}

		var selectedVelocity = selectedplayer.GetComponent<Rigidbody2D>().velocity;
		selectedplayer.GetComponent<Rigidbody2D>().velocity = new Vector3();

		await new WaitForSeconds(2);

		selectedplayer.GetComponent<Rigidbody2D>().velocity = selectedVelocity;
	}

	async void speedboost()
	{
		//temporarily set speed variable higher and call directionchanger so this is actually done
		//then set speed to original variable
		speed = 30f;
		directionChanger(lastDirection);
		await new WaitForSeconds(4);
		speed = 16f;
	}

	async void setInvincible()
	{
		//set bool Invincible to true for four seconds
		Invincible = true;
		await new WaitForSeconds(4);
		Invincible = false;		
	}

	void doInvincible()
	{
		//if Invincible is true, grab all walls and set color to gray
		if (Invincible)
		{
			string wallname = wall.name;
			List<GameObject> allwalls = GameObject.FindGameObjectsWithTag("playerWall").ToList();

			foreach (GameObject wall in allwalls)
			{
				if (wall.name == wallname)
				{
					wall.GetComponent<SpriteRenderer>().color = Color.gray;
				}
			}
		}
		//if not, if the wall color is still gray, set all walls to the original color
		else
		{
			if(wall.GetComponent<SpriteRenderer>().color == Color.gray)
			{
				string wallname = wall.name;
				List<GameObject> allwalls = GameObject.FindGameObjectsWithTag("playerWall").ToList();

				foreach (GameObject wall in allwalls)
				{
					if (wall.name == wallname)
					{
						wall.GetComponent<SpriteRenderer>().color = wallprefab.GetComponent<SpriteRenderer>().color;
					}
				}
			}
		}			
	}

	private void Update()
	{			
		//check if Invincible is true every frame
		doInvincible();	
	}
}
