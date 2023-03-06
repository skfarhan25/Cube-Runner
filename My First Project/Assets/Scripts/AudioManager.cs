using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance;
    [SerializeField] AudioMixerGroup mixer;
    const string MIXER_EFFECT = "LowPassFreq";

    void Awake()
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
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = mixer;
        }
    }

    void Start()
    {
        // mixer.audioMixer.SetFloat(MIXER_EFFECT, 22000);
        Play("Theme");
    }
    
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    // public IEnumerator ChangeFrequency(float startvalue, float endvalue, float duration)
    // {
    //     float time = 0;
    //     while (time < duration)
    //     {
    //         time += Time.deltaTime;
    //         mixer.audioMixer.SetFloat(MIXER_EFFECT, Mathf.Lerp(startvalue, endvalue, time / duration));
    //         yield return null;
    //     }
    // }

    // public void DecreaseLowBass()
    // {
    //     StartCoroutine(ChangeFrequency(22000, 5000, 0.5f));
    //     Debug.Log("Set low pass frequency to 5000");
    // }

    // public void IncreaseLowBass()
    // {
    //     StartCoroutine(ChangeFrequency(5000, 22000, 0.5f));
    //     Debug.Log("Set low pass frequency to 22000");
    // }
}
