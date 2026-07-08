using UnityEngine;

public class TimelineSlot : MonoBehaviour
{
    public TimelineNode placedNode;

    public bool IsOccupied()
    {
        return placedNode != null;
    }

    public void PlaceNode(TimelineNode node)
    {
        placedNode = node;

        node.transform.SetParent(transform);
        node.transform.position = transform.position;
    }
}