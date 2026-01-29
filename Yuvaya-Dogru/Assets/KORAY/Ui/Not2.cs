using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Not2 : MonoBehaviour
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
        kisaNotlarScript.KisaNotuCagir("Boyunun yetmediði yerlere tutunmaya çalýþ,eðer tutunursan e tuþuna basarak yukarý çýkabilirsin. Süngerlere dikkat et, göklerde kartal gibi olma Ayýca vantilatöre de dikkat et, yanlýþ açýdan zýplarsan þaftýn kayabilir.");
    }
}
