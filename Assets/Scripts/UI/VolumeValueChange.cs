using UnityEngine;
using UnityEngine.UI;

public class VolumeValueChange : MonoBehaviour {

    // Reference to Audio Source component
    private AudioSource audioSrc;

    // Music volume variable that will be modified
    // by dragging slider knob
    private float musicVolume = 1f;


	// Use this for initialization
	void Start () {

        // Assign Audio Source component to control it
        audioSrc = sm.Instance.MusicSource;        
	}

	
	// Update is called once per frame
	void Update () {

        // Setting volume option of Audio Source to be equal to musicVolume
        audioSrc.volume = musicVolume;
	}

    // Method that is called by slider game object
    // This method takes vol value passed by slider
    // and sets it as musicValue
    public void SetVolume(float vol)
    {
        musicVolume = vol;
    }
}

//public class DontDestroySlider : MonoBehaviour
//{

//    public Slider slider;

//	public DontDestroySlider(Slider Slider)
//	{
//        slider = Slider;
//        slider.onValueChanged.AddListener(delegate { setSetter(slider); });

//	}

//    void setSetter(Slider slider)
//	{
//        GameObject.Find("");
//	}

    
//}
