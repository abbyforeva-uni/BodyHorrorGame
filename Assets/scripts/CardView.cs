using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public CardData data;

    [Header("Assign in prefab")]
    public Image frameImage;
    public Image artImage;
    public TMP_Text nameText;
    public TMP_Text valueText;
    public TMP_Text descriptionText;
    public CanvasGroup canvasGroup;

    [Header("Drag tuning")]
    public float dragScale = 1.15f;
    public float returnSpeed = 12f;

    private RectTransform rect;
    private Transform homeParent;
    private Vector2 homeAnchoredPos;
    private Vector3 homeScale;
    private bool selected;

    public bool IsSelected => selected;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        homeScale = transform.localScale;
    }

    public void Setup(CardData cardData)
    {
        data = cardData;
        nameText.text = cardData.cardName;
        valueText.text = cardData.value.ToString();
        valueText.color = cardData.valueColor;
        descriptionText.text = cardData.description;
        if (cardData.cardArt) artImage.sprite = cardData.cardArt;
        if (cardData.cardFrame) frameImage.sprite = cardData.cardFrame;
    }

    public void SetHome(Transform parent)
    {
        homeParent = parent;
        rect.SetParent(parent, false);
        homeAnchoredPos = rect.anchoredPosition;
    }

    public void ToggleSelect()
    {
        selected = !selected;
        transform.localScale = selected ? homeScale * dragScale : homeScale;
    }

    public void ClearSelect()
    {
        selected = false;
        transform.localScale = homeScale;
    }

    public void OnPointerClick(PointerEventData e)
    {
        if (DragDropManager.Instance != null && DragDropManager.Instance.CanInteract)
            ToggleSelect();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        if (DragDropManager.Instance == null || !DragDropManager.Instance.CanInteract) return;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(DragDropManager.Instance.dragLayer, false);
        transform.localScale = homeScale * dragScale;
    }

    public void OnDrag(PointerEventData e)
    {
        if (DragDropManager.Instance == null || !DragDropManager.Instance.CanInteract) return;
        rect.position = e.position;
    }

    public void OnEndDrag(PointerEventData e)
    {
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(homeParent, false);
        rect.anchoredPosition = homeAnchoredPos;
        transform.localScale = selected ? homeScale * dragScale : homeScale;

        if (DragDropManager.Instance != null && DragDropManager.Instance.playZoneRect != null
            && RectTransformUtility.RectangleContainsScreenPoint(DragDropManager.Instance.playZoneRect, e.position, e.pressEventCamera))
        {
            DragDropManager.Instance.MarkForPlay(this);
        }
    }
}