using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GLOBAL : MonoBehaviour
{
    public static GLOBAL Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
}
