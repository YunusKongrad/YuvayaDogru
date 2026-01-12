using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumController : MonoBehaviour
{
    

    public Transform target1;   // mavi objenin ortası
    public Transform target2;   // üst sağ nokta
    public float pullSpeed = 3f;
    public float reachDistance = 0.15f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Char_Controller pc = other.GetComponent<Char_Controller>();
        if (pc == null) return;

        pc.isVacuumed = true;

        // Stage ilk defa girince başlasın
        if (pc.vacuumStage == 0)
            pc.vacuumStage = 1;

        Transform currentTarget =
            pc.vacuumStage == 1 ? target1 :
            pc.vacuumStage == 2 ? target2 :
            null;

        if (currentTarget == null) return;

        Vector3 dir = currentTarget.position - other.transform.position;

        // HAREKET
        if (dir.magnitude > reachDistance)
        {
            pc.VacuumMove(dir.normalized * pullSpeed);
        }
        else
        {
            // 🎯 HEDEF 1 → HEDEF 2 GEÇİŞİ
            if (pc.vacuumStage == 1)
            {
                pc.vacuumStage = 2;
            }
            // 🎯 HEDEF 2 BİTTİ
            else if (pc.vacuumStage == 2)
            {
                pc.isVacuumed = false;   // ister açık bırak ister kapat
                // burada animasyon / yok olma / sahne olayı eklenebilir
            }
        }
    }
}
