using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FanWindTarget : MonoBehaviour
{
   public float xForce = 15f;
   public float upwardForce = 6f;
   public float airDrag = 1.5f;

   private void OnTriggerEnter(Collider other)
   {
      if (!other.CompareTag("Player")) return;

      Rigidbody rb = other.GetComponent<Rigidbody>();
      if (rb == null) return;

      // Fan içi: yerçekimi KAPALI
      rb.useGravity = false;
      rb.drag = airDrag;
   }

   private void OnTriggerStay(Collider other)
   {
      if (!other.CompareTag("Player")) return;

      Rigidbody rb = other.GetComponent<Rigidbody>();
      if (rb == null) return;
      
      
      //  Fan kuvveti
      Vector3 force = new Vector3(xForce, upwardForce, 0f);
      rb.AddForce(force, ForceMode.Acceleration);
   }

   private void OnTriggerExit(Collider other)
   {
      if (!other.CompareTag("Player")) return;

      Rigidbody rb = other.GetComponent<Rigidbody>();
      if (rb == null) return;

      //  Fan bitti VE DOĞAL DÜŞÜŞ OLMA ANI 
      rb.useGravity = true;
      rb.drag = 0f;
   }
  
   
}
