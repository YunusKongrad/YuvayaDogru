using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    private void Reset()
    {
        // Script eklenince otomatik olarak trigger yapsın
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Sadece Player tetiklesin
        if (!other.CompareTag("Player")) return;

        // Player collider child olabilir diye parent’a kadar ara
        var respawn = other.GetComponentInParent<CheckpointRespawn>();
        if (respawn == null) return;

        // oyuncu checkpoint aldığında nereye bakıyorsa respawn’da da oraya bakar.
        Transform playerRoot = respawn.transform;

        respawn.SetCheckpoint(transform.position, playerRoot.rotation);
        SaveManager.Instance.SaveCheckpoint();
    }
}