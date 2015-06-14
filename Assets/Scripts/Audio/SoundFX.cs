using UnityEngine;

public class SoundFX : MonoBehaviour {
    private AudioSource AudioSource;

    public AudioClip Crit;
    public AudioClip Glance;
    public AudioClip Hit;
    public AudioClip Miss;

    public void Awake() {
        AudioSource = GetComponent<AudioSource>();
    }

    public void PlayMiss() {
        if (AudioSource.isPlaying) {
            Debug.Log("can't double play");
        }
        AudioSource.clip = Miss;
        AudioSource.Play();
    }

    public void PlayGlance() {
        if (AudioSource.isPlaying) {
            Debug.Log("can't double play");
        }
        AudioSource.clip = Glance;
        AudioSource.Play();
    }

    public void PlayHit() {
        if (AudioSource.isPlaying) {
            Debug.Log("can't double play");
        }
        AudioSource.clip = Hit;
        AudioSource.Play();
    }

    public void PlayCrit() {
        if (AudioSource.isPlaying) {
            Debug.Log("can't double play");
        }
        AudioSource.clip = Crit;
        AudioSource.Play();
    }
}