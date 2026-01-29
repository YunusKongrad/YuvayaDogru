using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public static Fade Instance;
    public CanvasGroup canvasGroup;
    public float fadeSuresi = 1.5f;
    private Coroutine fadeAktif;
    private void Start()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
    public void FadeIn()
    {
        FadeBaslat(0);
        Debug.Log("fade çalýþtý");
    }
    public void FadeOut()
    {
        FadeBaslat(1);
    }
    private void FadeBaslat(float end)
    {
        if(fadeAktif != null)
        {
            StopCoroutine(fadeAktif);
        }
        fadeAktif = StartCoroutine(FadeCoroutine(end));
    }
    private IEnumerator FadeCoroutine(float end)
    {
        float start = canvasGroup.alpha;
        float gecenSure = 0f;
        while(gecenSure < fadeSuresi)
        {
            gecenSure += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, gecenSure / fadeSuresi);
            yield return null;
        }
        canvasGroup.alpha = end;
        fadeAktif = null;
    }
}
