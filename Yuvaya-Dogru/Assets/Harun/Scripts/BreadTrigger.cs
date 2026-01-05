using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadTrigger : MonoBehaviour
{
    public GameObject bread1;

    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !used)
        {
            used = true;
            bread1.SetActive(true);
        }
    }
}
