using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StoveFire : MonoBehaviour
{
   
    public ParticleSystem[] fireParticle;
    public Light fireLight;

    [Header("Fire Time Range")]
    public float minFireOnTime = 2f;
    public float maxFireOnTime = 4f;

    [Header("Off Time Range")]
    public float minFireOffTime = 1f;
    public float maxFireOffTime = 3f;

    void Start()
    {
        //  Her ocak farklı anda başlasın
        float startDelay = Random.Range(0f, 2.5f);
        Invoke(nameof(StartFire), startDelay);
    }

    void StartFire()
    {
        StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        while (true)
        {
            //  ATEŞ YAN
            fireParticle.Play();
            if (fireLight != null)
                fireLight.enabled = true;

            float onTime = Random.Range(minFireOnTime, maxFireOnTime);
            yield return new WaitForSeconds(onTime);

            // ATEŞ SÖN
            fireParticle.Stop();
            if (fireLight != null)
                fireLight.enabled = false;

            float offTime = Random.Range(minFireOffTime, maxFireOffTime);
            yield return new WaitForSeconds(offTime);
        }
    }
}
