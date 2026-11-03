using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SequenceMemoryGame : MonoBehaviour
{
    public Button[] tiles;
    public Button RealStartButton;
    public Button LeaveButton;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI RealStartButtonText;

    [Header("Scene Settings")]
    public string mainGameSceneName = "Game"; // 👈 Your main game scene

    private List<int> activeButtons = new List<int>();
    private List<int> sequence = new List<int>();
    private List<int> userSequence = new List<int>();
    private int level = 1;
    private bool isPlayerTurn = false;
    private bool isGameActive = false;

    private int[] buttonsPerLevel = { 3, 5, 6 };
    private int[] repeatsPerLevel = { 4, 6, 6 };
    private float[] speeds = { 0.5f, 0.5f, 0.5f };

    void Start()
    {
        if (RealStartButton == null || levelText == null || tiles == null || tiles.Length != 6 || LeaveButton == null || RealStartButtonText == null)
        {
            Debug.LogError("Проверьте, что все нужные поля назначены в инспекторе!");
            return;
        }

        RealStartButton.gameObject.SetActive(true);
        LeaveButton.gameObject.SetActive(false);
        RealStartButtonText.text = "Start";

        RealStartButton.onClick.RemoveAllListeners();
        RealStartButton.onClick.AddListener(StartGame);
        LeaveButton.onClick.RemoveAllListeners();
        LeaveButton.onClick.AddListener(OnLeaveButton);

        foreach (Button tile in tiles)
        {
            if (tile != null)
                tile.onClick.RemoveAllListeners();
        }

        foreach (Button tile in tiles)
        {
            if (tile != null)
                tile.onClick.AddListener(() => OnTileClicked(tile));
        }

        ActivateAllButtons(false);
        levelText.text = "Memory Game";
    }

    void StartGame()
    {
        RealStartButton.gameObject.SetActive(false);
        LeaveButton.gameObject.SetActive(false);
        sequence.Clear();
        userSequence.Clear();
        level = 1;
        isGameActive = true;

        levelText.text = "Level: " + level;
        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel()
    {
        ActivateButtonsForLevel(level);

        userSequence.Clear();
        sequence.Clear();
        isPlayerTurn = false;

        List<int> candidateButtons = new List<int>(activeButtons);

        int toRepeat = repeatsPerLevel[level - 1];
        for (int i = 0; i < toRepeat; i++)
        {
            int idx = candidateButtons[Random.Range(0, candidateButtons.Count)];
            sequence.Add(idx);
        }

        levelText.text = $"Level: {level} - Watch!";
        yield return new WaitForSeconds(0.7f);
        foreach (int idx in sequence)
        {
            Button tile = tiles[idx];
            Color orig = tile.image.color;
            tile.image.color = Color.red;
            yield return new WaitForSeconds(speeds[level - 1]);
            tile.image.color = orig;
            yield return new WaitForSeconds(speeds[level - 1] * 0.35f);
        }

        isPlayerTurn = true;
        levelText.text = $"Level: {level} - Repeat!";
    }

    void OnTileClicked(Button clickedTile)
    {
        if (!isPlayerTurn || !isGameActive) return;

        int idx = System.Array.IndexOf(tiles, clickedTile);
        if (!activeButtons.Contains(idx)) return;

        userSequence.Add(idx);

        if (userSequence[userSequence.Count - 1] != sequence[userSequence.Count - 1])
        {
            GameOver();
            return;
        }

        if (userSequence.Count == sequence.Count)
        {
            if (level == 3)
                StartCoroutine(YouWin());
            else
            {
                level++;
                StartCoroutine(StartLevel());
            }
        }
    }

    void GameOver()
    {
        isPlayerTurn = false;
        isGameActive = false;
        levelText.text = "Game Over!";
        ActivateAllButtons(false);

        RealStartButton.gameObject.SetActive(true);
        RealStartButtonText.text = "Restart";

        RealStartButton.onClick.RemoveAllListeners();
        RealStartButton.onClick.AddListener(StartGame);

        LeaveButton.gameObject.SetActive(true);

        sequence.Clear();
        userSequence.Clear();
    }

    IEnumerator YouWin()
    {
        isPlayerTurn = false;
        isGameActive = false;
        levelText.text = "You Win!";
        yield return new WaitForSeconds(2f);

        ReturnToMainGame(true);
    }

    void ActivateButtonsForLevel(int lvl)
    {
        List<int> all = new List<int>(System.Linq.Enumerable.Range(0, tiles.Length));
        activeButtons = new List<int>(all.OrderBy(x => Random.value).Take(buttonsPerLevel[lvl - 1]));

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].gameObject.SetActive(activeButtons.Contains(i));
        }
    }

    void ActivateAllButtons(bool active)
    {
        foreach (var b in tiles)
            if (b != null) b.gameObject.SetActive(active);
    }

    void OnLeaveButton()
    {
        ReturnToMainGame(false);
    }

    void ReturnToMainGame(bool playerWon)
    {
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
                Debug.LogError($"❌ Scene '{mainGameSceneName}' not found in Build Settings! Add it to File > Build Settings.");
            }
        }
    }
}
