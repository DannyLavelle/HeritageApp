using System.Collections.Generic;
using UnityEngine;

public class TimelinePuzzle : MonoBehaviour
{
    public List<TimelineSlot> slots = new List<TimelineSlot>();

    public bool CheckAnswer()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            // Empty slot
            if (slots[i].placedNode == null)
            {
                Debug.Log("Timeline incomplete");
                return false;
            }

            // Wrong position
            if (slots[i].placedNode.eventData.correctPosition != i)
            {
                Debug.Log("Wrong order");
                return false;
            }
        }

        return true;
    }
}