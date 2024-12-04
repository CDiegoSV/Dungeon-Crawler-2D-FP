using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dante.DungeonCrawler
{

    [RequireComponent(typeof(Collider2D))]
    public class HurtBox : MonoBehaviour
    {
        #region Knobs

        [SerializeField] HurtBox_ScriptableObject _hurtBoxSO;

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
            _currentHealthPoints = _hurtBoxSO.hurtBoxValues.maxHealthPoints;
        }

        private void OnEnable()
        {
            _currentHealthPoints = _hurtBoxSO.hurtBoxValues.maxHealthPoints;
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
                        switch (_hurtBoxSO.agentType)
                        {
                            case AgentType.DestroyableObject:
                                if(other.gameObject.layer != LayerMask.NameToLayer("EnemyNPC"))
                                {

                                    if (_currentHealthPoints == 0)
                                    {
                                        _agent.StateMechanic(StateMechanics.DEATH);
                                        
                                    }
                                    else
                                    {
                                        StartCoroutine(DamageCoolDown(other.gameObject.GetComponent<HitBox>().GetDamage));
                                    }
                                }
                                break;
                            default:

                                if (_currentHealthPoints == 0)
                                {
                                    _agent.StateMechanic(StateMechanics.DEATH);
                                    
                                }
                                else
                                {
                                    StartCoroutine(DamageCoolDown(other.gameObject.GetComponent<HitBox>().GetDamage));
                                }
                                break;
                        }
                        //_currentHealthPoints--;
                        

                    }
                }
            }
        }

        #endregion

        #region Coroutines

        IEnumerator DamageCoolDown(int damage)
        {
            _isInCooldown = true;
            _currentHealthPoints -= damage;
            if(_agent as PlayersAvatar)
            {
                PlayersAvatar playersAvatar = _agent as PlayersAvatar;
                UIManager.Instance.HeartLoss((int)playersAvatar.playerIndex);
            }
            yield return new WaitForSeconds(_hurtBoxSO.hurtBoxValues.cooldownPerHit);
            _isInCooldown = false;
        }

        #endregion
    }
}
