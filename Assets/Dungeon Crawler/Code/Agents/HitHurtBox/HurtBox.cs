using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SotomaYorch.DungeonCrawler
{

    [RequireComponent(typeof(Collider2D))]
    public class HurtBox : MonoBehaviour
    {
        #region Knobs

        //TODO: Make an SO.
        public int maxHealthPoints = 3;
        public float coolDownTime = 1f;

        #endregion

        #region References

        [SerializeField] protected Agent _agent;

        #endregion

        #region Runtime Variabes

        protected bool _isInCooldown;

        [SerializeField] protected int _currentHealthPoints;

        #endregion

        #region Unity Methods

        private void Start()
        {
            _currentHealthPoints = maxHealthPoints;
        }

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            if(_agent == null)
            {
                _agent = transform.parent.gameObject.GetComponent<Agent>();

            }
            #endif
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!_isInCooldown)
            {
                if (other.gameObject.CompareTag("HitBox"))
                {
                    if(other.gameObject.layer != gameObject.layer)
                    {
                        //_currentHealthPoints--;
                        _currentHealthPoints -= other.gameObject.GetComponent<HitBox>().GetDamage;

                        if (_currentHealthPoints <= 0)
                        {
                            _agent.StateMechanic(StateMechanics.DIE);
                            //TODO: Complete the admin of this state
                            //animator, initialize, exe, finalize
                        }
                        else
                        {
                            StartCoroutine(CoolDown());
                        }

                    }
                }
            }
        }

        #endregion

        #region Coroutines

        IEnumerator CoolDown()
        {
            _isInCooldown = true;
            yield return new WaitForSeconds(coolDownTime);
            _isInCooldown = false;
        }

        #endregion
    }
}
