using System.Collections.Generic;
using UnityEngine;

public class DragDropManager : MonoBehaviour
{
    public static DragDropManager Instance;

    [Header("Assign")]
    public RectTransform dragLayer;
    public RectTransform playZoneRect;

    public bool CanInteract { get; set; } = true;

    private readonly HashSet<CardView> marked = new();

    void Awake() => Instance = this;

    public void MarkForPlay(CardView card)
    {
        card.ToggleSelect();
    }

    public void ClearMarked()
    {
        marked.Clear();
    }
}