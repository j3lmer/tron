using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
	public TextMeshProUGUI timer;

	sm _sm;

	///audioclips
	public AudioClip three;
	public AudioClip two;
	public AudioClip one;
	public AudioClip zero;
	public AudioClip gameplayMusic;

    private void Start()
    {
		loadClips();
		_sm = sm.Instance;
		timer = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();

		if(_sm != null)
        {
			startTimer();
        }
        else
        {
			timer.text = "";
			enabled = false;
		}		
	}

	async void startTimer()
    {
		Time.timeScale = 0f;
		timer.text = "3";
		timer.color = Color.cyan;
		_sm.Play(three);

		await new WaitForSecondsRealtime(1);
		timer.text = "2";
		timer.color = Color.red;
		_sm.Play(two);

		await new WaitForSecondsRealtime(1);
		timer.text = "1";
		timer.color = Color.yellow;
		_sm.Play(one);

		await new WaitForSecondsRealtime(1);
		timer.text = "GO!";
		timer.color = Color.green;
		Time.timeScale = 1f;
		_sm.Play(zero);

		await new WaitForSecondsRealtime(1);
		_sm.EffectsSource.clip = null;
		_sm.PlayMusic(gameplayMusic);
		timer.text = "";

		enabled = false;		
	}


	void loadClips()
    {
		three = Resources.Load<AudioClip>("music/countdown/three");
		two = Resources.Load<AudioClip>("music/countdown/two");
		one = Resources.Load<AudioClip>("music/countdown/one");
		zero = Resources.Load<AudioClip>("music/countdown/zero");
		gameplayMusic = Resources.Load<AudioClip>("music/tron-style-music-original-track");
    }
}
