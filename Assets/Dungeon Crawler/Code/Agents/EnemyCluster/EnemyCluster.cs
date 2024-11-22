using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCluster : MonoBehaviour
{
    #region Unity Methods


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf != true)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf != false)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    #endregion
}
