using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

   
    [Header("Panels")]
    public GameObject closeToPopup;
    public GameObject questionPanel;
    public GameObject cluePanel;
    public GameObject mainMenuUI;


    [Header("Question UI")]
    public TextMeshProUGUI questionText;
    public TMP_InputField answerInput;
    public GameObject questionTextObject;
    private GameObject answerInputObject;


    [Header("Clue UI")]
    public TextMeshProUGUI clueText;


    [Header("Timeline / Drag & Drop")]
    [SerializeField] private TimelineSlot slotPrefab;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private TimelineNode nodePrefab;
    [SerializeField] private Transform nodeContainer;
    private List<TimelineSlot> currentSlots = new List<TimelineSlot>();
    public TimelinePuzzle timelinePuzzle;

    [Header("Internal State")]
    private System.Action onCloseClicked;
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
        answerInput.gameObject.SetActive(true);
        questionText.text = clue.question;
    }
    public void ShowDragAndDropQuestion(ClueData clue)
    {
        questionPanel.SetActive(true);
        questionText.text = clue.question;

        answerInput.gameObject.SetActive(false);


        // Clear old puzzle objects
        foreach (Transform child in nodeContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }


        // Create a copy of events
        List<TimelineEvent> shuffledEvents =
            new List<TimelineEvent>(clue.timelineEvents);


        // Shuffle events
        for (int i = shuffledEvents.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            TimelineEvent temp = shuffledEvents[i];
            shuffledEvents[i] = shuffledEvents[randomIndex];
            shuffledEvents[randomIndex] = temp;
        }


        // Generate matching slots and nodes
        foreach (TimelineEvent entry in shuffledEvents)
        {
            // Create slot
            TimelineSlot slot =
                Instantiate(slotPrefab, slotContainer);
            currentSlots.Add(slot);


            // Create node
            TimelineNode node =
                Instantiate(nodePrefab, nodeContainer);


            node.Initialise(entry);
        }
    }
    public void ShowPuzzle(ClueData clue)
    {
        switch (clue.puzzleType)
        {
            case PuzzleType.TextAnswer:
            ShowQuestion(clue);
            break;

            case PuzzleType.DragAndDrop:
            ShowDragAndDropQuestion(clue);
            break;

            
        }
    }

    public void SubmitAnswer()
    {
        //FindFirstObjectByType<ClueManager>().SubmitAnswer(answerInput.text);
        FindFirstObjectByType<ClueManager>().CheckCurrentPuzzle();
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