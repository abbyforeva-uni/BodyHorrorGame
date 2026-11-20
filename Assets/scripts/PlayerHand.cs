using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [Header("Assign")]
    public RectTransform handContainer;   // horizontal layout group
    public CardView cardPrefab;

    [Header("Deck (drag assets here)")]
    public List<CardData> deck = new();

    public List<CardView> handViews = new();
    public List<CardData> discardPile = new();

    public void DrawRandom(int count = 3)
    {
        for (int i = 0; i < count; i++)
        {
            if (deck.Count == 0) return;
            int idx = Random.Range(0, deck.Count);
            CardData data = deck[idx];
            CardView view = Instantiate(cardPrefab, handContainer);
            view.Setup(data);
            view.SetHome(handContainer);
            handViews.Add(view);
        }
    }

    public List<CardView> GetSelectedCards()
    {
        var result = new List<CardView>();
        foreach (var c in handViews)
            if (c.IsSelected) result.Add(c);
        return result;
    }

    public void DiscardAllSelected()
    {
        var selected = GetSelectedCards();
        foreach (var c in selected)
        {
            handViews.Remove(c);
            Destroy(c.gameObject);
        }
    }

    public void ClearHand()
    {
        foreach (var c in handViews)
            if (c) Destroy(c.gameObject);
        handViews.Clear();
    }
}