using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TostMakinasi : MonoBehaviour
{
    public Transform lid;          // Kapak
    public float closeSpeed = 120f;

    public AudioSource audioSource;
    private bool isClosing = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isClosing)
        {
            float currentZ = lid.localEulerAngles.z;

            
            if (currentZ > 0 && currentZ < 270)
                currentZ = 270;

            float newZ = Mathf.MoveTowardsAngle(
                currentZ,
                0f,
                closeSpeed * Time.deltaTime
            );

            lid.localEulerAngles = new Vector3(
                lid.localEulerAngles.x,
                lid.localEulerAngles.y,
                newZ
            );

            if (Mathf.Approximately(newZ, 0f))
            {
                isClosing = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isClosing = true;

            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
