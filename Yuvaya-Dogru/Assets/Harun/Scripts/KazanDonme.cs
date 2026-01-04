using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KazanDonme : MonoBehaviour
{
    [Header("Hız Ayarı")]
    public float donmeHizi = 50f; 

    void Update()
    {
      
        transform.Rotate(0, donmeHizi * Time.deltaTime, 0, Space.Self);
    }
}
