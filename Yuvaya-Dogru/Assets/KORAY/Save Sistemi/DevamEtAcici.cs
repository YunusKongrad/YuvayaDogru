using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevamEtAcici : MonoBehaviour
{
    private bool devamEtmeAcildi = false;
    private void Reset()
    {
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

        if (!other.CompareTag("Player"))
            return;

        if (devamEtmeAcildi)
            return; // bir kere çalýþsýn

        devamEtmeAcildi = true;

        // DEVAM ET AÇILDI
        PlayerPrefs.SetInt("devamEtAcildi", 1);
        PlayerPrefs.Save();

        Debug.Log("Ýlk checkpoint alýndý, Devam Et açýldý!");
    }
}
