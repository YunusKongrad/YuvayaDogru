using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuzgarEtkisi : MonoBehaviour
{/*
    public float ruzgarGucu = 20f;
    private Vector3 ruzgarM;
    private void OnTriggerStay(Collider other)
    {
        // Sadece etiketi Player olanı it
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
               

                ruzgarM = new Vector3(0, transform.localRotation.y * ruzgarGucu, -7 * ruzgarGucu);
                
                // Z yönünde itme kuvveti uygula
                rb.AddForce(ruzgarM,ForceMode.Acceleration);
            }
        }
    }
    */
    public float ruzgarGucu = 6f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc == null) return;

        Vector3 windMove =
            -transform.forward * ruzgarGucu * Time.deltaTime;

        cc.Move(windMove);
    }
}
