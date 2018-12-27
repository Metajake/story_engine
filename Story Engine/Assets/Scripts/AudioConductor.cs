using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioConductor : MonoBehaviour {
    public AudioClip subwayCar;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadAndPlay(AudioClip clipName)
    {
        GetComponent<AudioSource>().clip = clipName;
        GetComponent<AudioSource>().Play();
    }
}
