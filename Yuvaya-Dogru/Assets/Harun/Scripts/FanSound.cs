using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSound : MonoBehaviour
{
    public float targetVolume = 0.5f;
    public float fadeSpeed = 1.5f;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.volume = targetVolume;
        audioSource.loop = true;
        audioSource.Play();
    }
    
    void Update()
    {
        audioSource.pitch = Mathf.Lerp(0.8f, 1.2f, Mathf.Abs(Mathf.Sin(Time.time)));
    }

}
