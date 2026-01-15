using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ImpactSound : MonoBehaviour
{
    [SerializeField] private AudioClip impactClip;
    [SerializeField] private float volume = 1f;
    [SerializeField] private float minImpactVelocity = 1f;

    private AudioSource audioSource;
    private Rigidbody rb;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (impactClip == null) return;

        // Çok hafif temaslarda çalmasın
        if (rb != null && rb.velocity.magnitude < minImpactVelocity)
            return;

        audioSource.PlayOneShot(impactClip, volume);
    }
}