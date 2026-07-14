using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TimelineNode : MonoBehaviour,
        IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public TimelineEvent eventData;
    public TimelineSlot currentSlot;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();
    }

    public void Initialise(TimelineEvent data)
    {
        eventData = data;
        titleText.text = data.title;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        canvasGroup.blocksRaycasts = false;

        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
        }
    }
}