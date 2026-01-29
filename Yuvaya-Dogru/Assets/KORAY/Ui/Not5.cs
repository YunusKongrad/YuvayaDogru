using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Not5 : MonoBehaviour
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
        kisaNotlarScript.KisaNotuCagir("Tependen düþenlere dikkat et, böcek gibi ezilme. Ocakta da kavrulma. Amma tehlike var. Ýtilebilir objeler var, gücünü kullan ve koþarken dikkatli ol.");
    }
}
