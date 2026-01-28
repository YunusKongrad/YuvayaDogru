using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FanWindTarget : MonoBehaviour
{/*
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
   */
   
   
   //aa
   public float forwardForce = 4f;
   public float upForce = 1f;
    public Transform forw;

   void OnTriggerStay(Collider other)
   {
      if (!other.CompareTag("Player")) return;

      CharacterController controller = other.GetComponent<CharacterController>();
      if (controller == null) return;

      Vector3 push =
        forw.forward * forwardForce +
         Vector3.up * upForce;

      controller.Move(push * Time.deltaTime);
      
   }

   void OnTriggerExit(Collider other)
   {
      if (!other.CompareTag("Player")) return;

      Playermovement pm = other.GetComponent<Playermovement>();
      if (pm == null) return;

      //  Fan alanından çıkınca gravity AZALSIN
      pm.SetLowGravity();
   }
}
