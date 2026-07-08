using UnityEngine;

[System.Serializable]
public class TimelineEvent
{
    public string title;

    [TextArea]
    public string description;

    public int correctPosition;
}