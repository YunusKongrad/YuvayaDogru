using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushableObjScripts : MonoBehaviour
{
    Char_Controller cc;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("asdasdad");
            if (cc==null)
            {
                cc=collision.gameObject.GetComponent<Char_Controller>();
            }
            else
            {
                Vector3 worldNormal = collision.contacts[0].normal;
                Debug.Log(worldNormal);
                    
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
    }
}
