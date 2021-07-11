using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	public AudioSource SoundSource;

	public static SoundManager Instance = null;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		}
		else if (Instance != this) {
			Destroy(gameObject);
		}
	}

	public void Play(AudioClip clip, float volume = 1) {
		SoundSource.clip = clip;
		SoundSource.volume = volume;
		SoundSource.Play();
	}
}
