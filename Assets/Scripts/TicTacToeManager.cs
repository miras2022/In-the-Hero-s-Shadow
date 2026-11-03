using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TicTacToeManager : MonoBehaviour
{
    public Button[] buttons; // 9 кнопок поля
    public Text resultText; // текст с результатом (UI Text)

    private string[] board; // текущее состояние поля
    private bool gameOver = false;
    private string currentPlayer = "X";

    void Start()
    {
        board = new string[9];
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
            buttons[i].GetComponentInChildren<Text>().text = "";
            buttons[i].interactable = true;
            board[i] = "";
        }
        resultText.text = "";
        currentPlayer = "X";
        gameOver = false;
    }

    void OnButtonClick(int index)
    {
        if (gameOver || board[index] != "") return;
        MakeMove(index, currentPlayer);
        if (CheckGameEnd()) return;

        // смена хода и запуск хода компьютера
        currentPlayer = (currentPlayer == "X") ? "O" : "X";
        Invoke(nameof(ComputerMove), 0.5f);
    }

    void ComputerMove()
    {
        if (gameOver) return;

        int move = GetBestMove();
        if (move != -1)
        {
            MakeMove(move, currentPlayer);
            if (CheckGameEnd()) return;
            currentPlayer = (currentPlayer == "X") ? "O" : "X";
        }
    }

    void MakeMove(int index, string player)
    {
        board[index] = player;
        buttons[index].GetComponentInChildren<Text>().text = player;
        buttons[index].interactable = false;
    }

    int GetBestMove()
    {
        // попытка победить
        for (int i = 0; i < 9; i++)
            if (board[i] == "" && WouldWin(i, currentPlayer))
                return i;

        // блокировка противника
        string opponent = (currentPlayer == "X") ? "O" : "X";
        for (int i = 0; i < 9; i++)
            if (board[i] == "" && WouldWin(i, opponent))
                return i;

        // центр или углы
        List<int> preferred = new List<int> { 4, 0, 2, 6, 8 };
        foreach (int pos in preferred)
            if (board[pos] == "") return pos;

        // любая свободная
        for (int i = 0; i < 9; i++)
            if (board[i] == "") return i;

        return -1;
    }

    bool WouldWin(int index, string player)
    {
        string[] temp = (string[])board.Clone();
        temp[index] = player;
        return CheckWinFor(temp, player);
    }

    bool CheckGameEnd()
    {
        if (CheckWin())
        {
            resultText.text = currentPlayer + " выиграл!";
            gameOver = true;
            DisableAllButtons();
            StartCoroutine(ReturnToMainGame());
            return true;
        }
        if (IsDraw())
        {
            resultText.text = "Ничья!";
            gameOver = true;
            DisableAllButtons();
            StartCoroutine(ReturnToMainGame());
            return true;
        }
        return false;
    }

    bool CheckWin()
    {
        return CheckWinFor(board, currentPlayer);
    }

    bool CheckWinFor(string[] b, string player)
    {
        int[,] lines = {
            {0,1,2},{3,4,5},{6,7,8}, // горизонтали
            {0,3,6},{1,4,7},{2,5,8}, // вертикали
            {0,4,8},{2,4,6}          // диагонали
        };
        for (int i = 0; i < lines.GetLength(0); i++)
        {
            int a = lines[i, 0], bb = lines[i, 1], c = lines[i, 2];
            if (b[a] == player && b[bb] == player && b[c] == player)
                return true;
        }
        return false;
    }

    bool IsDraw()
    {
        foreach (string cell in board)
            if (cell == "") return false;
        return true;
    }

    void DisableAllButtons()
    {
        foreach (Button btn in buttons)
            btn.interactable = false;
    }

    IEnumerator ReturnToMainGame()
    {
        yield return new WaitForSeconds(2f); // Show result for 2 seconds

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            // This is a zone minigame
            gm.OnZoneMinigameFinished(true, 0);
        }
        else
        {
            Debug.LogError("GameManager not found!");
        }
    }
}
