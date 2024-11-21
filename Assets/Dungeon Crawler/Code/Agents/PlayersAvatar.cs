using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SotomaYorch.DungeonCrawler
{
    #region Enums

    public enum PlayerIndexes
    {
        //PlayerInput starts the first index (of player) with 0
        ONE = 0,
        TWO = 1,
        THREE = 2,
        FOUR = 3,
    }

    #endregion

    #region Structs


    #endregion

    public class PlayersAvatar : Agent
    {
        #region Knobs

        public PlayerIndexes playerIndex;

        #endregion

        #region References

        [SerializeField] protected HitBox _hitBox;

        #endregion

        #region RuntimeVariables

        protected Vector2 _movementInputVector;

        #endregion

        #region LocalMethods

        public override void InitializeAgent()
        {
            base.InitializeAgent();
            _movementInputVector = Vector2.zero;
            if(_hitBox == null)
            {
                _hitBox = transform.GetChild(0).GetComponent<HitBox>();
            }
        }

        #endregion

        #region UnityMethods

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            InitializeAgent();
            #endif
        }

        void Start()
        {
        }

        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _movementInputVector;
        }

        #endregion

        #region PublicMethods

        public void ActivateHitBox()
        {
            _hitBox.ActivateHitBox();
        }

        public void OnMOVE(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                _movementInputVector = value.ReadValue<Vector2>();
                _fsm.SetMovementDirection = _movementInputVector;
                if (_fsm.GetCurrentState != States.ATTACK)
                {
                    _fsm.SetMovementSpeed = 3.0f;
                }
                CalculateStateMechanicDirection();
                _fsm.StateMechanic(_movementStateMechanic);
            }
            else if (value.canceled)
            {
                _movementInputVector = Vector2.zero;
                _fsm.SetMovementDirection = Vector2.zero;
                _fsm.SetMovementSpeed = 0.0f;
                _fsm.StateMechanic(StateMechanics.STOP);
            }
        }

        public void OnATTACK(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                _fsm.StateMechanic(StateMechanics.ATTACK);
                _fsm.SetMovementSpeed = 0.0f;
            }
            else if (value.canceled)
            {
                
            }
        }

        public void OnSPRINT(InputAction.CallbackContext value)
        {
            if (value.performed)
            {

            }
            else if (value.canceled)
            {

            }
        }

        public void OnPAUSE(InputAction.CallbackContext value)
        {
            if (value.performed)
            {

            }
            else if (value.canceled)
            {

            }
        }

        public void OnINTERACT(InputAction.CallbackContext value)
        {
            if (value.performed)
            {

            }
            else if (value.canceled)
            {

            }
        }

        #endregion

        #region GettersSetters



        #endregion
    }
}