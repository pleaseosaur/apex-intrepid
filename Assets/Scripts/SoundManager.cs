using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
    }

    public Sound[] sounds;
    private Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;
            source.volume = s.volume;
            source.pitch = s.pitch;
            audioSources[s.name] = source;
        }
    }

    public void Play(string name)
    {
        if (audioSources.TryGetValue(name, out AudioSource source))
        {
            source.Play();
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }

    public void PlayOneShot(string name)
    {
        if (audioSources.TryGetValue(name, out AudioSource source))
        {
            source.PlayOneShot(source.clip);
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }

    public void Stop(string name)
    {
        if (audioSources.TryGetValue(name, out AudioSource source))
        {
            source.Stop();
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }
}