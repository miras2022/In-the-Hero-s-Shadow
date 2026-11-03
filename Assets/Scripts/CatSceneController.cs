using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class CatSceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button skipButton;
    public string nextSceneName = "Game"; // сцена, куда перейти

    void Start()
    {
        skipButton.onClick.AddListener(SkipVideo);
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void SkipVideo()
    {
        videoPlayer.Stop();
        LoadNextScene();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Game");
    }
}
