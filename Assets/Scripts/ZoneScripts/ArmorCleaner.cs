using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ArmorCleaner : MonoBehaviour
{
    public RectTransform cloth;         // Тряпка
    public Image armorImage;            // Доспех
    public Slider cleanBar;             // Индикатор чистоты
    public TextMeshProUGUI resultText;  // Текст результата
    public float cleanSpeed = 0.2f;     // Скорость чистки
    public float maxDistance = 150f;    // Максимальное расстояние для чистки
    public Sprite clearedSprite;        // Новый спрайт после очистки (tors_clear.png)

    private bool isCleaning = false;
    private bool gameOver = false;

    void Update()
    {
        if (gameOver) return;

        HandleMovement();

        float distance = Vector2.Distance(cloth.anchoredPosition, armorImage.rectTransform.anchoredPosition);

        // Проверяем расстояние
        if (distance > maxDistance)
        {
            // Если слишком далеко, останавливаем чистку
            isCleaning = false;
        }

        if (isCleaning)
        {
            cleanBar.value += cleanSpeed * Time.deltaTime;
            if (cleanBar.value >= 1f)
            {
                cleanBar.value = 1f;
                resultText.text = "Блестит как новое!";
                resultText.color = Color.yellow;
                gameOver = true;

                // Меняем спрайт на tors_clear.png
                if (clearedSprite != null)
                {
                    armorImage.sprite = clearedSprite;
                }

                StartCoroutine(ExitAfterDelay());
            }
        }
    }

    void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) moveY = 1f;
        if (Input.GetKey(KeyCode.S)) moveY = -1f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        Vector3 move = new Vector3(moveX, moveY, 0f) * 200f * Time.deltaTime;
        cloth.anchoredPosition += new Vector2(move.x, move.y);

        // Если есть движение — чистим, но только если внутри допустимой зоны
        float distance = Vector2.Distance(cloth.anchoredPosition, armorImage.rectTransform.anchoredPosition);
        isCleaning = (moveX != 0 || moveY != 0) && distance <= maxDistance;
    }

    IEnumerator ExitAfterDelay()
    {
        yield return new WaitForSeconds(3f);  // ждём 3 секунды

        // Находим GameManager из основной сцены
        var gm = Object.FindFirstObjectByType<GameManager>();

        if (gm != null)
        {
            // сообщаем, что мини-игра завершена успешно
            gm.OnEncounterFinished(success: true);
        }
        else
        {
            Debug.LogWarning("GameManager не найден — не удалось вернуться к основной игре.");
        }

        // выгружаем текущую мини-сцену
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(scene.name != null)
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene.name);
    }
}
