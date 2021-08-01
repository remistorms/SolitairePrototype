using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHelper : MonoBehaviour
{
    public static CardHelper Instance;

    [Header("Suit Sprites")]
    [SerializeField] private Sprite m_clubSprite;
    [SerializeField] private Sprite m_diamondSprite;
    [SerializeField] private Sprite m_heartSprite;
    [SerializeField] private Sprite m_spadeSprite;

    [Header("Value Sprites")]
    [SerializeField] private Sprite[] m_valueSprites;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Sprite GetSuitSprite(CardSuit suit)
    {
        Sprite selectedSprite = null;

        switch (suit)
        {
            case CardSuit.Clubs:
                selectedSprite = m_clubSprite;
                break;

            case CardSuit.Diamonds:
                selectedSprite = m_diamondSprite;
                break;

            case CardSuit.Hearts:
                selectedSprite = m_heartSprite;
                break;

            case CardSuit.Spades:
                selectedSprite = m_spadeSprite;
                break;

            default:
                break;
        }

        return selectedSprite;
    }

    public Sprite GetValueSprite(int value)
    {
        return m_valueSprites[value];
    }

    public CardColor GetCardColor(CardSuit cardSuit)
    {
        CardColor selectedColor = CardColor.Black;

        switch (cardSuit)
        {
            case CardSuit.Clubs:
                selectedColor = CardColor.Black;
                break;
            case CardSuit.Diamonds:
                selectedColor = CardColor.Red;
                break;
            case CardSuit.Hearts:
                selectedColor = CardColor.Red;
                break;
            case CardSuit.Spades:
                selectedColor = CardColor.Black;
                break;
            default:
                break;
        }

        return selectedColor;
    }

    public Color GetColorFromSuit(CardSuit cardSuit)
    {
        Color color = Color.black;

        switch (cardSuit)
        {
            case CardSuit.Clubs:
                color = Color.black;
                break;
            case CardSuit.Diamonds:
                color = Color.red;
                break;
            case CardSuit.Hearts:
                color = Color.red;
                break;
            case CardSuit.Spades:
                color = Color.black;
                break;
            default:
                break;
        }

        return color;
    }
}
