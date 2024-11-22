using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dante.DungeonCrawler
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
        protected bool _isInteracting;
        protected bool _isCarrying;

        #endregion

        #region LocalMethods

        public override void InitializeAgent()
        {
            base.InitializeAgent();
            _isCarrying = false;
            _isInteracting = false;
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

            switch (_fsm.GetCurrentState)
            {
                case States.IDLE_UP:
                case States.IDLE_DOWN:
                case States.IDLE_LEFT:
                case States.IDLE_RIGHT:
                case States.MOVING_UP:
                case States.MOVING_DOWN:
                case States.MOVING_LEFT:
                case States.MOVING_RIGHT:
                case States.SPRINTING_UP:
                case States.SPRINTING_LEFT:
                case States.SPRINTING_RIGHT:
                case States.SPRINTING_DOWN:
                    if (_movementInputVector != Vector2.zero)
                    {
                        //Dot
                        //!=
                        //magnitude
                        if (_movementInputVector != _fsm.GetMovementDirection)
                        {
                            _fsm.SetMovementDirection = _movementInputVector;
                            CalculateStateMechanicDirection();
                            _fsm.StateMechanic(_movementStateMechanic);
                        }
                        
                    }
                    else
                    {
                        if (_fsm.GetMovementDirection != Vector2.zero)
                        {
                            _fsm.SetMovementDirection = Vector2.zero;
                            _fsm.SetMovementSpeed = 0.0f;
                            _fsm.StateMechanic(StateMechanics.STOP);
                        }
                    }
                    break;
                case States.ATTACKING:

                    break;
            }
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
                switch (_fsm.GetCurrentState)
                {
                    case States.IDLE_UP:
                    case States.IDLE_DOWN:
                    case States.IDLE_LEFT:
                    case States.IDLE_RIGHT:
                    case States.MOVING_UP:
                    case States.MOVING_DOWN:
                    case States.MOVING_LEFT:
                    case States.MOVING_RIGHT:
                    case States.SPRINTING_UP:
                    case States.SPRINTING_LEFT:
                    case States.SPRINTING_RIGHT:
                    case States.SPRINTING_DOWN:
                        if (_movementInputVector != Vector2.zero)
                        {
                            //Dot
                            //!=
                            //magnitude
                            if (_movementInputVector != _fsm.GetMovementDirection)
                            {
                                _fsm.SetMovementDirection = _movementInputVector;
                                CalculateStateMechanicDirection();
                                _fsm.StateMechanic(_movementStateMechanic);
                            }

                        }
                        else
                        {
                            if (_fsm.GetMovementDirection != Vector2.zero)
                            {
                                _fsm.SetMovementDirection = Vector2.zero;
                                _fsm.SetMovementSpeed = 0.0f;
                                _fsm.StateMechanic(StateMechanics.STOP);
                            }
                        }
                        break;
                    case States.ATTACKING:

                        break;
                }
            }
            else if (value.canceled)
            {
                _movementInputVector = Vector2.zero;
            }
        }

        public void OnATTACK(InputAction.CallbackContext value)
        {
            if(!_isCarrying)
            {
                if (value.performed)
                {
                    _fsm.StateMechanic(StateMechanics.ATTACK);
                }
            }
            
        }

        public void OnSPRINT(InputAction.CallbackContext value)
        {
            if (!_isCarrying)
            {
                if (value.performed)
                {
                    _isSprinting = true;
                    CalculateStateMechanicDirection();
                    _fsm.StateMechanic(_movementStateMechanic);
                }
                else if (value.canceled)
                {
                    _isSprinting = false;
                    CalculateStateMechanicDirection();
                    _fsm.StateMechanic(_movementStateMechanic);
                }
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
                _isInteracting = true;
            }
            else if (value.canceled)
            {
                _isInteracting = false;
            }
        }

        #endregion

        #region GettersSetters

        public bool IsInteracting { get { return _isInteracting; } }

        public bool SetIsCarrying { set { _isCarrying = value; } }

        public bool IsSprinting { get { return _isSprinting; } }

        public Vector2 GetMovementInputVector
        {
            get { return _movementInputVector; }
        }

        #endregion
    }
}