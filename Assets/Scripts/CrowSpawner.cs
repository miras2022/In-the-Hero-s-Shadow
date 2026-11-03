using UnityEngine;

public class CrowSpawner : MonoBehaviour
{
    public GameObject crowPrefab;
    public Transform[] targets;
    public float spawnInterval = 1f;
    public int maxCrows = 10;

    private int spawnedCrows = 0;
    private float timer = 0f;

    void Update()
    {
        if (spawnedCrows >= maxCrows) return; // лимит воронов

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCrow();
            timer = 0f;
        }
    }

    void SpawnCrow()
    {
        if (crowPrefab == null || targets.Length == 0) return;

        GameObject crow = Instantiate(crowPrefab, transform.position, Quaternion.identity);
        Crow crowScript = crow.GetComponent<Crow>();
        crowScript.target = targets[Random.Range(0, targets.Length)];

        spawnedCrows++;
    }
}
