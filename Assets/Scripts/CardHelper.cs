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

    public string GetCardValueString(CardValue cardValue)
    {
        string stringValue = "";

        switch (cardValue)
        {
            case CardValue.Ace:
                stringValue = "A";
                break;
            case CardValue.Two:
                stringValue = "2";
                break;
            case CardValue.Three:
                stringValue = "3";
                break;
            case CardValue.Four:
                stringValue = "4";
                break;
            case CardValue.Five:
                stringValue = "5";
                break;
            case CardValue.Six:
                stringValue = "6";
                break;
            case CardValue.Seven:
                stringValue = "7";
                break;
            case CardValue.Eight:
                stringValue = "8";
                break;
            case CardValue.Nine:
                stringValue = "9";
                break;
            case CardValue.Ten:
                stringValue = "10";
                break;
            case CardValue.Jack:
                stringValue = "J";
                break;
            case CardValue.Queen:
                stringValue = "Q";
                break;
            case CardValue.King:
                stringValue = "K";
                break;
            default:
                break;
        }

        return stringValue;
    }

    public CardSuit GetCardSuitFromIndex(int index)
    {
        CardSuit cardSuit = CardSuit.Clubs;

        switch (index)
        {
            case 0:
                cardSuit = CardSuit.Clubs;
                break;
            case 1:
                cardSuit = CardSuit.Diamonds;
                break;
            case 2:
                cardSuit = CardSuit.Hearts;
                break;
            case 3:
                cardSuit = CardSuit.Spades;
                break;

            default:
                break;
        }

        return cardSuit;
    }

    //Converts an int value from 0 to 12 into a CardValue Enum
    public CardValue GetCardValueFromInt(int intValue)
    {
        CardValue cardValue = CardValue.Ace;

        if (intValue == 0)
        {
            cardValue = CardValue.Ace;
        }
        else if (intValue == 1)
        {
            cardValue = CardValue.Two;
        }
        else if (intValue == 2)
        {
            cardValue = CardValue.Three;
        }
        else if (intValue == 3)
        {
            cardValue = CardValue.Four;
        }
        else if (intValue == 4)
        {
            cardValue = CardValue.Five;
        }
        else if (intValue == 5)
        {
            cardValue = CardValue.Six;
        }
        else if (intValue == 6)
        {
            cardValue = CardValue.Seven;
        }
        else if (intValue == 7)
        {
            cardValue = CardValue.Eight;
        }
        else if (intValue == 8)
        {
            cardValue = CardValue.Nine;
        }
        else if (intValue == 9)
        {
            cardValue = CardValue.Ten;
        }
        else if (intValue == 10)
        {
            cardValue = CardValue.Jack;
        }
        else if (intValue == 11)
        {
            cardValue = CardValue.Queen;
        }
        else if (intValue == 12)
        {
            cardValue = CardValue.King;
        }

        return cardValue;
    }

    public int GetValueDifference(CardValue topValue, CardValue botValue)
    {
        int valueDifference = (int)topValue - (int)botValue;
        Debug.Log( "value difference = " + valueDifference);
        return valueDifference;
    }

    //
    /*
    public bool CheckIfCanStack(Card upCard, Card downCard, bool isFinalPile = false)
    {
        if (isFinalPile)
        {
            //only return true if both cards are the same suit and if up card value is +1 value from down card
            if (upCard.m_cardColor == downCard.m_cardColor && CardHelper.Instance.GetValueDifference(upCard.m_cardValue, downCard.m_cardValue) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            //only return true if both cards are different card color and if up card value is -1 value from down card
            if (upCard.m_cardColor != downCard.m_cardColor && CardHelper.Instance.GetValueDifference(upCard.m_cardValue, downCard.m_cardValue) == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    */

    public bool CheckIfCanStack(Card card, CardPile pile)
    {
        bool stackResult = false;

        //Receiving Pile is Empty and Is END Pile
        if (pile.m_cardsInPile.Count <= 0 && pile.m_pileType == PileType.EndPile)
        {
            //Only return true if is an ace of same suit
            if (card.m_cardSuit == pile.m_finalPileSuit && card.m_cardValue == CardValue.Ace)
            {
                stackResult = true;
            }
        }
        //Receiving Pile is Empty and Is GAME PILE
        else if (pile.m_cardsInPile.Count <= 0 && pile.m_pileType == PileType.GamePile)
        {
            //Only return true if is a KING
            if (card.m_cardValue == CardValue.King)
            {
                stackResult = true;
            }
        }
        //Receiving Pile is NOT Empty and Is END PILE
        else if (pile.m_cardsInPile.Count > 0 && pile.m_pileType == PileType.EndPile)
        {
            //Only return true if ascending order and same suit
            if (card.m_cardSuit == pile.m_finalPileSuit && GetValueDifference(card.m_cardValue, pile.GetTopCard().m_cardValue) == 1)
            {
                stackResult = true;
            }
        }
        //Receiving Pile is NOT Empty and Is GAME PILE
        else if (pile.m_cardsInPile.Count > 0 && pile.m_pileType == PileType.GamePile)
        {
            //Only return true if descending order and different color
            if (card.m_cardColor != pile.GetTopCard().m_cardColor && GetValueDifference(card.m_cardValue, pile.GetTopCard().m_cardValue) == -1)
            {
                stackResult = true;
            }
        }
        else
        {
            stackResult = false;
        }

        return stackResult;
    }
}
