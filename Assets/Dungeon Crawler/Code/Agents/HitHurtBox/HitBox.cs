using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dante.DungeonCrawler
{
    [RequireComponent(typeof(Collider2D))]
    public class HitBox : MonoBehaviour
    {
        #region Knobs

        public int damage = 1;

        public float lifeTime = 1f;

        #endregion

        #region References

        [SerializeField] protected Collider2D _collider;

        #endregion

        #region Runtime Variables

        protected bool _isHitBoxActive;

        #endregion

        #region UnityMethods

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            if(_collider == null)
            {
                _collider = GetComponent<Collider2D>();
            }
            #endif
        }

        #endregion

        #region Public Methods

        public void ActivateHitBox()
        {
            if (!_isHitBoxActive)
            {
                StartCoroutine(LifeTime());
            }
        }

        #endregion

        #region Coroutines

        IEnumerator LifeTime()
        {
            _isHitBoxActive = true;
            _collider.enabled = true;

            yield return new WaitForSeconds(lifeTime);

            _collider.enabled = false;
            _isHitBoxActive = false;
        }

        #endregion

        #region Getters

        public int GetDamage
        {
            get { return damage; }
        }

        #endregion
    }
}
