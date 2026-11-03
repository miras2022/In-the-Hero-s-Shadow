using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerTMP : MonoBehaviour
{
    public Transform boardPanel;
    public GameObject cardPrefab;
    public int rows = 4;
    public int cols = 4;
    public float revealDelay = 0.8f;

    private CardTMP firstCard;
    private CardTMP secondCard;
    private bool busy = false;
    private int matchesFound = 0;
    private int totalPairs;
    private bool gameEnded = false;

    private List<string> words = new List<string>()
    {
        "меч", "щит", "зелье", "кольцо",
        "книга", "лук", "посох", "камень"
    };

    void Start()
    {
        SetupBoard();
    }

    public void SetupBoard()
    {
        foreach (Transform t in boardPanel) Destroy(t.gameObject);

        int total = rows * cols;
        totalPairs = total / 2;

        List<string> selected = new List<string>();
        for (int i = 0; i < totalPairs; i++)
        {
            if (i < words.Count) selected.Add(words[i]);
            else selected.Add("слово" + (i + 1));
        }

        List<string> all = new List<string>();
        foreach (var w in selected) { all.Add(w); all.Add(w); }

        for (int i = 0; i < all.Count; i++)
        {
            int r = Random.Range(i, all.Count);
            var tmp = all[i];
            all[i] = all[r];
            all[r] = tmp;
        }

        for (int i = 0; i < all.Count; i++)
        {
            GameObject go = Instantiate(cardPrefab, boardPanel);
            CardTMP card = go.GetComponent<CardTMP>();
            if (card == null) { Debug.LogError("CardPrefab должен содержать CardTMP script"); continue; }
            card.Initialize(all[i], this);
        }

        firstCard = null;
        secondCard = null;
        busy = false;
        matchesFound = 0;
        gameEnded = false;
    }

    public void OnCardClicked(CardTMP c)
    {
        if (busy) return;
        if (c.IsRevealed || c.IsMatched) return;

        c.Reveal();

        if (firstCard == null) { firstCard = c; return; }
        if (secondCard == null)
        {
            secondCard = c;
            StartCoroutine(CheckMatchRoutine());
        }
    }

    private IEnumerator CheckMatchRoutine()
    {
        busy = true;

        if (firstCard.word == secondCard.word)
        {
            yield return new WaitForSeconds(0.12f);
            firstCard.SetMatched();
            secondCard.SetMatched();
            matchesFound++;

            firstCard = null;
            secondCard = null;
            busy = false;

            if (matchesFound >= totalPairs)
            {
                OnGameWon();
            }
        }
        else
        {
            yield return new WaitForSeconds(revealDelay);
            firstCard.Hide();
            secondCard.Hide();
            firstCard = null;
            secondCard = null;
            busy = false;
        }
    }

    private void OnGameWon()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("🎉 Победа! Все пары слов найдены!");
        StartCoroutine(ReturnToMainGame(1.5f));
    }

    private IEnumerator ReturnToMainGame(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Try to notify GameManager first (if using additive loading)
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            Debug.Log("✅ Returning via GameManager");
            gm.OnZoneMinigameFinished(true, 1); // true = won, 1 = reward
        }
        else
        {
            // Fallback: just unload this minigame scene
            Debug.Log("⚠ GameManager not found, unloading minigame scene");
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.UnloadSceneAsync(currentScene);
        }
    }
}
