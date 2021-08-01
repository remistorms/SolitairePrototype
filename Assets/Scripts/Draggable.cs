using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
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
        m_returningPosition = transform.position;
        Debug.Log(this.gameObject.name + ": Begin Drag");
        m_canvasGroup.blocksRaycasts = false;
        m_card.SetOutlineColor(Color.cyan);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 eventPos = new Vector3( eventData.position.x, eventData.position.y, (-Camera.main.transform.position.z - 0.5f)  );
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(eventPos);
        transform.position = screenPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(this.gameObject.name + ": End Drag");
        m_canvasGroup.blocksRaycasts = true;
        transform.position = m_returningPosition;
        m_card.SetOutlineColor(Color.white);
    }

    public void SetReturningPosition(Vector3 position)
    {
        m_returningPosition = position;
    }
}
