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

    //Card Drop
    public static event Action<Card, CardPile> OnCardDropped = delegate { };
    public static void Fire_evt_CardDroppedOnPile(Card card, CardPile pile)
    {
        Debug.Log("EventsManager: " + card.name + " dropped onto " + pile.name);
        OnCardDropped(card, pile);
    }

    //Fire_evt_ClickedOnCard
    public static event Action<Card, PointerEventData> OnClickedOnCard = delegate { };
    public static void Fire_evt_OnClickedOnCard(Card card, PointerEventData pointerEventData)
    {
        //Debug.Log("EventsManager: " + card.name + " was clicked on");
        OnClickedOnCard(card, pointerEventData);
    }

    public static event Action<Card, CardPile, bool> OnCardStackCheck = delegate { };
    public static void Fire_evt_OnCardStackCheck(Card card, CardPile pile, bool canStack)
    {
        Debug.Log(" card " + card.name + " was dropped onto " + pile.gameObject.name + " stackable status -> " + canStack);
        OnCardStackCheck(card, pile, canStack);
    }

    public static event Action OnRequestDrawCards = delegate { };
    public static void Fire_evt_RequestDrawCards()
    {
        OnRequestDrawCards();
    }

    public static event Action OnDeckReshuffled = delegate { };
    public static void Fire_evt_OnDeckReshuffled()
    {
        OnDeckReshuffled();
    }

    public static event Action<ScreenOrientation> OnScreenOrientationChanged = delegate { };
    public static void Fire_evt_ScreenOrientationChanged(ScreenOrientation newOrientation)
    {
        OnScreenOrientationChanged(newOrientation);
    }

    public static event Action<Turn> OnUndoMovement = delegate { };
    public static void Fire_event_UndoMovement(Turn move)
    {
        Debug.Log("Event player undo");
        OnUndoMovement(move);
    }

    public static event Action<Card> OnCardFlipped = delegate { };
    public static void Fire_event_OnCardFlipped(Card card)
    {
        Debug.Log("fliped card");
        OnCardFlipped(card);
    }
}
