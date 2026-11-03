using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownStart : MonoBehaviour
{
    public TextMeshProUGUI startText;
    private bool gameStarted = false;

    void Start()
    {
        Time.timeScale = 0f;
        startText.text = "Cath the crows!       Press any button to start";
    }

    void Update()
    {
        if (!gameStarted && Input.anyKeyDown)
        {
            gameStarted = true;
            StartCoroutine(StartCountdown());
        }
    }

    IEnumerator StartCountdown()
    {
        startText.text = "3";
        yield return new WaitForSecondsRealtime(1f);

        startText.text = "2";
        yield return new WaitForSecondsRealtime(1f);

        startText.text = "1";
        yield return new WaitForSecondsRealtime(1f);

        startText.text = "Start!";
        yield return new WaitForSecondsRealtime(0.7f);

        startText.gameObject.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Игра началась!");

        // 🔽 Запускаем отсчёт на завершение игры
        StartCoroutine(EndGameAfterDelay(20f));
    }

    IEnumerator EndGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndGame();
    }

    void EndGame()
    {
        Debug.Log("Игра завершена через 20 секунд.");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // если игра запущена в редакторе
#else
        Application.Quit(); // если это сборка (.exe)
#endif
    }
}
