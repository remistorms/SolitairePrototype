using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private bool m_isFaceUp = true;

    [SerializeField] private GameObject m_frontSide;
    [SerializeField] private GameObject m_backSide;
    [SerializeField] private SpriteRenderer[] m_suitSprites;
    [SerializeField] private SpriteRenderer[] m_valueSprites;

    private bool m_isLerping = false;

    public void Flip(float flipTime = 0.15f )
    {
        if (m_isLerping)
            return;

        StartCoroutine(FlipRoutine(flipTime));
    }

    IEnumerator FlipRoutine(float flipTime = 0.15f)
    {
        if (flipTime <= 0.0f)
            flipTime = 0.001f;

        m_isLerping = true;
        float percent = 0f;
        float currentTime = 0f;
        bool hasSwitchedFaces = false;

        Quaternion initRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        Quaternion endRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);

        while (percent < 1.0f)
        {
            percent = currentTime / flipTime;

            currentTime += Time.deltaTime;

            transform.rotation = Quaternion.Lerp(initRotation, endRotation, percent);

            if (percent >= 0.5f && !hasSwitchedFaces)
            {
                m_isFaceUp = !m_isFaceUp;

                m_frontSide.gameObject.SetActive(m_isFaceUp);

                m_backSide.gameObject.SetActive(!m_isFaceUp);

                hasSwitchedFaces = true;
            }

            yield return null;
        }

        transform.rotation = endRotation;

        m_isLerping = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Flip(0.15f);
        }
    }

}
