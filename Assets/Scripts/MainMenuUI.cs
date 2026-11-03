using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelMain;        // Главная панель (кнопки Play/Options/Leaderboard/Quit)
    public GameObject panelOptions;     // Панель настроек (громкость/чувствительность)
    public GameObject panelLeaderboard; // Панель лидерборда

    [Header("Options UI")]
    public Slider volumeSlider;         // Слайдер громкости
    public Slider mouseSensSlider;      // Слайдер чувствительности мыши

    [Header("Config")]
    public string gameSceneName = "Game"; // Имя игровой сцены, которую грузит Play

    void Start()
    {
        // Инициализация значений (можете править под себя)
        if (volumeSlider) volumeSlider.value = PlayerPrefs.GetFloat("vol", 0.8f);
        if (mouseSensSlider) mouseSensSlider.value = PlayerPrefs.GetFloat("sens", 1.0f);

        // Включаем главную панель
        ShowPanel(panelMain);
        // Применяем громкость
        AudioListener.volume = volumeSlider ? volumeSlider.value : 0.8f;
    }

    // ==== Кнопки Главного меню ====
    public void OnPlay()
    {
        // Важно: сцена с таким именем должна быть добавлена в Build Settings
        SceneManager.LoadScene("CatScene");
    }

    public void OnOptions()
    {
        ShowPanel(panelOptions);
    }

    public void OnLeaderboard()
    {
        ShowPanel(panelLeaderboard);
        // Здесь позже добавите загрузку топа из SDK
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        // В WebGL приложение закрыть нельзя — спрячьте кнопку Quit
#else
        Application.Quit();
#endif
    }

    // ==== Кнопки внутри панелей ====
    public void OnBackToMain()
    {
        ShowPanel(panelMain);
    }

    public void OnVolumeChanged(float v)
    {
        AudioListener.volume = v;
        PlayerPrefs.SetFloat("vol", v);
    }

    public void OnMouseSensChanged(float v)
    {
        PlayerPrefs.SetFloat("sens", v);
    }

    // ==== Служебное ====
    void ShowPanel(GameObject panel)
    {
        if (panelMain) panelMain.SetActive(false);
        if (panelOptions) panelOptions.SetActive(false);
        if (panelLeaderboard) panelLeaderboard.SetActive(false);
        if (panel) panel.SetActive(true);
    }
}
