using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class TestAudio : MonoBehaviour
{
    /*
    USED FOR TESTING, NOT IN GAME
    */
    [SerializeField] AudioMixer mixer;
    float freq;
    const string MIXER_EFFECT = "LowPassFreq";
    void Start()
    {
        // base 22000 Hz
    }

    public IEnumerator ChangeFrequency(float endvalue, float duration)
    {
        float time = 0;
        float startvalue = 22000;
        while (time < duration)
        {
            freq = Mathf.Lerp(startvalue, endvalue, time / duration);
            time += Time.deltaTime;
            mixer.SetFloat(MIXER_EFFECT, freq);
            yield return null;
        }
    }

    public void DecreaseLowBass()
    {
        StartCoroutine(ChangeFrequency(5000, 0.5f));
        Debug.Log("Set low pass frequency to 5000");
    }

    public void IncreaseLowBass()
    {
        StartCoroutine(ChangeFrequency(22000, 0.5f));
        Debug.Log("Set low pass frequency to 22000");
    }
}
