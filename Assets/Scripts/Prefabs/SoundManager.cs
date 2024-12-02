using UnityEngine;

// This code attached to the SoundManager prefab object
public class SoundManager : MonoBehaviour {

    // Public properties
    public static SoundManager sm;
    public AudioSource fxSource;
    public AudioSource musicSource;
    public AudioSource extraSource;
    public float sfxVolume = 0.2f;
    public float musicVolume = 0.1f;

	// Use this for initialization
	void Start () {

	}

    // While SoundManager is running don't destroy static ref
    void Awake()
    {
        if (sm == null)
        {
            DontDestroyOnLoad(gameObject);
            sm = this;
        }
        else if (sm != this)
        {
            Destroy(gameObject);
        }
    }

    // Play sound effect once
    public void PlaySoundFX(AudioClip clip)
    {
        float pitch = 1;
        extraSource.pitch = pitch;
        extraSource.volume = sfxVolume;
        extraSource.clip = clip;
        extraSource.Play();
    }

    // Play sound effect for attacks with pitch adjustments
    public void PlaySoundFX(AudioClip clip, bool weak, bool resist, bool crit)
    {
        // Set pitch based on type of attack
        float pitch = 1;
        if (crit)
            pitch = 2.0f;
        else if (weak)
            pitch = 1.25f;
        else if (resist)
            pitch = 0.75f;
        // Adjust pitch and volume
        fxSource.pitch = pitch;
        fxSource.volume = sfxVolume;
        fxSource.clip = clip;
        fxSource.Play();
    }

    // Play looping music
    public void PlayMusic(AudioClip clip)
    {
        musicSource.volume = musicVolume;
        musicSource.clip = clip;
        musicSource.Play();
    }

    // Stop music
    public void StopMusic()
    {
        musicSource.Stop();
    }
}
