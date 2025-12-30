using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiOyunİciManager : MonoBehaviour
{
    public GameObject pausePaneli, pauseOgeleri, pauseAyarlarMenusu;
    public CheckpointRespawn checkpointRespawnScript;
    private void Start()
    {
        pausePaneli.SetActive(false);
        pauseOgeleri.SetActive(false);
        pauseAyarlarMenusu.SetActive(false);
    }
    public void AnaMenuyeGecis()
    {
        SceneManager.LoadScene("KorayDeneme");
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
            }
            else if(pausePaneli.activeSelf == true)
            {
                pausePaneli.SetActive(false);
                pauseOgeleri.SetActive(false);
                pauseAyarlarMenusu.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
    public void BolumuYenidenOyna()
    {
        checkpointRespawnScript.RespawnToLastCheckpoint();
    }
    public void OyunaDevamEtPauseEkranindanCik()
    {
        pauseOgeleri.SetActive(false);
        pausePaneli.SetActive(false);
        Time.timeScale = 1f;
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
    }
}
