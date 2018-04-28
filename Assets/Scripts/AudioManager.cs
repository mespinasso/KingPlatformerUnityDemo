using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioSource fxSource;
	public AudioSource musicSource;

	public static AudioManager instance = null;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}
	
	public void PlaySound (AudioClip clip) {
		fxSource.clip = clip;
		fxSource.Play ();
	}
}
