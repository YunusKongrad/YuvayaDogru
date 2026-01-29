using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Not1 : MonoBehaviour
{
    public KisaNotlar kisaNotlarScript;
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

        // oyuncu checkpoint aldýðýnda nereye bakýyorsa respawn’da da oraya bakar.
        kisaNotlarScript.KisaNotuCagir("Engellerden zýplayarak ve kenarýndan geçerek çamaþýr makinesine  gitmen lazim. Burda 2 farklý yol var. Seçimini iyi yap!");
    }
}
