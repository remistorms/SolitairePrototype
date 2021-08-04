using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public static class EventsManager
{
    public static event Action<Card, CardPile> OnCardDroppedOnPile = delegate{};
    public static void Fire_evt_OnCardDroppedOnPile(Card card, CardPile pile)
    {
        //Debug.Log( "EventsManager: " + card.name + " was dropped onto " + pile.name );
        OnCardDroppedOnPile( card, pile );
    }

    //DRAG EVENTS
    //Start Drag
    public static event Action<Card, PointerEventData> OnCardDragStarted = delegate { };
    public static void Fire_evt_OnCardDragStarted(Card card, PointerEventData pointerEventData)
    {
       // Debug.Log("EventsManager: " + card.name + " drag START");
        OnCardDragStarted( card, pointerEventData);
    }

    //UpdateDrag
    public static event Action<Card, PointerEventData> OnCardDragUpdate = delegate { };
    public static void Fire_evt_OnCardDragUpdate(Card card, PointerEventData pointerEventData)
    {
      //  Debug.Log("EventsManager: " + card.name + " drag UPDATE");
        OnCardDragUpdate(card, pointerEventData);
    }

    //End Drag
    public static event Action<Card, PointerEventData> OnCardDragEnded = delegate { };
    public static void Fire_evt_OnCardDragEnded(Card card, PointerEventData pointerEventData)
    {
       // Debug.Log("EventsManager: " + card.name + " drag END");
        OnCardDragEnded(card, pointerEventData);
    }

    //Fire_evt_ClickedOnCard
    public static event Action<Card, PointerEventData> OnClickedOnCard = delegate { };
    public static void Fire_evt_OnClickedOnCard(Card card, PointerEventData pointerEventData)
    {
        //Debug.Log("EventsManager: " + card.name + " was clicked on");
        OnClickedOnCard(card, pointerEventData);
    }

    public static event Action<Card, CardPile, bool, bool> OnCardStackCheck = delegate { };
    public static void Fire_evt_OnCardStackCheck(Card card, CardPile pile, bool canStack, bool hasSwitchedPiles)
    {
        Debug.Log(" card " + card.name + " was dropped onto " + pile.gameObject.name + " stackable status -> " + canStack);
        OnCardStackCheck(card, pile, canStack, hasSwitchedPiles);
    }

    public static event Action OnRequestDrawCards = delegate { };
    public static void Fire_evt_RequestDrawCards()
    {
        OnRequestDrawCards();
    }
}
