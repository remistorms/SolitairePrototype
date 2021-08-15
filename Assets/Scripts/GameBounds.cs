using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBounds : MonoBehaviour
{
    [SerializeField] private GameObject[] m_gameBounds;
    [SerializeField] private GameObject[] m_referenceObjects;


    [SerializeField] private Bounds m_bounds;
    [SerializeField] private Camera m_cam;
    [SerializeField] private float m_distanceBetween;
    [SerializeField] private float m_refDistance;
    [SerializeField] private float m_fovSpeed = 1.0f;

    private void Start()
    {
        m_bounds = new Bounds();
        m_cam = Camera.main;

        for (int i = 0; i < m_gameBounds.Length; i++)
        {
            m_bounds.Encapsulate(m_gameBounds[i].transform.position);
        }
    }

    private void Update()
    {
        m_gameBounds[0].transform.position = m_cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, 5.5f));

        m_gameBounds[1].transform.position = m_cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, 5.5f));

        m_distanceBetween = Vector3.Distance(m_gameBounds[0].transform.position, m_gameBounds[1].transform.position);

        m_refDistance = Vector3.Distance(m_referenceObjects[0].transform.position, m_referenceObjects[1].transform.position);

        UpdateBasedOnDistance();
    }

    void UpdateBasedOnDistance()
    {
        float difference = m_distanceBetween - m_refDistance;

        difference = Mathf.Abs(difference);

        if (difference < 0.1f)
        {
            Debug.Log("MORE");
            m_cam.fieldOfView -= Time.deltaTime * m_fovSpeed;
        }
        else if (difference > 0.1f)
        {
            Debug.Log("LESS");
            m_cam.fieldOfView += Time.deltaTime * m_fovSpeed;
        }
        else if (m_distanceBetween == m_refDistance)
        {
            Debug.Log("Perfect");
        }
    }
}
