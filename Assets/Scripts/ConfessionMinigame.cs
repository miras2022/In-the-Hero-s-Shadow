using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ConfessionMinigame : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform indicator; // Двигающаяся линия
    public RectTransform targetZone; // Зона попадания
    public Image feedbackFlash; // Вспышка успеха/ошибки
    public Text resultText; // Текст результата

    [Header("Settings")]
    public float moveSpeed = 250f; // Скорость движения
    public int successClicksNeeded = 3; // Кол-во успешных кликов

    [Header("Scene Settings")]
    public string mainGameSceneName = "Game"; // 👈 Main game scene name

    private bool movingRight = true;
    private int successCount = 0;
    private int totalClicks = 0;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = indicator;
        feedbackFlash.gameObject.SetActive(false);
        resultText.text = "";
    }

    void Update()
    {
        MoveIndicator();
        HandleInput();
    }

    void MoveIndicator()
    {
        float move = moveSpeed * Time.deltaTime * (movingRight ? 1 : -1);
        rectTransform.anchoredPosition += new Vector2(move, 0);
        float halfWidth = ((RectTransform)rectTransform.parent).rect.width / 2;
        if (rectTransform.anchoredPosition.x > halfWidth || rectTransform.anchoredPosition.x < -halfWidth)
            movingRight = !movingRight;
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            totalClicks++;
            if (IsHit())
            {
                successCount++;
                StartCoroutine(FlashFeedback(Color.green));
            }
            else
            {
                StartCoroutine(FlashFeedback(Color.red));
            }

            if (successCount >= successClicksNeeded)
                EndGame("✅ Священник поверил: Герой грешник...", true);
            else if (totalClicks >= successClicksNeeded + 2)
                EndGame("❌ Священник сомневается в твоей исповеди.", false);
        }
    }

    bool IsHit()
    {
        float indicatorX = rectTransform.anchoredPosition.x;
        float targetMin = targetZone.anchoredPosition.x - targetZone.rect.width / 2;
        float targetMax = targetZone.anchoredPosition.x + targetZone.rect.width / 2;
        return indicatorX >= targetMin && indicatorX <= targetMax;
    }

    IEnumerator FlashFeedback(Color color)
    {
        feedbackFlash.color = color;
        feedbackFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        feedbackFlash.gameObject.SetActive(false);
    }

    void EndGame(string message, bool success)
    {
        enabled = false;
        resultText.text = message;

        // Return to main game after 2 seconds
        StartCoroutine(ReturnToMainGame(success));
    }

    IEnumerator ReturnToMainGame(bool playerWon)
    {
        yield return new WaitForSeconds(2f); // Show result for 2 seconds

        GameManager gm = FindObjectOfType<GameManager>();

        if (gm != null)
        {
            // GameManager found - return via additive scene unloading
            Debug.Log($"✅ Returning to main game via GameManager. Won: {playerWon}");
            gm.OnZoneMinigameFinished(playerWon, playerWon ? 1 : 0);
        }
        else
        {
            // No GameManager - load main game scene directly
            Debug.LogWarning("⚠️ GameManager not found. Loading 'Game' scene directly.");

            if (Application.CanStreamedLevelBeLoaded(mainGameSceneName))
            {
                SceneManager.LoadScene(mainGameSceneName, LoadSceneMode.Single);
            }
            else
            {
                Debug.LogError($"❌ Scene '{mainGameSceneName}' not found in Build Settings!");
            }
        }
    }
}
