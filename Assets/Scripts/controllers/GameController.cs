using System.Collections.Generic;
using UnityEngine;

namespace controllers
{
    public class GameController : MonoBehaviour
    {
  
        //PLAYERPREFS
        private int _pvp;
        private int _contenders;

        //PREPARED PLAYER
        GameObject _tempPlayer;

        //STARTPOSITIONS
        Dictionary<int, Vector3> _startpos;
        Dictionary<Vector3, Vector3> _linked;

        //STARTDIRECTION
        Vector3 _dir;

        //WALLPREFABS
        List<GameObject> _wallPrefabs;





        void Start()
        {
            PlayerPrefs.SetInt("AlivePlayers", 0);
                
            _tempPlayer = getRemoveTempPlayer();

            _startpos = getStartPositions();

            _linked = linkedStartPositions();

            _wallPrefabs = getWallPrefabs();

            GetPlayerPrefs();

            setupPlayers();

            addComponents();

            checkTouchScreen();
        }

        void addComponents()
        {
            gameObject.AddComponent<Timer>();

            gameObject.AddComponent<PowerupPlacer>();

            //gameObject.AddComponent<loadFinal>();

            if (_pvp == 0)
            {
                gameObject.AddComponent<NavController>();
            }
        }

        void checkTouchScreen()
        {
            var tmode = PlayerPrefs.GetInt("Touch");

            //print($"Touchscreen mode :{tmode},\n 1 means adding touchscreen");

            switch (tmode)
            {
                case 0:
                    var t =  GameObject.Find("template");
                    for(var i=0; i<t.transform.childCount; i++)
                    {
                        var thistemp = t.transform.GetChild(i);
                        thistemp.gameObject.SetActive(false);
                    }

                    break;

                case 1:
                    //Debug.Log("adding touchscreen module");
                    gameObject.AddComponent<TouchScreenModule>();
                    break;
            }
        }



        void setupPlayers()
        {        
            switch (_pvp)
            {
                case 1: //PVP MODE
                    for (var i = 0; i < _contenders; i++)
                    {                    
                        makePlayer("Player", i);
                        PlayerPrefs.SetInt("AlivePlayers", PlayerPrefs.GetInt("AlivePlayers")+1);
                        //print(PlayerPrefs.GetInt("AlivePlayers"));
                    }
                    break;

                case 0: //PVE MODE
                    for (var i = 0; i < _contenders; i++)
                    {
                        if (i == 0)
                        {
                            makePlayer("Player", i);
                        }
                        else
                        {
                            makePlayer("Bot", i);
                        }
                        PlayerPrefs.SetInt("AlivePlayers", PlayerPrefs.GetInt("AlivePlayers") + 1);
                        //print(PlayerPrefs.GetInt("AlivePlayers"));
                    }
                    break;
            }

            PlayerPrefs.SetInt("placedPlayers", 1);
        }


        GameObject getRemoveTempPlayer()
        {
            var player = GameObject.FindGameObjectWithTag("playerTemp");
            return player;
        }

        void makePlayer(string tag, int i)
        {
            //print($"Making player {tag}, number {i}");

            Vector3 thisStartpos = _startpos[i];
            _dir = _linked[thisStartpos];

            var player = Instantiate(_tempPlayer);
            player.SetActive(true);


            player.tag = tag;
            player.transform.position = thisStartpos;


            Speler speler = player.AddComponent<Speler>();
            speler.wallprefab = _wallPrefabs[i];
            speler.directionChanger(_dir);

            if (tag == "Player")
            {
                SpelerController controller = player.AddComponent<SpelerController>();
                player.name = $"Speler {i+1}";
                var thiskeyset = Controls()[i];
                controller.SetKeyCodes(thiskeyset);
            }
            else
            {
                player.AddComponent<BotController>();
                player.name = $"Bot {i+1}";
            }

        }

        Dictionary<Vector3, Vector3> linkedStartPositions()
        {
            Dictionary<Vector3, Vector3> startPositions = new Dictionary<Vector3, Vector3>();

            startPositions.Add(new Vector3(-60.5f, 52.2f, 0), Vector3.right);
            startPositions.Add(new Vector3(65.687f, 52.23f, 0), Vector3.down);
            startPositions.Add(new Vector3(65.639f, -56.006f, 0), Vector3.left);
            startPositions.Add(new Vector3(-60.49f, -55.95f, 0), Vector3.up);

            return startPositions;
        }

        Dictionary<int, Vector3> getStartPositions()
        {
            Dictionary<int, Vector3> startPositions = new Dictionary<int, Vector3>();

            startPositions.Add(0,new Vector3(-60.5f, 52.2f, 0));
            startPositions.Add(1,new Vector3(65.687f, 52.23f, 0));
            startPositions.Add(2,new Vector3(-60.49f, -55.95f, 0));
            startPositions.Add(3,new Vector3(65.639f, -56.006f, 0));

            return startPositions;
        }

        List<GameObject> getWallPrefabs()
        {
            List<GameObject> prefabs = new List<GameObject>();

            prefabs.Add(Resources.Load<GameObject>("Prefabs/lightwall_pink"));
            prefabs.Add(Resources.Load<GameObject>("Prefabs/lightwall_cyan"));
            prefabs.Add(Resources.Load<GameObject>("Prefabs/lightwall_yellow"));
            prefabs.Add(Resources.Load<GameObject>("Prefabs/lightwall_green"));
            return prefabs;
        }

        List<KeyCode[]> Controls()
        {
            //returns a list of key presets
            KeyCode[] keyset1 = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
            KeyCode[] keyset2 = new KeyCode[] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };
            KeyCode[] keyset3 = new KeyCode[] { KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L };
            KeyCode[] keyset4 = new KeyCode[] { KeyCode.Keypad8, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6 };

            List<KeyCode[]> keysets = new List<KeyCode[]>();
            keysets.Add(keyset1);
            keysets.Add(keyset2);
            keysets.Add(keyset3);
            keysets.Add(keyset4);

            return keysets;
        }

        void GetPlayerPrefs()
        {
            //check if there even is a pvp variable
            if (!PlayerPrefs.HasKey("PVP") | !PlayerPrefs.HasKey("contestants"))
            {
                print("going to default mode. either no PVP or contestants value");

                PlayerPrefs.SetInt("PVP", 1);
                PlayerPrefs.SetInt("contestants", 2);

                _pvp = 1;
                _contenders = 2;
            }
            else
            {
                _pvp = PlayerPrefs.GetInt("PVP");
                _contenders = PlayerPrefs.GetInt("contestants");
                print($"PVP = {_pvp}, CONTENDERS = {_contenders}");
            }
        }
    }
}

