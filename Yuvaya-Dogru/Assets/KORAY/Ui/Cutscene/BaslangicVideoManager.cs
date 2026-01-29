using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class BaslangicVideoManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        Fade.Instance.FadeIn();
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(OyunaSifirdanBaslaCoroutine());
    }
    public IEnumerator OyunaSifirdanBaslaCoroutine()
    {
        Fade.Instance.FadeOut();
        yield return new WaitForSeconds(Fade.Instance.fadeSuresi);
        SceneManager.LoadScene("yeniLevelSahnesi");
    }
}
