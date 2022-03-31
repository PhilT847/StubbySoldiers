using UnityEngine.Audio;
using System;
using UnityEngine;

public class audioManagerScript : MonoBehaviour
{
    public static audioManagerScript instance;
    public AudioMixerGroup mixerGroup;
    public Sound[] sounds;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = mixerGroup;
        }
    }

    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        if (s.loop)
        {
            s.source.Play();
        }
        else
        {
            s.source.PlayOneShot(s.clip);
        }
    }

    public void PlayMusicWithIntro(string soundIntro, string soundLoop)
    {
        Sound s = Array.Find(sounds, item => item.name == soundIntro);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        Sound l = Array.Find(sounds, item => item.name == soundLoop);
        if (l == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();
        l.source.PlayDelayed(s.clip.length - 0.25f); //plays looping intro after first part plays
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

}
