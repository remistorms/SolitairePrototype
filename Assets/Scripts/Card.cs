using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private bool m_isFaceUp = false;

    [SerializeField] private GameObject m_frontSide;
    [SerializeField] private GameObject m_backSide;
    [SerializeField] private SpriteRenderer[] m_suitSprites;
    [SerializeField] private SpriteRenderer m_valueSprite;

    private bool m_isLerping = false;
    private float m_flipTime = 0.2f;

    public void Flip()
    {
        StartCoroutine(FlipRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Flip();
        }
    }

    IEnumerator FlipRoutine()
    {
        m_isLerping = true;
        float percent = 0f;
        float currentTime = 0f;
        bool hasSwitchedFaces = false;

        Quaternion initRotation = transform.localRotation;
        Quaternion desiredRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);

        while (percent < 1.0f)
        {
            transform.localRotation = Quaternion.Lerp(initRotation, desiredRotation, percent);

            percent = currentTime / m_flipTime;

            currentTime += Time.deltaTime;

            if (percent >= 0.5f && !hasSwitchedFaces)
            {
                m_frontSide.gameObject.SetActive(m_isFaceUp);
                m_backSide.gameObject.SetActive(!m_isFaceUp);
                hasSwitchedFaces = true;
            }

            yield return null;
        }

        transform.localRotation = desiredRotation;

        m_isFaceUp = !m_isFaceUp;

        m_isLerping = false;


    }

}
