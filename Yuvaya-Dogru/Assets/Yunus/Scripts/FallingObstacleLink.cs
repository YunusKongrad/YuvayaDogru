using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FallingObstacleLink : MonoBehaviour
{
    [Header("Bu trigger tetiklenince düşecek obje (sahnede zaten duruyor olmalı)")]
    [SerializeField] private Rigidbody targetRigidbody;

    [Header("Bir kez mi çalışsın?")]
    [SerializeField] private bool triggerOnce = true;

    [Header("İstersen küçük gecikme (telegraph için)")]
    [SerializeField] private float delaySeconds = 0f;

    private bool _used;

    private void Reset()
    {
        // Trigger objesinde collider zorunlu ve trigger olmalı
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggerOnce && _used) return;

        if (targetRigidbody == null)
        {
            Debug.LogError($"{name}: Target Rigidbody atanmadı. Inspector'da targetRigidbody alanını doldur.");
            return;
        }

        _used = true;

        if (delaySeconds <= 0f)
        {
            DropNow();
        }
        else
        {
            Invoke(nameof(DropNow), delaySeconds);
        }
    }

    private void DropNow()
    {
        if (CameraShake.Instance != null)
            CameraShake.Instance.Shake(duration: 0.25f, strength: 0.06f, frequency: 28f);

        targetRigidbody.isKinematic = false;
        targetRigidbody.useGravity = true;
        targetRigidbody.WakeUp();
    }
}