using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace finalScreen
{
	public class loadFinal : MonoBehaviour
	{
		string _winningName;

		bool[] _bools;


		private void Start()
		{
			CheckforFinal();
		}

		private void Update()
		{
			var alivePlayers = FindObjectsOfType<Speler>();
		
			_bools = alivePlayers.Select(s => s.Alive).ToArray();

			print(_bools.Length);
		}


		async void CheckforFinal()
		{	
		
			while(_bools.Length > 2)
			{
				print("routining");

				await new WaitForEndOfFrame();
			}

			NavMesh.RemoveAllNavMeshData();

			if(_bools.Length == 1)
			{
				_winningName = "Gelijkspel!";
				SetWinner();
			}
			else
			{			
				var player = GameObject.FindGameObjectWithTag("Player");
				var bot = GameObject.FindGameObjectWithTag("Bot");

				if (player)
				{
					_winningName = player.name;
				}

				if (bot)
				{
					_winningName = bot.name;
				}
				SetWinner();
			}
		
		
		}


		void SetWinner()
		{
			PlayerPrefs.SetString("winner", _winningName);		
			PlayerPrefs.SetInt("placedPlayers", 0);
			PlayerPrefs.SetInt("controls", 0);
			var cc = PlayerPrefs.GetInt(_winningName + "CollectedCoins");
			PlayerPrefs.SetFloat("winnercoins", cc);

			foreach(Speler participant in FindObjectsOfType<Speler>())
			{
				PlayerPrefs.SetInt(participant.name + "CollectedCoins", 0);
			}


			if (sm.Instance != null)
			{
				if (sm.Instance.MusicSource.isPlaying)
				{
					sm.Instance.MusicSource.Stop();
				}
			}

		
			SceneManager.LoadScene("finalScreen");

		}
	}
}


