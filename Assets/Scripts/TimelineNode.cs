using UnityEngine;

public class TimelineNode : MonoBehaviour
{
    public TimelineEvent EventData;

    public void Initialise(TimelineEvent data)
    {
        EventData = data;
    }
}