using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    private CanvasGroup m_canvasGroup;
    private Vector3 m_returningPosition;
    private Transform m_lastParent;
    private Card m_card;

    private void Awake()
    {
        m_card = GetComponent<Card>();
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_lastParent = this.transform.parent;
        m_returningPosition = this.transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //m_returningPosition = transform.position;
        m_canvasGroup.blocksRaycasts = false;
        //m_card.SetOutlineColor(Color.cyan);
        EventsManager.Fire_evt_OnCardBeginDrag(m_card);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 eventPos = new Vector3( eventData.position.x, eventData.position.y, (-Camera.main.transform.position.z - 0.5f)  );
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(eventPos);
        transform.position = screenPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log(this.gameObject.name + ": End Drag");
        m_canvasGroup.blocksRaycasts = true;
        //transform.position = m_returningPosition;
        //m_card.SetOutlineColor(Color.white);
        EventsManager.Fire_evt_OnCardEndedDrag(m_card);
    }

    /*
    public void SetReturningPosition(Vector3 position)
    {
        m_returningPosition = position;
    }
    */

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_card.m_isTopCard && !m_card.m_isFaceUp)
        {
            m_card.Flip();
        }
    }
}
