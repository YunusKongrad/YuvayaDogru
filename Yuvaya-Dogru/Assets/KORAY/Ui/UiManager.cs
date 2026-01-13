using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public GameObject anaMenu, anaMenuOgeleri, nasilOynanirMenusu, yapimcilarMenusu, ayarlarMenusu, kazandinMenusu;
    GameControls _gameControlls;
    private void Start()
    {
        anaMenu.SetActive(true);
        anaMenuOgeleri.SetActive(true);
        nasilOynanirMenusu.SetActive(false);
        yapimcilarMenusu.SetActive(false);
        ayarlarMenusu.SetActive(false);
        kazandinMenusu.SetActive(false);
    }
    public IEnumerator OyunaSifirdanBaslaCoroutine()
    {
        Fade.Instance.FadeOut();
        yield return new WaitForSeconds(Fade.Instance.fadeSuresi);
        SceneManager.LoadScene("yunusScene");
    }
    public void OyunaSifirdanBasla()
    {
        StartCoroutine(OyunaSifirdanBaslaCoroutine());
    }
    public IEnumerator OyunaKaldiginYerdenDevamEtCoroutine()
    {
        Fade.Instance.FadeOut();
        yield return new WaitForSeconds(Fade.Instance.fadeSuresi);
        anaMenu.SetActive(false);
        anaMenuOgeleri.SetActive(false);
        // Oyunun kald���m�z yerden ba�lat�lmas� ve oyun i�i ui buraya kodlanacak
        // �imdilik loadscene at�yorum
        SceneManager.LoadScene("OyunEkraniDeneme");
    }
    public void OyunaKaldiginYerdenDevamEt()
    {
        StartCoroutine(OyunaKaldiginYerdenDevamEtCoroutine());
    }
    public void YapimcilarMenusuneGecis()
    {
        anaMenuOgeleri.SetActive(false);
        yapimcilarMenusu.SetActive(true);
    }
    public void AyarlarMenusuneGecis()
    {
        anaMenuOgeleri.SetActive(false);
        ayarlarMenusu.SetActive(true);
    }
    public void NasilOynanirMenusuneGecis()
    {
        anaMenuOgeleri.SetActive(false);
        nasilOynanirMenusu.SetActive(true);
    }
    public void AnaMenuyeGecis()
    {
        if(nasilOynanirMenusu.activeSelf == true)
        {
            anaMenuOgeleri.SetActive(true);
            nasilOynanirMenusu.SetActive(false);
        }
        else if(yapimcilarMenusu.activeSelf == true)
        {
            anaMenuOgeleri.SetActive(true);
            yapimcilarMenusu.SetActive(false);
        }
        else if(ayarlarMenusu.activeSelf == true)
        {
            anaMenuOgeleri.SetActive(true);
            ayarlarMenusu.SetActive(false);
        }
        else if(kazandinMenusu.activeSelf == true)
        {
            kazandinMenusu.SetActive(false);
            anaMenu.SetActive(true);
            anaMenuOgeleri.SetActive(true);
            //oyun i�i'in kapat�lmas� falan buraya yaz�lacak
        }
    }

}
