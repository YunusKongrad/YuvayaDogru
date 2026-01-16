using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanRotateTwo : MonoBehaviour
{
    public float bladeSpeed = 600f;
    
    void Update()
    {
            
    
        transform.Rotate( 0 ,bladeSpeed * Time.deltaTime,0, Space.Self);
    }
}


