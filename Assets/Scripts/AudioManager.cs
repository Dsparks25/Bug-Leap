using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Music
    public AudioClip backgroundMusic;

    // Sound Effects
    public AudioClip jumpSound;

    private AudioSource backgroundMusicSource;
    private AudioSource soundEffectSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        soundEffectSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayBackgroundMusic()
    {
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void PlayJumpSound()
    {
        soundEffectSource.PlayOneShot(jumpSound);
    }

    public void StopMusic()
    {
        backgroundMusicSource.Stop();
    }
}
