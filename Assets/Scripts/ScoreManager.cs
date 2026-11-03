using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public Text scoreText;      // текст на Canvas для текущих очков
    private int score = 0;

    void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int points)
    {
        score += 1;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Очки: " + score; // обновление во время игры
    }

    // Вызываем в конце игры
    public void ShowFinalScore()
    {   
        if (scoreText != null) 
            scoreText.text = "Итоговые очки: " + score; // показываем финальный результат
    }

    public int GetScore()
    {
        return score; // можно использовать для других целей
    }
}
