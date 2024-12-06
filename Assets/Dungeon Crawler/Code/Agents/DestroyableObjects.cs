using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dante.DungeonCrawler
{
    #region Enums


    #endregion

    #region Structs


    #endregion

    public class DestroyableObjects : Agent
    {
        #region Knobs

        [SerializeField] private bool vessel;

        #endregion

        #region References

        #endregion

        #region RuntimeVariables

        #endregion

        #region LocalMethods



        #endregion

        #region UnityMethods

        void Start()
        {
            //we can access the rigidbody via the
            //inheritance of the Agent
            //_rigidbody.velocity = Vector2.right;
        }

        void Update()
        {
            
        }

        #endregion

        #region PublicMethods

        public void ItemDrop()
        {
            if (!vessel)
            {
                float roll = Random.Range(1f, 10f);

                if (roll <= 2)
                {
                    transform.GetChild(2).gameObject.SetActive(true);
                    transform.GetChild(2).gameObject.transform.SetParent(null);
                }
            }
            else
            {
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.transform.SetParent(null);
            }
        }

        #endregion

        #region GettersSetters

        #endregion
    }
}