using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadMoveUp : MonoBehaviour
{
    public float startY = 5.178f;
    public float targetY = 5.9f;
    public float duration = 0.5f;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (audioSource != null)
            audioSource.Play();

        StartCoroutine(MoveUp());
    }
    IEnumerator MoveUp()
    {
        float time = 0f;
        Vector3 pos = transform.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            float newY = Mathf.Lerp(startY, targetY, t);
            transform.position = new Vector3(pos.x, newY, pos.z);

            yield return null;
        }

        transform.position = new Vector3(pos.x, targetY, pos.z);
    }
}
