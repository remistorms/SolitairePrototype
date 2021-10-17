using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public PlayerData m_currentPlayerData = null;
    public string path;

    private void Awake()
    {
        m_currentPlayerData = new PlayerData();

        path = Application.persistentDataPath + "/solitaireSav.sol";

        LoadData();

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
}
