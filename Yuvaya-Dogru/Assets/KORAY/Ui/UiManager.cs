using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject anaMenu, anaMenuOgeleri, nasilOynanirMenusu, yapimcilarMenusu, ayarlarMenusu, kazandinMenusu, devamEtUyarisi;
    public Button devamEtButonu;
    GameControls _gameControlls;
    public Image targetImage;
    public Sprite normalSprite, lockedSprite;
    private int devamEtAcildi;
    private Coroutine aktifCorotine;
    private void Start()
    {
        anaMenu.SetActive(true);
        anaMenuOgeleri.SetActive(true);
        nasilOynanirMenusu.SetActive(false);
        yapimcilarMenusu.SetActive(false);
        ayarlarMenusu.SetActive(false);
        kazandinMenusu.SetActive(false);
        devamEtUyarisi.SetActive(false);
        int devamEtAcildi = PlayerPrefs.GetInt("devamEtAcildi", 0);
        if(devamEtAcildi == 1)
        {
            targetImage.sprite = normalSprite;
        }
        else
        {
            targetImage.sprite = lockedSprite;
        }
    }
    public IEnumerator OyunaSifirdanBaslaCoroutine()
    {
        // Eski kaydı temizle (istersen DeleteAll da atabilirsin)
        PlayerPrefs.DeleteKey("player_px"); // tüm kaydı silmek istersen: PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("load_from_save", 0);
        Fade.Instance.FadeOut();
        yield return new WaitForSeconds(Fade.Instance.fadeSuresi);
        SceneManager.LoadScene("BaslangicCutScene");
    }
    public void OyunaSifirdanBasla()
    {
        StartCoroutine(OyunaSifirdanBaslaCoroutine());
    }
    public IEnumerator OyunaKaldiginYerdenDevamEtCoroutine()
    {
        Debug.Log("[UI] Devam Et coroutine başladı.");

        // Önce flag'i yaz, sonra fade / sahne işleri gelsin
        PlayerPrefs.SetInt("load_from_save", 1);
        PlayerPrefs.Save();
        Debug.Log("[UI] load_from_save = 1 yazıldı.");

        Fade.Instance.FadeOut();
        yield return new WaitForSeconds(Fade.Instance.fadeSuresi);

        anaMenu.SetActive(false);
        anaMenuOgeleri.SetActive(false);

        Debug.Log("[UI] yeniLevelSahnesi yükleniyor...");
        SceneManager.LoadScene("yeniLevelSahnesi");
    }
    public void OyunaKaldiginYerdenDevamEt()
    {
        int devamEtAcildiIki = PlayerPrefs.GetInt("devamEtAcildi", 0);
        if(devamEtAcildiIki == 1)
        {
            StartCoroutine(OyunaKaldiginYerdenDevamEtCoroutine());
        }
        else
        {
            if(aktifCorotine != null)
            {
                StopCoroutine(aktifCorotine);
            }
            aktifCorotine = StartCoroutine(NotunBelirmeSureci());
        }
    }
    private IEnumerator NotunBelirmeSureci()
    {
        devamEtUyarisi.SetActive(true);
        yield return new WaitForSeconds(3f);
        devamEtUyarisi.SetActive(false);
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
    public void ExitGame()
    {
        Application.Quit();
    }
}
