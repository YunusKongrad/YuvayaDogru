using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBladeRotate : MonoBehaviour
{
    public float bladeSpeed = 600f;
    
    
        void Update()
        {
            
    
            transform.Rotate( bladeSpeed * Time.deltaTime, 0,0, Space.Self);
        }
}
