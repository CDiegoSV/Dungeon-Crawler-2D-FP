using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject lifePanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;

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
        int tempCurrentActiveHearts = 0;

        Debug.Log("Num de Cora: " + playerCanvasObjects[playerIndex].playerHealth.transform.GetChild(1).gameObject.transform.childCount.ToString());

        for (int i = 0; i < playerCanvasObjects[playerIndex].playerHealth.transform.GetChild(1).gameObject.transform.childCount; i++)
        {
            if (playerCanvasObjects[playerIndex].playerHealth.transform.GetChild(1).transform.GetChild(i).GetComponent<Image>().color == Color.white)
            {
                tempCurrentActiveHearts++;
            }
        }
        playerCanvasObjects[playerIndex].playerHealth.transform.GetChild(1).transform.GetChild(tempCurrentActiveHearts -1 ).GetComponent<Image>().color = Color.black;
        Debug.Log("Active Hearts Index: " + tempCurrentActiveHearts.ToString());

    }

    public void HeartGain(int playerIndex)
    {
        for (int i = 0; i < playerCanvasObjects[playerIndex].playerHealth.transform.GetChild(1).gameObject.transform.childCount; i++)
        {
            int tempCurrentActiveHearts = 0;
            if (playerCanvasObjects[playerIndex].playerHealth.transform.GetChild(1).transform.GetChild(i).GetComponent<Image>().color == Color.white)
            {
                tempCurrentActiveHearts++;
            }
            if(playerCanvasObjects[playerIndex].playerHealth.transform.GetChild(1).transform.GetChild(i).GetComponent<Image>().color == Color.black)
            {
                playerCanvasObjects[playerIndex].playerHealth.transform.GetChild(1).transform.GetChild(i).GetComponent<Image>().color = Color.white;
                break;
            }
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


    public void MetaPanelActivation(bool victoryOfThePlayer)
    {
        lifePanel.SetActive(false);
        if(victoryOfThePlayer)
        {
            victoryPanel.SetActive(true);
        }
        else
        {
            defeatPanel.SetActive(true);
        }
    }
    #endregion
}
