using UnityEngine;

public enum CardType { Attack, Defend, Special }

[CreateAssetMenu(fileName = "NewCard", menuName = "Oranges/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    public int value;              // the bolded number shown on play
    public Color valueColor = Color.red;
    public Sprite cardArt;         // replace with your art
    public Sprite cardFrame;       // replace with your art
    public string description;
}