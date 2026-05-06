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

    public GameObject closeToPopup;
    public GameObject questionPanel;
    public TextMeshProUGUI questionText;
    public TMP_InputField answerInput;
    public GameObject questionTextObject;
    private GameObject answerInputObject;
    public GameObject cluePanel;
    public TextMeshProUGUI clueText;
    private System.Action onCloseClicked;
    public GameObject mainMenuUI;
    private void Start()
    {
        
    }
    public void ShowClosePopup(System.Action onClick)
    {
        closeToPopup.SetActive(true);
        cluePanel.SetActive(false);
        onCloseClicked = onClick;
    }

    public void OnClosePopupButton()
    {
        closeToPopup.SetActive(false);
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
    public void NextCluePanelShift()
    {
        questionPanel.SetActive(false);
        cluePanel.SetActive(false);
    }

}