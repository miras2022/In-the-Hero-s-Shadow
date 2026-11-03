using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController1 : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveInput;

    public ScoreManager scoreManager; // ссылка на ScoreManager

    void Update()
    {
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f) * speed * Time.deltaTime;
        transform.Translate(movement);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Crow"))
        {
            Crow crow = collision.GetComponent<Crow>();
            if (crow != null)
            {
                crow.CatchCrow(scoreManager); // ловим ворона
            }
        }
    }
}