using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
	public TextMeshProUGUI timer;

	sm sm;

	///audioclips
	public AudioClip three;
	public AudioClip two;
	public AudioClip one;
	public AudioClip zero;
	public AudioClip gameplayMusic;

    private void Start()
    {
		loadClips();
		sm = sm.Instance;
		timer = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();

		if(sm != null)
        {
			startTimer();
		}
		timer.text = "";
		enabled = false;
	}

	async void startTimer()
    {
		Time.timeScale = 0f;
		timer.text = "3";
		timer.color = Color.cyan;
		sm.Play(three);

		await new WaitForSecondsRealtime(1);
		timer.text = "2";
		timer.color = Color.red;
		sm.Play(two);

		await new WaitForSecondsRealtime(1);
		timer.text = "1";
		timer.color = Color.yellow;
		sm.Play(one);

		await new WaitForSecondsRealtime(1);
		timer.text = "GO!";
		timer.color = Color.green;
		Time.timeScale = 1f;
		sm.Play(zero);

		await new WaitForSecondsRealtime(1);
		sm.EffectsSource.clip = null;
		sm.PlayMusic(gameplayMusic);		
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
