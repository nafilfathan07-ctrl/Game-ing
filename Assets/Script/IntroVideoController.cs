using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string namaSceneTujuan;

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += PindahScene;
        }
    }

    void PindahScene(VideoPlayer vp)
    {
        SceneManager.LoadScene(namaSceneTujuan);
    }
}