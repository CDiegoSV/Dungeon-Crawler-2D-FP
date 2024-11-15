using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SotomaYorch.DungeonCrawler
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

        ATTACK,

        DEATH //TODO: Complete code to admin new states.
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
        //ATTACK
        ATTACK,

        DIE
    }

    #endregion

    #region Structs


    #endregion

    public class FiniteStateMachine : MonoBehaviour
    {
        #region Knobs


        #endregion

        #region References

        [SerializeField,HideInInspector] protected Animator _animator;
        [SerializeField,HideInInspector] protected Rigidbody2D _rigidbody;



        #endregion

        #region RuntimeVariables

        [SerializeField] protected States _state;
        [SerializeField] protected Vector2 _movementDirection;
        [SerializeField] protected float _movementSpeed;

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
                _animator.SetBool(stateMechanic.ToString(), false);
            }
        }

        protected void InitializeState()
        {
            switch(_state)
            {
                case States.DEATH:
                    //Death
                    break;
                case States.ATTACK:
                    if(_agent as PlayersAvatar)
                    {
                        ((PlayersAvatar)_agent).ActivateHitBox();
                    }
                    break;
            }
        }

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
            _rigidbody.velocity = _movementDirection * _movementSpeed;
        }

        #endregion

        #region PublicMethods

        //Action
        public void StateMechanic(StateMechanics value)
        {
            _animator.SetBool(value.ToString(), true);
        }

        public void SetState(States value)
        {
            CleanAnimatorFlags();
            _state = value;
            InitializeState();
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
            set { _movementSpeed = value; }
        }

        #endregion
    }
}