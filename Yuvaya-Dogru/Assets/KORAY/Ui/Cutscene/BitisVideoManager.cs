using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class BitisVideoManager : MonoBehaviour
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
        StartCoroutine(OyunBitisiCoroutine());
    }
    public IEnumerator OyunBitisiCoroutine()
    {
        Fade.Instance.FadeOut();
        yield return new WaitForSeconds(Fade.Instance.fadeSuresi);
        SceneManager.LoadScene("anamenu");
    }
}
