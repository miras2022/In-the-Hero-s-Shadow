using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class WinSceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button skipButton;
    public string nextSceneName = "MainMenu"; // Scene to load after video

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
        SceneManager.LoadScene(nextSceneName);
    }
}
