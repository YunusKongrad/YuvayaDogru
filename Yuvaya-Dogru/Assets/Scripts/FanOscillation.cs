using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanOscillation : MonoBehaviour
{
    public float maxAngle = 30f;   // sağ +30, sol -30 = toplam 60 derece 
    public float rotateSpeed = 50f;
    

    float currentAngle = 0f;
    int direction = 1;

    void Update()
    {
        

        currentAngle += direction * rotateSpeed * Time.deltaTime;

        if (currentAngle >= maxAngle)
        {
            currentAngle = maxAngle;
            direction = -1;
        }
        else if (currentAngle <= -maxAngle)
        {
            currentAngle = -maxAngle;
            direction = 1;
        }

        transform.localRotation = Quaternion.Euler(-90f, currentAngle, 0f);
    }
}
