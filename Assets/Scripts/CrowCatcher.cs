using UnityEngine;

public class CrowCatcher : MonoBehaviour
{
    public ScoreManager scoreManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Crow"))
        {
            Destroy(other.gameObject);
            scoreManager.AddScore(1);
        }
    }
}