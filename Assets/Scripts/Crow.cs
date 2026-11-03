using UnityEngine;

public class Crow : MonoBehaviour
{
    public float speed = 2f;       // скорость полета ворона
    public Transform target;        // цель (куда летит ворона)
    private bool isCaught = false;

    void Update()
    {
        if (!isCaught && target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject); // исчезает при достижении цели
            }
        }
    }

    public void CatchCrow(ScoreManager scoreManager)
    {
        if (isCaught) return;

        isCaught = true;
        scoreManager.AddScore(1); // добавляем очко
        Destroy(gameObject);       // исчезает
    }
}