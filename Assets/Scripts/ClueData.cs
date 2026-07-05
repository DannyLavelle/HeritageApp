using UnityEngine;

[CreateAssetMenu(fileName = "NewClue", menuName = "Clue System/Clue")]
public class ClueData : ScriptableObject
{
    [Header("Clue Info")]
    public string clueText;

    [Header("Location")]
    public double latitude;
    public double longitude;
    public float triggerDistanceMetres = 20f; // meters

    [Header("Question")]
    public string question;
    public string correctAnswer;

    [Header("Reward")]
    public string badgeName;

    [Header("Puzzle Type")]
    public PuzzleType puzzleType;

    // Timeline
    public string[] timelineEvents;

    // Multiple Choice
    public string[] options;
    public int correctOptionIndex;

}