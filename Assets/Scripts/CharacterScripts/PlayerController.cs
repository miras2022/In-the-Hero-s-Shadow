using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Zone currentZone;
    private bool canMove = false;
    private GameManager gameManager;
    private Camera cam;
    public void DisableMove() => canMove = false;

    private void Awake()
    {
        cam = Camera.main;
        gameManager = Object.FindFirstObjectByType<GameManager>();

    }

    public void MoveTo(Zone targetZone)
    {
        // ”бираем запуск мини-игры здесь Ч делаем это централизованно
        transform.position = targetZone.transform.position;
        currentZone = targetZone;
        canMove = false;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!canMove) return;

        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, -cam.transform.position.z));

        Collider2D col = Physics2D.OverlapPoint(worldPos);
        if (col == null) return;

        Zone clickedZone = col.GetComponent<Zone>();
        if (clickedZone == null) return;

        if (currentZone != null && currentZone.connectedZones.Contains(clickedZone))
        {
            transform.position = clickedZone.transform.position;
            currentZone = clickedZone;

            // ÷ентрализованный вызов логики входа в зону,
            // где решаетс€ кака€ мини-игра запускатьс€
            gameManager.AttemptZoneEntry(currentZone);

            canMove = false;
        }
    }

    public void EnableMove()
    {
        canMove = true;
    }
    public void DisableMoveImmediate() { canMove = false; }
}
