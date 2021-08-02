using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public static class EventsManager
{
    public static event Action<Card, CardPile> OnCardDropped = delegate{};
    public static void Fire_evt_OnCardDropped(Card card, CardPile pile)
    {
        //Debug.Log( card.name + " was dropped onto " + pile.name );
        OnCardDropped( card, pile );
    }

    public static event Action<Card> OnCardBeginDrag = delegate { };
    public static void Fire_evt_OnCardBeginDrag(Card card)
    {
        //Debug.Log(card.name + " started being dragged.");
        OnCardBeginDrag( card );
    }

    public static event Action<Card> OnCardEndedDrag = delegate { };
    public static void Fire_evt_OnCardEndedDrag(Card card)
    {
        //Debug.Log(card.name + " ended drag.");
        OnCardEndedDrag(card);
    }
}
