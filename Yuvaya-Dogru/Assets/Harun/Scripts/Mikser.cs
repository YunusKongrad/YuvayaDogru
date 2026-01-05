using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mikser : MonoBehaviour
{
    public Transform rotatePart;
    public float rotateSpeed = 150f;

    private bool canRotate = false;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (canRotate)
        {
            rotatePart.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canRotate = true;

            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canRotate = false;

            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
