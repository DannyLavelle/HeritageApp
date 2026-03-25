using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public GameObject closePopup;
    public GameObject questionPanel;
    public Text questionText;
    public InputField answerInput;
    public GameObject cluePanel;
    public TextMeshProUGUI clueText;
    private System.Action onCloseClicked;
    public GameObject mainMenuUI;
    public void ShowClosePopup(System.Action onClick)
    {
        closePopup.SetActive(true);
        onCloseClicked = onClick;
    }

    public void OnClosePopupButton()
    {
        closePopup.SetActive(false);
        onCloseClicked?.Invoke();
    }

    public void ShowQuestion(ClueData clue)
    {
        questionPanel.SetActive(true);
        questionText.text = clue.question;
    }

    public void SubmitAnswer()
    {
        FindFirstObjectByType<ClueManager>().SubmitAnswer(answerInput.text);
    }

    public void ShowBadge(string badgeName)
    {
        Debug.Log("Awarded badge: " + badgeName);
    }

    public void ShowClue(string text)
    {
        cluePanel.SetActive(true);
        clueText.text = text;
        mainMenuUI.SetActive(false);
        Debug.Log("Showing clue: " + text);
    }

}