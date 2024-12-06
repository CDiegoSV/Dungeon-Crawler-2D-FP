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

        [SerializeField] protected ControllerInputHandler _controllerInputHandler;

        [SerializeField] protected Vector2 _movementInputVector;
        [SerializeField] protected bool _isInteracting;
        [SerializeField] protected bool _isCarrying;

        #endregion

        #region LocalMethods

        public override void InitializeAgent()
        {
            base.InitializeAgent();
            transform.GetChild(1).gameObject.SetActive(true);
            _isCarrying = false;
            _isInteracting = false;
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
            //InitializeAgent();
            #endif
        }

        private void OnEnable()
        {
            InitializeAgent();
        }

        private void FixedUpdate()
        {
            if(!_fsm.GetGameReferee.IsTheGamePaused)
            {
                switch (_fsm.GetCurrentState)
                {
                    case States.ATTACKING:
                        break;

                    case States.DIE:
                        break;

                    default:
                        if (_movementInputVector.magnitude > 0)
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
                        break;
                }
            }
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.CompareTag("Orb"))
        //    {
        //        if(IsInteracting && !_isCarrying)
        //        {
        //            _isCarrying = true;
        //            other.gameObject.transform.SetParent(transform, false);
        //        }
        //    }
        //}

        #endregion

        #region PublicMethods

        public void ActivateHitBox()
        {
            _hitBox.ActivateHitBox();
        }

        public void OnMOVE(InputAction.CallbackContext value)
        {
            if (value.performed && !_fsm.GetGameReferee.IsTheGamePaused)
            {
                _movementInputVector = new Vector2( value.ReadValue<Vector2>().x, value.ReadValue<Vector2>().y);
            }
            else if (value.canceled)
            {
                _movementInputVector = Vector2.zero;
                _fsm.SetMovementDirection = _movementInputVector;
                _fsm.SetMovementSpeed = 0.0f;
                _fsm.StateMechanic(StateMechanics.STOP);
            }
        }

        public void OnATTACK(InputAction.CallbackContext value)
        {
            if(!_isCarrying)
            {
                if (value.performed && !_fsm.GetGameReferee.IsTheGamePaused)
                {
                    switch (_fsm.GetCurrentState)
                    {
                        case States.IDLE_UP:
                        case States.MOVING_UP:
                        case States.SPRINTING_UP:
                            SetHitboxColliderOffset(0.18f, 0.24f);
                            break;
                        case States.IDLE_LEFT:
                        case States.MOVING_LEFT:
                        case States.SPRINTING_LEFT:
                            SetHitboxColliderOffset(-0.18f, 0.24f);
                            break;
                        case States.IDLE_RIGHT:
                        case States.MOVING_RIGHT:
                        case States.SPRINTING_RIGHT:
                            SetHitboxColliderOffset(0.18f, -0.24f);
                            break;
                        case States.IDLE_DOWN:
                        case States.MOVING_DOWN:
                        case States.SPRINTING_DOWN:
                            SetHitboxColliderOffset(-0.18f, -0.24f);
                            break;
                    }
                    _fsm.StateMechanic(StateMechanics.ATTACK);
                }
            }
            
        }

        public void OnSPRINT(InputAction.CallbackContext value)
        {
            if (!_isCarrying)
            {
                if (value.performed && !_fsm.GetGameReferee.IsTheGamePaused)
                {
                    _isSprinting = true;
                    CalculateStateMechanicDirection();
                    _fsm.StateMechanic(_movementStateMechanic);
                }
                else if (value.canceled && !_fsm.GetGameReferee.IsTheGamePaused)
                {
                    _isSprinting = false;
                    if(_fsm.GetMovementDirection != Vector2.zero)
                    {
                        CalculateStateMechanicDirection();
                        _fsm.StateMechanic(_movementStateMechanic);
                    }
                    else
                    {
                        _fsm.StateMechanic(StateMechanics.STOP);
                    }
                }
            }
        }

        public void OnPAUSE(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                if (!_fsm.GetGameReferee.IsTheGamePaused)
                {
                    _fsm.GetGameReferee.SetPlayerWhoPausedTheGame = (int)playerIndex;
                    _fsm.GetGameReferee.GameStateMechanic(GameStates.PAUSE);
                }
                else
                {
                    _fsm.GetGameReferee.GameStateMechanic(GameStates.GAME);
                }
            }
        }

        public void OnINTERACT(InputAction.CallbackContext value)
        {
            if (value.performed && !_fsm.GetGameReferee.IsTheGamePaused)
            {
                _isInteracting = true;
            }
            else if (value.canceled)
            {
                _isInteracting = false;
            }
        }

        #endregion

        #region RuntimeMethods

        public void SetHitboxColliderOffset(float x, float y)
        {
            _hitBox.SetColliderOffset = new Vector2(x, y);
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

        public ControllerInputHandler MyControllerInputHandler
        {
            set { _controllerInputHandler = value; }
            get { return _controllerInputHandler; }
        }

        #endregion
    }
}