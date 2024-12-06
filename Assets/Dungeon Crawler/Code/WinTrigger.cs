using Dante.DungeonCrawler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private DC_GameReferee _gameReferee;
    private bool triggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") &&  !triggered)
        {
            _gameReferee.GameStateMechanic(GameStates.VICTORY);
        }

    }
}
