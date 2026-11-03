using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Zone : MonoBehaviour
{
    public string zoneName;
    public List<Zone> connectedZones = new List<Zone>();

    [Header("Anchors (Optional)")]
    public Transform playerAnchor; // point for player
    public Transform knightAnchor; // point for knight

    [Header("Minigame")]
    public string minigameSceneName; // scene name for this zone's minigame
    public bool isMinigameCompleted = false; // track if minigame was already played

    /// <summary>
    /// Подсветка зоны (жёлтый = активна, белый = неактивна)
    /// </summary>
    public void Highlight(bool on)
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = on ? Color.yellow : Color.white;
        }
    }

    /// <summary>
    /// Отметить мини-игру в этой зоне как завершённую
    /// </summary>
    public void MarkMinigameCompleted()
    {
        isMinigameCompleted = true;
        Debug.Log($"✅ Zone '{zoneName}' minigame marked as completed");
    }
}
