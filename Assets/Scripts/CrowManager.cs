using UnityEngine;

public class CrowManager : MonoBehaviour
{
    public float gameDuration = 20f;      // длительность мини-игры в секундах
    private float timer = 0f;

    public ScoreManager scoreManager;      // ссылка на ScoreManager

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= gameDuration)
        {
            EndGame();
            enabled = false; // останавливаем GameManager
        }
    }

    void EndGame()
    {
        if (timer == 20) 
        {
            scoreManager.ShowFinalScore(); // показываем итоговый счет
        }
    }
}
