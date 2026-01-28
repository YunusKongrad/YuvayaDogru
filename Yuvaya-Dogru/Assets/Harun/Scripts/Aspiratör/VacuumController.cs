using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumController : MonoBehaviour
{
    public Transform[] targets;   // 1,2,3,4 sırasıyla
    public float pullSpeed = 3f;
    public float reachDistance = 0.15f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Char_Controller pc = other.GetComponent<Char_Controller>();
        if (pc == null) return;

        pc.isVacuumed = true;

        if (pc.vacuumStage < 0)
            pc.vacuumStage = 0;

        // Güvenlik
        if (pc.vacuumStage >= targets.Length)
        {
            pc.isVacuumed = false;
            return;
        }

        Transform currentTarget = targets[pc.vacuumStage];
        Vector3 dir = currentTarget.position - other.transform.position;

        // Hareket
        if (dir.magnitude > reachDistance)
        {
            pc.VacuumMove(dir.normalized * pullSpeed);
            pc.gravity = 0;
        }
        else
        {

            pc.vacuumStage++;
            if (pc.vacuumStage >= targets.Length)
            {
                pc.isVacuumed = false;

            }
        }
    }

}

