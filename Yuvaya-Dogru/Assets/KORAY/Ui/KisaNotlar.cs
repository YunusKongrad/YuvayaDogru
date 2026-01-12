using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KisaNotlar : MonoBehaviour
{
    public GameObject kisaNotlarPaneli;
    public Text kisaNotTexti;
    private float belirmeSuresi = 5f;
    private Coroutine aktifCoroutine;
    private void Awake()
    {
        kisaNotlarPaneli.SetActive(false);
    }
    public void KisaNotuCagir(string yazacakNot)
    {
        if(aktifCoroutine != null)
        {
            StopCoroutine(aktifCoroutine);
        }
        kisaNotTexti.text = yazacakNot;
        aktifCoroutine = StartCoroutine(NotunBelirmeSureci());
    }
    private IEnumerator NotunBelirmeSureci()
    {
        kisaNotlarPaneli.SetActive(true);
        yield return new WaitForSeconds(belirmeSuresi);
        kisaNotlarPaneli.SetActive(false);
    }
}
