using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StoreItem : MonoBehaviour
{
    public StoreItemData m_storeItemData;
    public DeckStoreButtonState m_buttonState = DeckStoreButtonState.Normal;
    [SerializeField] Button m_buyButton;
    [SerializeField] Image m_image;
    [SerializeField] GameObject m_priceContainer;
    [SerializeField] TextMeshProUGUI m_priceLabel;
    [SerializeField] TextMeshProUGUI m_buttonStateLabel;
    public int deckPrice;
    public int deckBackgroundIndex;
    public bool isEquiped = false;
    public bool hasBeenPurchased = false;
    public GameObject m_equipedTag;

    private void Awake()
    {
        if (m_storeItemData.hasBeenPurchased)
        {
            m_buyButton.onClick.AddListener(EquipDeck);
            DisablePriceTag();
        }
        else
        {
            m_buyButton.onClick.AddListener(TryPurchaseDeck);
        }
    }

    private void OnEnable()
    {
        EventsManager.OnDeckEquiped += OnDeckEquiped;
    }

    private void OnDeckEquiped(StoreItemData storeItemData)
    {
        if (m_storeItemData.Equals(storeItemData))
        {
            m_equipedTag.SetActive(true);
            isEquiped = true;
        }
        else
        {
            m_equipedTag.SetActive(false);
            isEquiped = false;
        }

        if (isEquiped)
        {
            m_buttonStateLabel.text = "Equiped";
        }
        else
        {
            if (m_storeItemData.hasBeenPurchased)
            {
                m_buttonStateLabel.text = "Equip";
            }
            else
            {
                m_buttonStateLabel.text = "Buy";
            }
         
        }
    }

    public void InitializeStoreItem(StoreItemData storeItemData)
    {
        Debug.Log("Initializing Store Item with " + storeItemData.itemName +  storeItemData.hasBeenPurchased);

        m_storeItemData = storeItemData;

        hasBeenPurchased = storeItemData.hasBeenPurchased;

        m_buyButton.onClick.RemoveAllListeners();

        deckBackgroundIndex = storeItemData.itemId;

        m_image.sprite = storeItemData.deckImage;

        deckPrice = storeItemData.storePrice;

        m_priceLabel.text = storeItemData.storePrice.ToString();

        if (hasBeenPurchased)
        {
            m_buyButton.onClick.AddListener(EquipDeck);
            DisablePriceTag();
        }
        else
        {
            m_buyButton.onClick.AddListener(TryPurchaseDeck);
        }
    }

    private void Update()
    {
        if (hasBeenPurchased)
        {
            DisablePriceTag();
        }
    }

    public void EquipDeck()
    {
        Debug.Log("Set new deck");

        isEquiped = true;

        CardHelper.Instance.SetDeckImage(m_storeItemData);
    }

    public void TryPurchaseDeck()
    {
        Debug.Log("Player coins: " + DataManager.Instance.m_currentPlayerData.m_coins.ToString());
        Debug.Log("Deck Prize: " + deckPrice.ToString());

        if (DataManager.Instance.m_currentPlayerData.m_coins >= deckPrice)
        {
            m_storeItemData.hasBeenPurchased = true;
            //Player has enought coins
            m_priceContainer.SetActive(false);

            m_buyButton.onClick.RemoveAllListeners();

            m_buyButton.onClick.AddListener(EquipDeck);

            EventsManager.Fire_evt_StoreItemBought(m_storeItemData);
            //TODO save purchase somethere
            EquipDeck();
        }
        else
        {
            Debug.Log("Not enough coins :");
            //Not enough coins
        }
    }

    public void DisablePriceTag()
    {
        m_priceContainer.SetActive(false);
    }

    private void OnDisable()
    {
        EventsManager.OnDeckEquiped -= OnDeckEquiped;
    }
}

public enum DeckStoreButtonState
{
    Normal,
    Purchased
}
