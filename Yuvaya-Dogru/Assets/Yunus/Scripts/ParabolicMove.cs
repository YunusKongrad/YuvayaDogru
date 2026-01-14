using System;
using System.Collections;
using UnityEngine;

public class ParabolicMove : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public float duration = 0.8f;

    [Header("Arc")]
    public float arcHeight = 0.5f;   // tepe yüksekliği
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Rotation")]
    public Vector3 spinDegrees = new Vector3(0, 360f, 0); // hareket boyunca döndürme
    
    private bool played = false;

    private void Start()
    {
    }

    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        if (!start || !end) yield break;

        Vector3 p0 = start.position;
        Vector3 p1 = end.position;
        Quaternion r0 = start.rotation;
        Quaternion r1 = end.rotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float u = ease.Evaluate(Mathf.Clamp01(t));

            // düz lerp
            Vector3 pos = Vector3.Lerp(p0, p1, u);

            // parabolik yükseklik (uçlarda 0, ortada max)
            float height = 4f * u * (1f - u) * arcHeight;
            pos.y += height;

            transform.position = pos;

            // dönüş: start->end + ekstra spin
            Quaternion baseRot = Quaternion.Slerp(r0, r1, u);
            Quaternion spin = Quaternion.Euler(spinDegrees * u);
            transform.rotation = baseRot * spin;

            yield return null;
        }

        transform.position = p1;
        transform.rotation = r1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (played) return;

        if (other.CompareTag("Player"))
        {
            played = true;
            Play();
        }
    }
}