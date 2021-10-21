using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : Singleton<StoreManager>
{
    [SerializeField] private List<StoreItemData> m_allStoreItemData;
    public Dictionary<int, StoreItemData> m_allStoreItemDataDictionary = new Dictionary<int, StoreItemData>();

    private void OnEnable()
    {
        EventsManager.OnStoreItemBought += OnStoreItemBought;
    }

    private void Start()
    {
    }

    private void OnStoreItemBought(StoreItemData item)
    {
        item.hasBeenPurchased = true;

        /*
        if (!m_purchasedItems.Contains(item))
        {
            m_purchasedItems.Add(item);
        }
        */
    }

    private void OnDisable()
    {
        EventsManager.OnStoreItemBought -= OnStoreItemBought;
    }
}
