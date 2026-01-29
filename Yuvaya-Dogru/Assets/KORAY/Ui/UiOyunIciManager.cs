using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiOyunIciManager : MonoBehaviour
{
    public GameObject pausePaneli, pauseOgeleri, pauseAyarlarMenusu;
    public CheckpointRespawn checkpointRespawnScript;
    public bool pauseAktif;
    private void Start()
    {
        Debug.Log("UiOyunİciManager Start ÇALIŞTI, Fade.Instance: " + Fade.Instance);
        pausePaneli.SetActive(false);
        pauseOgeleri.SetActive(false);
        pauseAyarlarMenusu.SetActive(false);
        pauseAktif = false;
        Time.timeScale = 1;
        Fade.Instance.FadeIn();
    }
    public void AnaMenuyeGecis()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("anamenu");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePaneli.activeSelf == false)
            {
                pausePaneli.SetActive(true);
                pauseOgeleri.SetActive(true);
                Time.timeScale = 0f;
                pauseAktif = true;
            }
            else if(pausePaneli.activeSelf == true)
            {
                pausePaneli.SetActive(false);
                pauseOgeleri.SetActive(false);
                pauseAyarlarMenusu.SetActive(false);
                Time.timeScale = 1f;
                pauseAktif = false;
            }
        }
    }
    public void BolumuYenidenOyna()
    {
        checkpointRespawnScript.RespawnToLastCheckpoint();
        pauseOgeleri.SetActive(false);
        pausePaneli.SetActive(false);
        Time.timeScale = 1f;
        pauseAktif = false;
    }
    public void OyunaDevamEtPauseEkranindanCik()
    {
        pauseOgeleri.SetActive(false);
        pausePaneli.SetActive(false);
        Time.timeScale = 1f;
        pauseAktif = false;
    }
    public void PausedakiAyarlarMenusuneGecis()
    {
        pauseOgeleri.SetActive(false);
        pauseAyarlarMenusu.SetActive(true);
    }
    public void PauseAyarlardanDuzPaneleGeriGecme()
    {
        pauseOgeleri.SetActive(true);
        pauseAyarlarMenusu.SetActive(false);
        Time.timeScale = 1f;
        pauseAktif = false;
    }
}
