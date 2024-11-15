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

        #endregion

        #region RuntimeVariables

        protected Vector2 _movementInputVector;

        #endregion

        #region LocalMethods

        public override void InitializeAgent()
        {
            base.InitializeAgent();
            _movementInputVector = Vector2.zero;
        }

        #endregion

        #region UnityMethods

        void Start()
        {
            InitializeAgent();
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

        public void OnMOVE(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                _movementInputVector = value.ReadValue<Vector2>();
                _fsm.SetMovementDirection = _movementInputVector;
                _fsm.SetMovementSpeed = 3.0f;
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

        #endregion

        #region GettersSetters

        #endregion
    }
}