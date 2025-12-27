using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonRay : MonoBehaviour
{
    public float rayDistance = 3f;

    void Update()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1.2f;

        Debug.DrawRay(origin, transform.forward * rayDistance, Color.green);

        if (Physics.Raycast(origin, transform.forward, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Button"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Kapıacma button = hit.collider.GetComponent<Kapıacma>();
                    if (button != null)
                        button.PressButton();
                    Debug.Log("çarptı ışın");
                }
            }
        }
    }
}
