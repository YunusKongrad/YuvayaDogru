using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    private float fadeSuresi = 5f;
    private void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
    public void FadeIn()
    {
        StartCoroutine(FadeCoroutine(0));
    }
    public void FadeOut()
    {
        StartCoroutine(FadeCoroutine(1));
    }
    private IEnumerator FadeCoroutine(float end)
    {
        float gecenSure = 0f;
        while(gecenSure < fadeSuresi)
        {
            gecenSure += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, end, gecenSure / fadeSuresi);
            yield return null;
        }
        canvasGroup.alpha = end;
    }
}
