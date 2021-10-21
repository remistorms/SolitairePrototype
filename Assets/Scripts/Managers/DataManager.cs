using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public PlayerData m_currentPlayerData = null;
    [SerializeField] string path;

    public override void Awake()
    {
        base.Awake();

        m_currentPlayerData = new PlayerData();

        path = Application.persistentDataPath + "/solitaireSav.sol";

        LoadData();

    }

    private void OnEnable()
    {
        EventsManager.OnStoreItemBought += OnStoreItemBought;
    }

    private void OnStoreItemBought(StoreItemData storeItemData)
    {
        Debug.Log("DataManager: OnStoreImteBought <-" + storeItemData.itemName + " purchased: " + storeItemData.hasBeenPurchased);

        if (!m_currentPlayerData.purchasedItems.Contains(storeItemData))
        {
            storeItemData.hasBeenPurchased = true;

            m_currentPlayerData.m_coins -= storeItemData.storePrice;

            m_currentPlayerData.purchasedItems.Add(storeItemData);

            SaveData();

            EventsManager.Fire_evt_PlayerDataChanged(m_currentPlayerData);
        }
    }

    public bool CheckIfItemHasBeenPurchased(StoreItemData storeItemData)
    {
        if (m_currentPlayerData.purchasedItems.Contains(storeItemData))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LoadData()
    {
        SaveManager.Instance.Load<PlayerData>(path, OnDataLoaded, false);
    }

    private void OnDataLoaded(PlayerData playerData, SaveResult saveResults, string arg2)
    {
        switch (saveResults)
        {
            case SaveResult.Success:
                Debug.Log("Data Loaded Correctly, cheers");
                m_currentPlayerData = playerData;
                
                EventsManager.Fire_evt_PlayerDataLoade(m_currentPlayerData);
                break;

            case SaveResult.Error:
                Debug.Log("Data Could Not LOAD");
                break;

            case SaveResult.EmptyData:
                SaveData();
                break;
            default:
                break;
        }
    }

    public void SaveData()
    {
        SaveManager.Instance.Save<PlayerData>(m_currentPlayerData, path, OnDataSaved, false);
    }

    private void OnDataSaved(SaveResult saveResults, string arg1)
    {
        switch (saveResults)
        {
            case SaveResult.Success:
                Debug.Log("Save completed");
                EventsManager.Fire_evt_PlayerDataSaved(m_currentPlayerData);
                break;

            case SaveResult.Error:
                Debug.Log("Save ERROR");
                break;

            case SaveResult.EmptyData:
                Debug.Log("Save EMPTY....????");
                break;
            default:
                break;
        }
    }

    /*
    public void UpdatePlayerData()
    {
        EventsManager.Fire_evt_PlayerDataChanged(m_currentPlayerData);
    }
    */

    private void OnDisable()
    {
        EventsManager.OnStoreItemBought -= OnStoreItemBought;
    }

}

[System.Serializable]
public class StoreItemData
{
    public int itemId;
    public string itemName;
    public bool hasBeenPurchased;
    public int storePrice;
    public Sprite deckImage;
}

[System.Serializable]
public class PlayerData
{
    //Basic data
    public string m_playerName;
    public int m_coins;
    public List<StoreItemData> purchasedItems;
}
