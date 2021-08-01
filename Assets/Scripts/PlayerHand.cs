using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public List<Card> m_cardsInHand = new List<Card>();
    //Raycast
    public bool isPressed = false;
    private Ray ray;
    private RaycastHit hit;

    public GameObject m_currentClickedObject = null;
    public GameObject m_previousClickedObject = null;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnPressed();
        }

        if (Input.GetMouseButton(0))
        {
            OnHeldDown();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnReleased();
        }
    }

    void OnPressed()
    {
        //Debug.Log("user begin touch or clicked down");
        isPressed = true;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            m_previousClickedObject = m_currentClickedObject;
            m_currentClickedObject = hit.collider.gameObject;
        }
    }

    void OnHeldDown()
    {
        //Debug.Log("user is touching or holding down mouse click");
    }

    void OnReleased()
    {
        //Debug.Log("user removed touch or released mouse click");
        isPressed = false;
    }

}
