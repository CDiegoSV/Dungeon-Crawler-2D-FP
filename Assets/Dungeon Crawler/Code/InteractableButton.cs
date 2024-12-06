using Dante.DungeonCrawler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableButton : MonoBehaviour
{
    #region References

    [SerializeField] GameObject fakeLake;
    [SerializeField] Sprite pressedButton;

    #endregion

    #region Runtime Variables

    private bool wasActivated;


    #endregion

    #region Unity Methods

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !wasActivated)
        {
            wasActivated = true;
            
            StartCoroutine(LakeCoroutine());
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator LakeCoroutine()
    {
        GetComponent<SpriteRenderer>().sprite = pressedButton;
        fakeLake.GetComponent<Animator>().Play("LakeFadeOut");
        CameraManager.instance.ChangeCameraToLake();
        CameraManager.instance.ShakeCameraForSeconds(5f);
        foreach(Gamepad gamepad in Gamepad.all)
        {
            if(gamepad.enabled)
            {
                gamepad.SetMotorSpeeds(0.25f, 0.75f);
            }
        }
        yield return new WaitForSeconds(6);
        foreach (Gamepad gamepad in Gamepad.all)
        {
            if (gamepad.enabled)
            {
                gamepad.SetMotorSpeeds(0f, 0f);
            }
        }
        CameraManager.instance.ChangeCameraToPlayers();
        fakeLake.SetActive(false);
        GetComponent<InteractableButton>().enabled = false;
    }

    #endregion
}
