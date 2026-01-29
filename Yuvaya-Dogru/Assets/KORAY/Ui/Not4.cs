using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Not4 : MonoBehaviour
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
        kisaNotlarScript.KisaNotuCagir("Kýrýntýlar atýþtýrmalýk deðil. Onlar sana hýz ve güç veriyor. Uzak yerlere atlamak için kullan bunu. Reçelli ekmek dilimleri yapýþkandýr, seni yavaþlatýr. Tost makinesinin içine kahvaltýlýk olursun, üstüne çýkman lazým.");
    }
}
