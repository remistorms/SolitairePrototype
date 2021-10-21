using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StoreScreen : UIScreen
{
    [SerializeField] private TextMeshProUGUI m_coinsLabel;
    [SerializeField] private GameObject m_deckStoreItemPrefab;
    [SerializeField] private RectTransform m_deckItemsContainer;
    private List<StoreItem> m_allStoreItems = new List<StoreItem>();
    private List<StoreItemData> m_boughtItemData = new List<StoreItemData>();

    public override void Awake()
    {
        base.Awake();

    }

    private void OnEnable()
    {
        EventsManager.OnPlayerDataChanged += OnPlayerDataChanged;
    }

    private void OnPlayerDataChanged(PlayerData data)
    {
        m_coinsLabel.text = data.m_coins.ToString();
    }

    private void Start()
    {
        FillStoreItems();
    }

    protected override void OnScreenShown()
    {
        base.OnScreenShown();

        m_coinsLabel.text = DataManager.Instance.m_currentPlayerData.m_coins.ToString();
    }

    protected override void OnScreenHide()
    {
        base.OnScreenHide();

    }

    void FillStoreItems()
    {
        /*
        for (int i = 0; i < StoreManager.Instance.m_allStoreItemData.Count ; i++)
        {
            StoreItem storeItem = Instantiate(m_deckStoreItemPrefab, m_deckItemsContainer).GetComponent<StoreItem>();

            storeItem.InitializeStoreItem(StoreManager.Instance.m_allStoreItemData[i]);

            m_allStoreItems.Add(storeItem);
        }

        if (m_allStoreItems.Count > 0)
        {
            m_allStoreItems[0].EquipDeck();
        }
        */
    }

    private void OnDisable()
    {
        EventsManager.OnPlayerDataChanged -= OnPlayerDataChanged;
    }
}
