using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Card : MonoBehaviour, IDropHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Card Properties")]
    public bool m_isReady = false;
    public bool m_isFaceUp = true;
    public bool m_isTopCard = false;
    public int m_value;
    public CardValue m_cardValue;
    public CardSuit m_cardSuit;
    public CardColor m_cardColor;
    public CardPile m_cardPile = null;
    public CardPile m_previousPile = null;
    public List<Card> m_cardAndAllAbove = null;

    [Header("Card Objects")]

    [SerializeField] private GameObject m_frontSide;
    [SerializeField] private GameObject m_backSide;
    [SerializeField] private Image[] m_suitSprites;
    [SerializeField] private TextMeshProUGUI[] m_valueLabels;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_audioClip;

    [Header("Outline Colors")]
    [SerializeField] private Color m_normalColor;
    [SerializeField] private Color m_highlightColor;
    [SerializeField] private Color m_acceptColor;
    [SerializeField] private Color m_rejectColor;

    private Image m_outlineImage;
    private CanvasGroup m_canvasGroup;

   [SerializeField] private Image m_backImage;

    private void Awake()
    {
        m_backImage.sprite = CardHelper.Instance.GetCatPictureFromIndex( CardsManager.Instance.backCardImageIndex );

        m_outlineImage = GetComponent<Image>();
        SetOutlineColor(m_normalColor);
        m_canvasGroup = GetComponent<CanvasGroup>();
        //Audio stuff
        if (m_audioSource == null)
            m_audioSource = GetComponent<AudioSource>();

        m_audioSource.clip = m_audioClip;
        m_audioSource.playOnAwake = false;
    }

    private void Start()
    {
        EventsManager.OnSFXVolumeChanged += SetFlipSoundVolume;
    }

    private bool m_isLerping = false;

    public void InitializeCard(int value, CardSuit suit)
    {
        m_value = value;

        m_cardValue = CardHelper.Instance.GetCardValueFromInt(value);

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
        Color spriteColor = CardHelper.Instance.GetColorFromSuit(m_cardSuit);
        string cardValueText = CardHelper.Instance.GetCardValueString(m_cardValue);

        for (int i = 0; i < m_valueLabels.Length; i++)
        {
            m_valueLabels[i].text = cardValueText;

            m_valueLabels[i].color = spriteColor;
        }

        //this.gameObject.name = m_cardValue.ToString() + " of " + m_cardSuit.ToString();
    }

    /*
    public void FlipNoSignal(float flipTime = 0.15f)
    {
        if (m_isLerping)
            return;
        StartCoroutine(FlipRoutine(flipTime));
    }
    */
    
    
    public void Flip(float flipTime = 0.15f)
    {
        if (m_isLerping)
            return;
        //EventsManager.Fire_event_OnCardFlipped(this);
        StartCoroutine(FlipRoutine(flipTime));
    }
    

        /*
    public void FlipCardNoSignal(float flipTime = 0.15f)
    {
        if (m_isLerping)
            return;
        StartCoroutine(FlipRoutine(flipTime));
    }
    */

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

        PlayFlipSound();

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

    public void SetOutlineColor(Color color)
    {
        m_outlineImage.color = color;
    }

    public void SetFlipState(bool flipState)
    {
        if (flipState)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            m_frontSide.gameObject.SetActive(true);
            m_backSide.gameObject.SetActive(false);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            m_frontSide.gameObject.SetActive(false);
            m_backSide.gameObject.SetActive(true);
        }

        m_isFaceUp = flipState;
    }

    //Drag Interfaces
    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (!m_isFaceUp)
        {
            return;
        }

        m_cardAndAllAbove = m_cardPile.GetAllCardsAboveSelected(this);

        //Can only drag card from Game Piles, EndPiles and DrawPiles
        if (m_cardPile.m_pileType == PileType.GamePile || m_cardPile.m_pileType == PileType.EndPile)
        {
            EventsManager.Fire_evt_OnCardDragStarted(this, pointerEventData);
            m_canvasGroup.blocksRaycasts = false;
            //Added this to save previous pile
            m_previousPile = m_cardPile;
            m_cardPile = null;
        }
        else if (m_cardPile.m_pileType == PileType.DrawPile && m_isTopCard )
        {
            EventsManager.Fire_evt_OnCardDragStarted(this, pointerEventData);
            m_canvasGroup.blocksRaycasts = false;
            m_previousPile = m_cardPile;
            m_cardPile = null;
        }
        else
        {
            Debug.Log("Trying to drag a card from :" + m_cardPile.m_pileType + " which is not allowed");
            
        }

    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        EventsManager.Fire_evt_OnCardDragUpdate(this, pointerEventData);
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        EventsManager.Fire_evt_OnCardDragEnded(this, pointerEventData);
        m_canvasGroup.blocksRaycasts = true;
    }

    //DROP Interface
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Card dropped on top of another card !!");

        if (m_cardPile == null)
        {
            return;
        }

        if (eventData.pointerDrag.GetComponent<Card>() != null)
        {
            Debug.Log("Card dropped on top of another card !!");

            Card card = eventData.pointerDrag.GetComponent<Card>();

            m_cardPile.DropCardOnPile(card);

            EventsManager.Fire_evt_CardDroppedOnPile(this, m_cardPile);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!m_isReady)
        {
            return;
        }

        if (hasClickedOnce)
        {
            Debug.Log("Double Click");
            EventsManager.Fire_evt_OnDoubleClickedOnCard(this, eventData);
        }
        else
        {
            StartCoroutine(OnPointerClickRoutine());

            Debug.Log("Single Click");

            //Do nothing if card is not placed on pile
            if (m_cardPile == null)
            {
                return;
            }
            //Flip card if its top card of a game pile and is face down
            else if (m_cardPile.m_pileType == PileType.GamePile && !m_isFaceUp && m_isTopCard)
            {
                Flip();
                EventsManager.Fire_event_OnCardFlipped(this);
            }
            else if (m_cardPile.m_pileType == PileType.DeckPile && !m_isFaceUp && m_isTopCard)
            {
                //Debug.Log("Draw card here");
                EventsManager.Fire_evt_RequestDrawCards();
            }
        }
        
 
        //EventsManager.Fire_evt_OnClickedOnCard(this, eventData);
    }

    public void RemoveCardFromPile()
    {
        this.m_cardPile.RemoveCardFromPile(this);
    }

    public bool hasClickedOnce;

    IEnumerator OnPointerClickRoutine()
    {
        hasClickedOnce = true;
        float currentTime = 0f;
        float doubleClickTime = 0.2f;

        while (currentTime < doubleClickTime)
        {
            currentTime += Time.deltaTime;

            yield return null;
        }

        hasClickedOnce = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter != this)
        {
            SetOutlineColor(m_highlightColor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetOutlineColor(m_normalColor);
    }

    public void SetCanvasGroupState(bool state)
    {
        m_canvasGroup.blocksRaycasts = state;
    }

    public void PlayFlipSound()
    {
        m_audioSource.Play();
    }

    public void SetFlipSoundVolume(float volume)
    {
        m_audioSource.volume = volume;
    }

    private void OnDisable()
    {
        EventsManager.OnSFXVolumeChanged -= SetFlipSoundVolume;
    }
}
