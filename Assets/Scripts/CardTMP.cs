using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardTMP : MonoBehaviour
{
    public string word;
    public bool IsRevealed { get; private set; }
    public bool IsMatched { get; private set; }

    public TMP_Text wordText;       // перетащить NumberText (TMP) в инспекторе
    public Image backgroundImage;   // перетащить Image (корн€) в инспекторе

    private Button button;
    private GameManagerTMP gameManager;
    private RectTransform rectTransform;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        rectTransform = GetComponent<RectTransform>(); // Get the RectTransform component
    }

    // »нициализаци€ карточки: слово и ссылка на менеджер
    public void Initialize(string w, GameManagerTMP manager)
    {
        word = w;
        gameManager = manager;
        HideImmediate();
        IsMatched = false;

        // Set the card's position in the center of the parent (for UI elements)
        SetCardPositionToCenter();

        // ћожно задать стиль текста (дл€ предзаполнени€), но не об€зательно
        if (wordText) wordText.text = "";
    }

    // Method to set the card's position to the center of its parent container
    private void SetCardPositionToCenter()
    {
        if (rectTransform != null)
        {
            // Set the position to the center of the parent RectTransform
            rectTransform.anchoredPosition = Vector2.zero; // Centers it in the parent
            rectTransform.localScale = Vector3.one; // Ensure it's not scaled (optional)
        }
    }

    void OnClick()
    {
        if (IsMatched || IsRevealed) return;
        gameManager.OnCardClicked(this);
    }

    public void Reveal()
    {
        IsRevealed = true;
        if (wordText)
        {
            wordText.text = word;
            wordText.gameObject.SetActive(true);
        }
        if (backgroundImage) backgroundImage.color = new Color(0.92f, 0.92f, 0.92f);
    }

    public void Hide()
    {
        IsRevealed = false;
        if (wordText) wordText.gameObject.SetActive(false);
        if (backgroundImage) backgroundImage.color = Color.white;
    }

    public void HideImmediate()
    {
        IsRevealed = false;
        if (wordText) { wordText.gameObject.SetActive(false); wordText.text = ""; }
        if (backgroundImage) backgroundImage.color = Color.white;
    }

    public void SetMatched()
    {
        IsMatched = true;
        if (button) button.interactable = false;
        if (backgroundImage) backgroundImage.color = new Color(0.7f, 1f, 0.7f);
    }
}
