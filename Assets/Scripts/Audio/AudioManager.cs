using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, s => s.name == name);
        if (s != null)
        {
            s.source.Play();
        }
    }

    public void StopAllMusic()
    {
        foreach (Sound s in sounds)
        {
            if(s != null)
            {
                s.source.Stop();
            }
        }
    }

    public void PauseAllMusic()
    {
        foreach (Sound s in sounds)
        {
            if (s != null)
            {
                s.source.Pause();
            }
        }
    }

    public void UnPauseAllMusic()
    {
        foreach (Sound s in sounds)
        {
            if (s != null)
            {
                s.source.UnPause();
            }
        }
    }
}
