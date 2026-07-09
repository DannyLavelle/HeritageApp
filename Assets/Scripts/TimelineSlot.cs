using UnityEngine;
using UnityEngine.EventSystems;

public class TimelineSlot : MonoBehaviour, IDropHandler
{
    public TimelineNode placedNode;

    public void OnDrop(PointerEventData eventData)
    {
        TimelineNode node = eventData.pointerDrag.GetComponent<TimelineNode>();

        if (node == null)
            return;

        if (placedNode != null)
            return;

        // Remove the node from its previous slot
        if (node.currentSlot != null)
        {
            node.currentSlot.placedNode = null;
        }

        placedNode = node;
        node.currentSlot = this;

        node.transform.SetParent(transform);
        node.transform.localPosition = Vector3.zero;
    }
}