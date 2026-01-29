using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bitisTrigger : MonoBehaviour
{
    private bool tetiklendi = false;

    private void OnTriggerEnter(Collider other)
    {
        if (tetiklendi) return;

        if (other.CompareTag("Player"))
        {
            tetiklendi = true;
            StartCoroutine(BitisCutsceneineGecis());
        }
    }
    public IEnumerator BitisCutsceneineGecis()
    {
        Fade.Instance.FadeOut();
        yield return new WaitForSeconds(Fade.Instance.fadeSuresi);
        SceneManager.LoadScene("BitisCutScene");
    }
}
