using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using IDosGames;  // Make sure to include this for SDK methods

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public KnightAI knight;

    [Header("Encounter")]
    public Vector2 duoOffset = new Vector2(0.45f, 0f);
    public string encounterSceneName = "EncounterMinigame";

    [Header("Game State")]
    public int totalMinigames = 6;
    public float maxGameTime = 300f;
    public string mainMenuSceneName = "MainMenu";

    [Header("HUD UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI minigamesText;
    public TextMeshProUGUI suspicionText;
    public TextMeshProUGUI coinText;  // Display the coin balance

    [Header("Game Over UI")]
    public Canvas gameOverCanvas;
    public Image gameOverImage;

    [Header("Game Over Images")]
    public Sprite youDiedSprite;
    public Sprite youWinSprite;

    private bool encounterRunning = false;
    private bool zoneMinigameRunning = false;
    private string activeZoneMinigameScene = null;
    private Zone currentMinigameZone = null;

    private int minigamesCompleted = 0;
    private int suspicionLevel = 0;
    private float gameStartTime;
    private bool gameEnded = false;

    private int playerScore = 0;
    private int playerCoins = 0;

    private void Awake()
    {
        if (player == null)
        {
            var pObj = GameObject.Find("Player");
            if (pObj != null) player = pObj.GetComponent<PlayerController>();
        }

        if (knight == null)
        {
            var kObj = GameObject.Find("Knight");
            if (kObj != null) knight = kObj.GetComponent<KnightAI>();
        }

        if (gameOverCanvas != null)
            gameOverCanvas.gameObject.SetActive(false);
        if (gameOverImage != null)
            gameOverImage.enabled = false;
    }

    private void Start()
    {
        gameStartTime = Time.time;
        UpdateHUD();
        StartPlayerTurn();
        StartCoroutine(CheckGameTimer());
    }

    private void Update()
    {
        if (!gameEnded)
        {
            UpdateTimerDisplay();
            CheckGameOverConditions();
            CheckWinCondition();
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText == null) return;

        float timeRemaining = maxGameTime - (Time.time - gameStartTime);
        if (timeRemaining < 0) timeRemaining = 0;

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = $"Time: {minutes:00}:{seconds:00}";

        if (timeRemaining < 60f)
            timerText.color = Color.red;
        else if (timeRemaining < 120f)
            timerText.color = Color.yellow;
        else
            timerText.color = Color.white;
    }

    private void UpdateHUD()
    {
        if (minigamesText != null)
            minigamesText.text = $"Minigames: {minigamesCompleted}/{totalMinigames}";

        if (suspicionText != null)
        {
            suspicionText.text = $"Suspicion: {suspicionLevel}/6";
            if (suspicionLevel >= 5)
                suspicionText.color = Color.red;
            else if (suspicionLevel >= 3)
                suspicionText.color = Color.yellow;
            else
                suspicionText.color = Color.white;
        }

        if (coinText != null)
            coinText.text = $"Coins: {playerCoins}";  // Update coin display
    }

    private IEnumerator CheckGameTimer()
    {
        while (!gameEnded)
        {
            float elapsed = Time.time - gameStartTime;
            if (elapsed >= maxGameTime)
            {
                GameOver(false);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void CheckGameOverConditions()
    {
        if (suspicionLevel >= 6)
            GameOver(false);
    }

    private void CheckWinCondition()
    {
        if (minigamesCompleted >= totalMinigames && suspicionLevel < 6)
            GameWin();
    }

    private void GameWin()
    {
        if (gameEnded) return;
        gameEnded = true;
        CalculateScore();
        ConvertScoreToCoins();
        StartCoroutine(ShowGameEndScreen(true));
    }

    private void GameOver(bool isWin)
    {
        if (gameEnded) return;
        gameEnded = true;
        StartCoroutine(ShowGameEndScreen(isWin));
    }

    private IEnumerator ShowGameEndScreen(bool isWin)
    {
        if (gameOverCanvas != null && gameOverImage != null)
        {
            gameOverCanvas.gameObject.SetActive(true);
            gameOverImage.sprite = isWin ? youWinSprite : youDiedSprite;
            gameOverImage.enabled = true;
        }

        yield return new WaitForSeconds(5f); // Show image for 5 seconds

        if (isWin)
        {
            SceneManager.LoadScene("WinScene");
        }
        else
        {
            SceneManager.LoadScene("LoseScene");
        }
    }


    // Calculate score based on time and suspicion level
    private void CalculateScore()
    {
        // Calculate remaining time
        float timeRemaining = maxGameTime - (Time.time - gameStartTime);
        if (timeRemaining < 0) timeRemaining = 0;

        // Normalize time score (0 to 700 points)
        float timeScore = (timeRemaining / maxGameTime) * 700f;

        // Normalize suspicion score (0 to 300 points)
        float suspicionScore = ((6 - suspicionLevel) / 6f) * 300f;

        playerScore = Mathf.RoundToInt(timeScore + suspicionScore);
        playerScore = Mathf.Clamp(playerScore, 0, 1000);

        Debug.Log($"Player Score: {playerScore}");
    }

    // Convert the score to coins (for SDK integration)
    private void ConvertScoreToCoins()
    {
        // Assuming 1 score point = 1 coin, but you can adjust this ratio
        playerCoins = playerScore/2; // Or change this ratio as needed, e.g., 1 point = 0.5 coin

        // Save the coins to PlayerPrefs (you can replace this with SDK integration logic)
        PlayerPrefs.SetInt("PlayerCoins", playerCoins);
        PlayerPrefs.Save();

        // Log the conversion
        Debug.Log($"Player converted {playerScore} points to {playerCoins} coins.");

        // Integrate with SDK to add coins
        // Using ClaimRewardSystem to add coins
        if (UserInventory.HasVIPStatus == true)
        {
            ClaimRewardSystem.ClaimTokenReward(playerCoins, 5);
        } 
        else { ClaimRewardSystem.ClaimCoinReward(playerScore, 5); }  // Example of claiming reward

        // Optionally, you can call any other SDK method for adding coins if needed
        // IDOS.Games.AddCoins(playerCoins); 
    }

    public void AttemptZoneEntry(Zone targetZone)
    {
        if (targetZone == null) return;

        // Проверка на рыцаря
        if (knight != null && knight.currentZone == targetZone)
        {
            if (TryStartEncounterIfZone(targetZone))
                return;
        }

        // Переместить игрока
        player.currentZone = targetZone;
        player.transform.position = targetZone.transform.position;

        // Если зона новая — запускаем мини-игру
        if (!targetZone.isMinigameCompleted)
        {
            StartZoneMinigame(targetZone);
        }
        else
        {
            // ✅ Если зона уже пройдена:
            player.EnableMove(); // вернуть управление игроку
            if (knight != null)
            {
                // ✅ Рыцарь всё равно должен сделать ход
                knight.TakeTurn(player.currentZone);
            }
        }
    }

    public bool TryStartEncounterIfZone(Zone zone)
    {
        if (player == null || knight == null || zone == null) return false;
        if (knight.currentZone == zone)
        {
            suspicionLevel++;
            UpdateHUD();

            AlignActorsSideBySide(zone.transform.position);
            StartEncounter(); // Load EncounterMinigame
            return true;
        }
        return false;
    }

    private void AlignActorsSideBySide(Vector3 zoneCenter)
    {
        var z = player.currentZone;
        if (z != null && z.playerAnchor && z.knightAnchor)
        {
            if (player)
            {
                var p = z.playerAnchor.position;
                p.z = player.transform.position.z;
                player.transform.position = p;
            }
            if (knight)
            {
                var k = z.knightAnchor.position;
                k.z = knight.transform.position.z;
                knight.transform.position = k;
            }
        }
        else
        {
            if (player)
            {
                var p = zoneCenter - (Vector3)duoOffset;
                p.z = player.transform.position.z;
                player.transform.position = p;
            }
            if (knight)
            {
                var k = zoneCenter + (Vector3)duoOffset;
                k.z = knight.transform.position.z;
                knight.transform.position = k;
            }
        }
    }

    private void StartEncounter()
    {
        if (encounterRunning || gameEnded) return;
        if (!Application.CanStreamedLevelBeLoaded(encounterSceneName))
        {
            Debug.LogError($"[Encounter] Scene '{encounterSceneName}' not found in Build Settings.");
            return;
        }

        encounterRunning = true;
        if (player) player.DisableMoveImmediate();
        SceneManager.LoadScene(encounterSceneName, LoadSceneMode.Additive);
    }

    public void OnEncounterFinished(bool success, Zone zone = null)
    {
        if (SceneManager.GetSceneByName(encounterSceneName).isLoaded)
            SceneManager.UnloadSceneAsync(encounterSceneName);

        encounterRunning = false;

        if (zone != null && !zone.isMinigameCompleted)
        {
            zone.MarkMinigameCompleted();
        }

        if (knight != null)
            knight.TakeTurn(player.currentZone);
        else
            StartPlayerTurn();
    }

    public void NotifyKnightMoved(bool moved)
    {
    }

    public void StartZoneMinigame(Zone zone)
    {
        if (zoneMinigameRunning || encounterRunning || gameEnded) return;
        if (zone == null) return;

        if (knight != null && knight.currentZone == zone)
        {
            Debug.Log($"Not starting zone minigame {zone.zoneName}, as knight is there.");
            return;
        }

        if (zone.isMinigameCompleted) return;
        if (string.IsNullOrEmpty(zone.minigameSceneName)) return;
        if (!Application.CanStreamedLevelBeLoaded(zone.minigameSceneName))
        {
            Debug.LogWarning($"Zone minigame scene '{zone.minigameSceneName}' cannot be loaded. Check Build Settings.");
            return;
        }

        zoneMinigameRunning = true;
        activeZoneMinigameScene = zone.minigameSceneName;
        currentMinigameZone = zone;

        player?.DisableMoveImmediate();
        SceneManager.LoadScene(activeZoneMinigameScene, LoadSceneMode.Additive);
    }

    public void OnZoneMinigameFinished(bool success, int spreadPoints = 0)
    {
        if (!string.IsNullOrEmpty(activeZoneMinigameScene)
         && SceneManager.GetSceneByName(activeZoneMinigameScene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(activeZoneMinigameScene);
        }

        if (currentMinigameZone != null)
        {
            currentMinigameZone.MarkMinigameCompleted();

            if (success)
            {
                minigamesCompleted++;
            }

            currentMinigameZone = null;
        }

        UpdateHUD();
        zoneMinigameRunning = false;
        activeZoneMinigameScene = null;

        if (knight != null)
            knight.TakeTurn(player.currentZone);
        else
            StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        if (encounterRunning || gameEnded) return;
        if (player == null) { return; }
        player.EnableMove();
    }

    public void EndPlayerTurn()
    {
        if (encounterRunning || gameEnded) return;
        if (knight == null) { return; }
        knight.TakeTurn(player.currentZone);
    }

    public void EndKnightTurn()
    {
        if (gameEnded) return;
        StartPlayerTurn();
    }
}