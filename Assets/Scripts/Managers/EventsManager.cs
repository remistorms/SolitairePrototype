using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public static class EventsManager
{
    public static event Action OnGameStarted = delegate{};
    public static void Fire_evt_GameStarted()
    {
        OnGameStarted();
    }

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
        //Debug.Log("EventsManager: " + card.name + " dropped onto " + pile.name);
        OnCardDropped(card, pile);
    }

    //Fire_evt_ClickedOnCard
    public static event Action<Card, PointerEventData> OnClickedOnCard = delegate { };
    public static void Fire_evt_OnClickedOnCard(Card card, PointerEventData pointerEventData)
    {
        //Debug.Log("EventsManager: " + card.name + " was clicked on");
        OnClickedOnCard(card, pointerEventData);
    }

    //Fire_evt_DoubleClickedOnCard
    public static event Action<Card, PointerEventData> OnDoubleClickedOnCard = delegate { };
    public static void Fire_evt_OnDoubleClickedOnCard(Card card, PointerEventData pointerEventData)
    {
        Debug.Log("Double clicked on: " + card.name);
        //Debug.Log("EventsManager: " + card.name + " was clicked on");
        OnDoubleClickedOnCard(card, pointerEventData);
    }

    
    public static event Action<Card, CardPile, bool> OnCardStackCheck = delegate { };
    public static void Fire_evt_OnCardStackCheck(Card card, CardPile pile, bool canStack)
    {
        //Debug.Log(" card " + card.name + " was dropped onto " + pile.gameObject.name + " stackable status -> " + canStack);
        OnCardStackCheck(card, pile, canStack);
    }
    
    public static event Action OnRequestDrawCards = delegate { };
    public static void Fire_evt_RequestDrawCards()
    {
        OnRequestDrawCards();
    }

    public static event Action OnRefillDeckRequested = delegate { };
    public static void Fire_evt_RefillDeckRequested()
    {
        OnRefillDeckRequested();
    }

    public static event Action<bool> OnReshuffleWithAd = delegate { };
    public static void Fire_evt_ShuffleWithAd(bool adAccepted)
    {
        OnReshuffleWithAd(adAccepted);
    }

    public static event Action<bool> OnDeckReshuffled = delegate { };
    public static void Fire_evt_OnDeckReshuffled(bool watchedAd)
    {
        OnDeckReshuffled(watchedAd);
    }

    public static event Action<ScreenOrientation> OnScreenOrientationChanged = delegate { };
    public static void Fire_evt_ScreenOrientationChanged(ScreenOrientation newOrientation)
    {
        OnScreenOrientationChanged(newOrientation);
    }

    public static event Action<Turn> OnUndoMovement = delegate { };
    public static void Fire_event_UndoMovement(Turn move)
    {
        //Debug.Log("Event player undo");
        OnUndoMovement(move);
    }

    public static event Action<Card> OnCardFlipped = delegate { };
    public static void Fire_event_OnCardFlipped(Card card)
    {
        //Debug.Log("fliped card");
        OnCardFlipped(card);
    }

    public static event Action<Turn> OnTurnSaved = delegate { };
    public static void Fire_evt_TurnSaved(Turn turn)
    {
        Debug.Log("Saved new turn " + turn.turnIndex);
        OnTurnSaved(turn);
    }

    public static event Action<float> OnSFXVolumeChanged = delegate { };
    public static void Fire_evt_SFXVolumeChanged(float vol)
    {
        OnSFXVolumeChanged(vol);
    }

    public static event Action<Card> OnCardDestroyed = delegate { };
    public static void Fire_evt_CardDestroyed(Card destroyedCard)
    {
        OnCardDestroyed(destroyedCard);
    }
}
