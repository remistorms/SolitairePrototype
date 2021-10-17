using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsScreen : UIScreen
{
    private DataManager dataManager;

    [SerializeField] TMP_InputField m_nameInputField;
    [SerializeField] TMP_InputField m_coinsInputField;
    [SerializeField] Button m_loadButton;
    [SerializeField] Button m_saveButton;

    public override void Awake()
    {
        base.Awake();

        m_loadButton.onClick.AddListener(LoadTest);
        m_saveButton.onClick.AddListener(SaveTest);
    }

    private void Start()
    {
        dataManager = FindObjectOfType<DataManager>();

        LoadTest();
    }

    public void LoadTest()
    {
        m_nameInputField.text = dataManager.m_currentPlayerData.m_playerName;
        m_coinsInputField.text = dataManager.m_currentPlayerData.m_coins.ToString();
    }

    public void SaveTest()
    {
        dataManager.m_currentPlayerData.m_playerName = m_nameInputField.text;

        dataManager.m_currentPlayerData.m_coins = int.Parse(m_coinsInputField.text);

        FindObjectOfType<DataManager>().SaveData();
    }

}
