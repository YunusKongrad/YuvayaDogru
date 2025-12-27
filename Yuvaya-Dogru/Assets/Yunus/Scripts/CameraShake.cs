using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private Vector3 _originalLocalPos;
    private Coroutine _shakeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _originalLocalPos = transform.localPosition;
    }

    public void Shake(float duration = 0.2f, float strength = 0.08f, float frequency = 25f)
    {
        // Üst üste tetiklenirse mevcut shake’i kesip yenisini başlat
        if (_shakeRoutine != null) StopCoroutine(_shakeRoutine);
        _shakeRoutine = StartCoroutine(ShakeRoutine(duration, strength, frequency));
    }

    private IEnumerator ShakeRoutine(float duration, float strength, float frequency)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            // “deprem” hissi: küçük, hızlı titreşim
            float offsetX = (Mathf.PerlinNoise(Time.time * frequency, 0f) - 0.5f) * 2f * strength;
            float offsetY = (Mathf.PerlinNoise(0f, Time.time * frequency) - 0.5f) * 2f * strength;

            transform.localPosition = _originalLocalPos + new Vector3(offsetX, offsetY, 0f);

            yield return null;
        }

        transform.localPosition = _originalLocalPos;
        _shakeRoutine = null;
    }
}