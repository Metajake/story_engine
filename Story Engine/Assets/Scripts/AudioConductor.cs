using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioConductor : MonoBehaviour {
    public AudioClip subwayCar;
    public AudioClip dateMusic;

    public AudioSource musicAudioSource;
    
    private bool isFadingIn;

    private bool isFadingOut;

    public  float VOLUME_FADE_IN_INCREMENT = 0.1f;
    public float VOLUME_FADE_OUT_INCREMENT = 0.4f;
    
    private AudioSource sfxAudioSource;

    void Start () {
			sfxAudioSource = GetComponent<AudioSource>();
			this.musicAudioSource.volume = 0;
	}
	
	void Update ()
	{
		if (!checkValid()) return;

		if (isFadingIn)
		{
			musicAudioSource.volume = Mathf.Min(1.0f, musicAudioSource.volume + VOLUME_FADE_IN_INCREMENT * Time.deltaTime);
		}
		else if (isFadingOut)
		{
			musicAudioSource.volume = Mathf.Max(0.0f, musicAudioSource.volume - VOLUME_FADE_OUT_INCREMENT * Time.deltaTime);
		}

		checkStopFade();
	}

	private void checkStopFade()
	{
		if (musicAudioSource.volume >= 0.999f || musicAudioSource.volume <= 0.001f)
		{
			isFadingIn = false;
			isFadingOut = false;
		}
	}

	private bool checkValid()
	{
		if (isFadingIn && isFadingOut)
		{
			Debug.Log("Connor you broke the audio it's fading in and out at once which shouldn't be a thing really");
			return false;
		}

		return true;
	}

	public bool isFading()
	{
		return !(this.isFadingIn || this.isFadingOut);
	}

	public void loadAndPlay(AudioClip clipName)
    {
        sfxAudioSource.clip = clipName;
        sfxAudioSource.Play();
    }

    public void startMusic(AudioClip clip)
    {
	    musicAudioSource.clip = clip;
	    musicAudioSource.Play();
	    fadeInCurrentMusic();
    }

    public void fadeOutCurrentMusic()
    {
	    this.isFadingIn = false;
	    this.isFadingOut = true;
    }

    public void fadeInCurrentMusic()
    {
	    this.isFadingOut = false;
	    this.isFadingIn = true;
    }
}
