using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string additiveSceneName = "Main";

    void Awake()
    {
        // Проверяем, загружена ли сцена с UI
        if (!SceneManager.GetSceneByName(additiveSceneName).isLoaded)
        {
            // Загружаем сцену поверх текущей
            SceneManager.LoadScene(additiveSceneName, LoadSceneMode.Additive);
        }
    }
}
