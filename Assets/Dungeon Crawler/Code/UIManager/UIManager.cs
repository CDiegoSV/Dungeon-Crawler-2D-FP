using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Structs

    [Serializable]
    public struct PlayerCanvasObjects
    {
        public GameObject playerPressStart;
        public GameObject playerHealth;
        public GameObject playerIsDead;
    }

    #endregion

    #region UIManager

    public static UIManager Instance;

    private void Awake()
    {
        if( Instance != null  && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    #region References
    [Header("Panel References")]

    [SerializeField] private GameObject pausePanel;

    [Header("Player Canvas References")]
    [SerializeField] private PlayerCanvasObjects[] playerCanvasObjects;

    #endregion

    #region Public Methods

    public void StartPressed(int playerIndex)
    {
        if (playerIndex < playerCanvasObjects.Length)
        {
            playerCanvasObjects[playerIndex].playerPressStart.SetActive(false);
            playerCanvasObjects[playerIndex].playerHealth.SetActive(true);
            playerCanvasObjects[playerIndex + 1].playerPressStart.SetActive(true);
        }
        else
        {
            playerCanvasObjects[playerIndex].playerPressStart.SetActive(false);
            playerCanvasObjects[playerIndex].playerHealth.SetActive(true);
        }
    }

    public void PlayerIsDead(int playerIndex)
    {
        playerCanvasObjects[playerIndex].playerHealth.SetActive(false);
        playerCanvasObjects[playerIndex].playerPressStart.SetActive(false);
        playerCanvasObjects[playerIndex].playerIsDead.SetActive(true);
    }


    public void HeartLoss(int playerIndex)
    {
        for (int i = 0; i < playerCanvasObjects[playerIndex].playerHealth.transform.childCount; i++)
        {
            int tempCurrentActiveHearts = 0;
            if (playerCanvasObjects[playerIndex].playerHealth.transform.GetChild(i).gameObject.activeSelf)
            {
                tempCurrentActiveHearts++;
            }
            playerCanvasObjects[playerIndex].playerHealth.transform.GetChild(tempCurrentActiveHearts).gameObject.SetActive(false);
        }
    }
    
    public void HeartGain(int playerIndex)
    {
        for (int i = 0; i < playerCanvasObjects[playerIndex].playerHealth.transform.childCount; i++)
        {
            int tempCurrentActiveHearts = 0;
            if (playerCanvasObjects[playerIndex].playerHealth.transform.GetChild(i).gameObject.activeSelf)
            {
                tempCurrentActiveHearts++;
            }
            playerCanvasObjects[playerIndex].playerHealth.transform.GetChild(tempCurrentActiveHearts).gameObject.SetActive(true);
        }
    }

    public void TogglePausePanel()
    {
        if (!pausePanel.activeSelf)
        {
            pausePanel.SetActive(true);
        }
        else
        {
            pausePanel.SetActive(false);
        }
    }
    #endregion
}
