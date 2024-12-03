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

    #endregion
}
