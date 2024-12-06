using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        yield return new WaitForSeconds(6);
        CameraManager.instance.ChangeCameraToPlayers();
        fakeLake.SetActive(false);
        GetComponent<InteractableButton>().enabled = false;
    }

    #endregion
}
