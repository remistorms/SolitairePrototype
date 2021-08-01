using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Card Properties")]
    public int m_value;
    public CardSuit m_cardSuit;
    public CardColor m_cardColor;

    [Header("Card Objects")]
    [SerializeField] private bool m_isFaceUp = true;
    [SerializeField] private GameObject m_frontSide;
    [SerializeField] private GameObject m_backSide;
    [SerializeField] private SpriteRenderer[] m_suitSprites;
    [SerializeField] private SpriteRenderer[] m_valueSprites;

    private bool m_isLerping = false;

    public void InitializeCard(int value, CardSuit suit)
    {
        m_value = value;

        m_cardSuit = suit;

        switch (m_cardSuit)
        {
            case CardSuit.Clubs:
                m_cardColor = CardColor.Black;
                break;

            case CardSuit.Diamonds:
                m_cardColor = CardColor.Red;
                break;

            case CardSuit.Hearts:
                m_cardColor = CardColor.Red;
                break;

            case CardSuit.Spades:
                m_cardColor = CardColor.Black;
                break;

            default:
                break;
        }
           
        //Set Suit Sprites
        Sprite suitSprite = CardHelper.Instance.GetSuitSprite(m_cardSuit);
        for (int i = 0; i < m_suitSprites.Length; i++)
        {
            m_suitSprites[i].sprite = suitSprite;
        }

        //Set Value Sprites
        Sprite valueSprite = CardHelper.Instance.GetValueSprite(value);
        Color spriteColor = CardHelper.Instance.GetColorFromSuit(m_cardSuit);

        for (int i = 0; i < m_valueSprites.Length; i++)
        {
            m_valueSprites[i].sprite = valueSprite;

            m_valueSprites[i].color = spriteColor;
        }
    }

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
