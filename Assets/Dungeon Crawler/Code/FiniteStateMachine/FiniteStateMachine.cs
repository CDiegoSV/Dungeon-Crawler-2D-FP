using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dante.DungeonCrawler
{
    #region Enums

    public enum States
    {
        //IDLE
        IDLE_DOWN,
        IDLE_UP,
        IDLE_RIGHT,
        IDLE_LEFT,
        //MOVING
        MOVING_DOWN,
        MOVING_UP,
        MOVING_RIGHT,
        MOVING_LEFT,

        SPRINTING_UP,
        SPRINTING_DOWN,
        SPRINTING_LEFT,
        SPRINTING_RIGHT,

        ATTACKING,

        DIE //TODO: Complete code to admin new states.
    }

    public enum StateMechanics
    {
        //STOP
        STOP,
        //MOVE
        MOVE_UP,
        MOVE_DOWN,
        MOVE_LEFT,
        MOVE_RIGHT,

        SPRINT_UP,
        SPRINT_DOWN,
        SPRINT_LEFT,
        SPRINT_RIGHT,

        //ATTACK
        ATTACK,

        DEATH
    }

    #endregion

    #region Structs


    #endregion

    public class FiniteStateMachine : MonoBehaviour
    {
        #region Knobs

        [SerializeField] protected float walkSpeed;
        [SerializeField] protected float sprintSpeed;

        #endregion

        #region References

        [SerializeField, HideInInspector] protected Animator _animator;
        [SerializeField,HideInInspector] protected Rigidbody2D _rigidbody;

        [SerializeField] protected AnimationClip _deathClip;

        #endregion

        #region RuntimeVariables

        [SerializeField] protected States _state;
        [SerializeField] protected Vector2 _movementDirection;
        [SerializeField] protected float _currentMovementSpeed;

        [SerializeField] protected Agent _agent;

        #endregion

        #region LocalMethods

        protected void InitializeFiniteStateMachine()
        {
            
        }

        protected void CleanAnimatorFlags()
        {
            foreach (StateMechanics stateMechanic in Enum.GetValues(typeof(StateMechanics)))
            {
                //print( "CleanAnimatorFlags() from: " + gameObject.name + ": animator string: " + stateMechanic.ToString());
                _animator.SetBool(stateMechanic.ToString(), false);
            }
        }

        public void InitializeState()
        {
            switch (_state)
            {
                case States.DIE:
                    InitializeDeathState();
                    //gameObject.SetActive(false); //PROTOTYPE TO DELETE
                    break;
                case States.IDLE_UP:
                case States.IDLE_DOWN:
                case States.IDLE_LEFT:
                case States.IDLE_RIGHT:
                    InitializeIdleState();
                    break;
                case States.MOVING_UP:
                case States.MOVING_DOWN:
                case States.MOVING_LEFT:
                case States.MOVING_RIGHT:
                    InitializeMovingState();
                    break;

                case States.ATTACKING:
                    if(_agent as PlayersAvatar)
                    {
                        InitializeAttackingState();
                    }
                    break;
                case States.SPRINTING_UP:
                case States.SPRINTING_LEFT:
                case States.SPRINTING_RIGHT:
                case States.SPRINTING_DOWN:
                    InitializeSprintingState();
                    break;
            }
        }

        public void ExecutingState()
        {
            switch (_state)
            {
                case States.IDLE_UP:
                case States.IDLE_DOWN:
                case States.IDLE_LEFT:
                case States.IDLE_RIGHT:
                    ExecutingIdleState();
                    break;
                case States.MOVING_UP:
                case States.MOVING_DOWN:
                case States.MOVING_LEFT:
                case States.MOVING_RIGHT:
                    ExecutingMovingState();
                    break;
                case States.ATTACKING:
                    if (_agent as PlayersAvatar)
                    {
                        ExecutingAttackingState();
                    }
                    break;
                case States.SPRINTING_UP:
                case States.SPRINTING_LEFT:
                case States.SPRINTING_RIGHT:
                case States.SPRINTING_DOWN:
                    ExecutingSprintingState();
                    break;

                case States.DIE:
                    ExecutingDeathState();
                    //gameObject.SetActive(false); //PROTOTYPE TO DELETE
                    break;
            }
        }

        public void FinalizeState()
        {
            switch (_state)
            {
                case States.IDLE_UP:
                case States.IDLE_DOWN:
                case States.IDLE_LEFT:
                case States.IDLE_RIGHT:
                    FinalizeIdleState();
                    break;
                case States.MOVING_UP:
                case States.MOVING_DOWN:
                case States.MOVING_LEFT:
                case States.MOVING_RIGHT:
                    FinalizeMovingState();
                    break;
                case States.ATTACKING:
                    if (_agent as PlayersAvatar)
                    {
                        FinalizeAttackingState();
                    }
                    break;
                case States.SPRINTING_UP:
                case States.SPRINTING_LEFT:
                case States.SPRINTING_RIGHT:
                case States.SPRINTING_DOWN:
                    FinalizeSprintingState();
                    break;

                case States.DIE:
                    FinalizeDeathState();
                    //gameObject.SetActive(false); //PROTOTYPE TO DELETE
                    break;
            }
        }


        #region FiniteStateMachineStates

        #region IdleState
        protected virtual void InitializeIdleState()
        {
            _currentMovementSpeed = 0.0f;
        }

        protected virtual void ExecutingIdleState()
        {

        }
        protected virtual void FinalizeIdleState()
        {

        }
        #endregion IdleState

        #region MovingState
        protected virtual void InitializeMovingState()
        {
            _currentMovementSpeed = walkSpeed;
        }

        protected virtual void ExecutingMovingState()
        {

        }
        protected virtual void FinalizeMovingState()
        {

        }
        #endregion MovingState

        #region AttackingState
        protected virtual void InitializeAttackingState()
        {
            ((PlayersAvatar)_agent).ActivateHitBox();
            SetMovementSpeed = 0.0f;
        }

        protected virtual void ExecutingAttackingState()
        {

        }
        protected virtual void FinalizeAttackingState()
        {
            //if (_agent as PlayersAvatar)
            //{
            //    PlayersAvatar tempPlayerAvatar = (PlayersAvatar)_agent;
            //    if (tempPlayerAvatar.GetMovementInputVector != Vector2.zero && !tempPlayerAvatar.IsSprinting)
            //    {
            //        print("Attack movement check");
            //        StateMechanic(_agent.GetMovementStateMechanic);
            //    }
            //    else if (tempPlayerAvatar.GetMovementInputVector != Vector2.zero && tempPlayerAvatar.IsSprinting)
            //    {
            //        print("Attack movement check");
            //        StateMechanic(_agent.GetMovementStateMechanic);

            //    }
            //    else if (tempPlayerAvatar.GetMovementInputVector == Vector2.zero)
            //    {
            //        print("Attack movement check");
            //        StateMechanic(StateMechanics.STOP);
            //    }
            //}
        }
        #endregion AttackingState

        #region SprintingState
        protected virtual void InitializeSprintingState()
        {
            _currentMovementSpeed = sprintSpeed;
        }

        protected virtual void ExecutingSprintingState()
        {

        }
        protected virtual void FinalizeSprintingState()
        {

        }
        #endregion SprintingState

        #region DeathState
        protected virtual void InitializeDeathState()
        {
            if(_agent as DestroyableObjects)
            {
                StartCoroutine(AgentDeathCoroutine());
            }
            else
            {
                StartCoroutine(AgentDeathCoroutine());
            }
        }

        protected virtual void ExecutingDeathState()
        {

        }
        protected virtual void FinalizeDeathState()
        {

        }
        #endregion InteractingState

        #endregion FiniteStateMachineStates

        #endregion

        #region UnityMethods

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            if(_agent == null)
            {
                _agent = gameObject.GetComponent<Agent>();
            }
            #endif
        }

        

        private void Start()
        {
            InitializeFiniteStateMachine();
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _movementDirection * _currentMovementSpeed;
        }

        #endregion

        #region PublicMethods

        //Action
        public void StateMechanic(StateMechanics value)
        {

            FinalizeState();
            CleanAnimatorFlags();
            _animator.SetBool(value.ToString(), true);
        }

        public void SetState(States value)
        {
            CleanAnimatorFlags();
            _state = value;
            InitializeState();
        }

        #endregion

        #region Coroutines

        private IEnumerator AgentDeathCoroutine()
        {
            Debug.Log(gameObject.name + " is dying");
            _agent.IsDead = true;
            _agent.transform.GetChild(1).gameObject.SetActive(false);
            yield return new WaitForSeconds(_deathClip.length);
            _agent.gameObject.SetActive(false);
            if(_agent as EnemyNPC)
            {
                EnemyNPC tempEnemy = _agent as EnemyNPC;
                if(!tempEnemy.isEnemyProjectile)
                {
                    _agent.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
            else if(_agent as PlayersAvatar)
            {
                _agent.transform.GetChild(1).gameObject.SetActive(true);
                PlayersAvatar avatar = _agent as PlayersAvatar;
                UIManager.Instance.PlayerIsDead((int)avatar.playerIndex);
            }
        }

        #endregion

        #region GettersSetters

        public Vector2 GetMovementDirection
        {
            get { return _movementDirection; }
        }

        public Vector2 SetMovementDirection
        {
            set { _movementDirection = value; }
        }

        public float SetMovementSpeed
        {
            set { _currentMovementSpeed = value; }
        }

        public float SetAllMovementSpeeds
        {
            set { _currentMovementSpeed = value; walkSpeed = value; sprintSpeed = value; }
        }
        public States GetCurrentState
        {
            get { return _state;}
        }

        #endregion
    }
}