using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

	private Dictionary<string, AudioSource> efxRegistry;
	public AudioSource efxSource;
	public AudioSource musicSource;

	public static SoundManager instance = null;

	public float defaultLowPitchRange;
	public float defaultHighPitchRange;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		efxRegistry = new Dictionary<string, AudioSource> ();
		DontDestroyOnLoad (gameObject);
	}



	public void PlaySingle (AudioClip clip) {
		efxSource.clip = clip;
		efxSource.Play ();
	}

	public void PlaySingleFor(string registrant, AudioClip clip, float lowRange = -1f, float highRange = -1f) {
		if (!efxRegistry.ContainsKey (registrant)) {
			return;
		}
		AudioSource s = efxRegistry[registrant];

		s.clip = clip;
		s.pitch = RandomPitch (lowRange, highRange);
		s.Play();
	}

	public void Register(string registrant) {
		if (efxRegistry.ContainsKey (registrant)) {
			return;
		}
		AudioSource s = gameObject.AddComponent<AudioSource>();
		efxRegistry.Add(registrant, s);
	}
	public AudioSource GetSource(string registrant) {
		if (!efxRegistry.ContainsKey (registrant)) {
			return null;
		}

		AudioSource s = gameObject.AddComponent<AudioSource>();
		return s;
	}

	public void Unregister(string registrant) {
		if (!efxRegistry.ContainsKey (registrant)) {
			return;
		}
		//destroy the source component
		Destroy(efxRegistry[registrant]);
		efxRegistry.Remove(registrant);
	}

	public void RandomizeSfx(params AudioClip [] clips)
	{
		int randomIndex = Random.Range (0, clips.Length);

		efxSource.pitch = RandomPitch();
		efxSource.clip = clips [randomIndex];
		efxSource.Play ();
	}


	public void PlaySourceFor(string registrant, float lowRange = -1f, float highRange = -1f) {
		if (!efxRegistry.ContainsKey (registrant)) {
			return;
		}
		AudioSource s = efxRegistry[registrant];
		s.pitch = RandomPitch (lowRange, highRange);
		s.Play();
	}

	public void SetClipFor(string registrant, AudioClip c) {
		if (!efxRegistry.ContainsKey (registrant)) {
			return;
		}
		AudioSource s = efxRegistry[registrant];
		s.clip = c;
	}
	public void SetVolumeFor(string registrant, float volume) {
		if (!efxRegistry.ContainsKey (registrant)) {
			return;
		}
		AudioSource s = efxRegistry[registrant];
		s.volume = volume;
	}

	public float RandomPitch(float lowRange = -1f, float highRange = -1f)
	{

		if (lowRange == -1f && highRange == -1f) {
			lowRange = defaultLowPitchRange;
			highRange = defaultHighPitchRange;
		}
		return  Random.Range (lowRange, highRange);
	}


}
