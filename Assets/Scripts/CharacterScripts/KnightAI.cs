using UnityEngine;
using System.Collections;

public class KnightAI : MonoBehaviour
{
    public Zone currentZone;
    public Zone lastZone;
    public GameManager gameManager;
    public float thinkDelay = 0.5f;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = FindFirstObjectByType<GameManager>();
    }

    public void TakeTurn(Zone playerZone)
    {
        StartCoroutine(DoMove());
    }

    private IEnumerator DoMove()
    {
        yield return new WaitForSeconds(thinkDelay);

        if (currentZone == null || currentZone.connectedZones.Count == 0)
        {
            gameManager.EndKnightTurn();
            yield break;
        }

        lastZone = currentZone;

        Zone nextZone = currentZone.connectedZones[Random.Range(0, currentZone.connectedZones.Count)];
        MoveTo(nextZone);

        Debug.Log($"🛡 Рыцарь перешёл в зону: {nextZone.zoneName}");

        if (gameManager != null)
            gameManager.NotifyKnightMoved(true);

        // ✅ Исправлено: проверяем пересечение с зоной игрока, а не текущую зону рыцаря
        if (!gameManager.TryStartEncounterIfZone(gameManager.player.currentZone))
        {
            gameManager.EndKnightTurn();
        }
    }

    private void MoveTo(Zone zone)
    {
        transform.position = zone.transform.position;
        currentZone = zone;
    }
}