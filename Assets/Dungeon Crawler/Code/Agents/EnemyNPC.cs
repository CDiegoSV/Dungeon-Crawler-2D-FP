using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dante.DungeonCrawler
{
    #region Enums

    public enum EnemyBehaviourState
    {
        PATROL,
        PERSECUTION
    }

    #endregion

    #region Structs


    #endregion

    public class EnemyNPC : Agent
    {
        #region Knobs

        public EnemyBehaviours_ScriptableObject scriptBehaviours;
        public bool isEnemyProjectile;

        #endregion

        #region References

        [SerializeField] protected GameObject enemyProjectile;

        #endregion

        #region RuntimeVariables

        protected EnemyBehaviourState _currentEnemyBehaviourState;
        [SerializeField] protected EnemyBehaviour _currentEnemyBehaviour;
        protected int _currentEnemyBehaviourIndex;
        protected Transform _avatarsTransform;
        protected StateMechanics _previousMovementStateMechanic;

        #endregion

        #region LocalMethods

        protected void InvokeStateMechanic()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    _fsm.StateMechanic(StateMechanics.STOP);
                    break;
                case EnemyBehaviourType.FIRE:
                    //_fsm.StateMechanic(StateMechanics.MOVE_RIGHT);
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    //TODO: Obtain the State Mechanic direction, 
                    //according to the direction of the enemy
                    _fsm.StateMechanic(_movementStateMechanic);
                    break;
                case EnemyBehaviourType.DEACTIVATE:
                    _fsm.StateMechanic(StateMechanics.DEATH);
                    break;
            }
        }

        protected virtual IEnumerator TimerForEnemyBehaviour()
        {
            yield return new WaitForSeconds(_currentEnemyBehaviour.time);
            FinalizeSubState();
            if (_currentEnemyBehaviourIndex < scriptBehaviours.patrolBehaviours.Length)
                GoToNextEnemyBehaviour();
        }

        protected virtual void GoToNextEnemyBehaviour()
        {
            _currentEnemyBehaviourIndex++;
            if (_currentEnemyBehaviourState == EnemyBehaviourState.PATROL)
            {
                if (_currentEnemyBehaviourIndex >= scriptBehaviours.patrolBehaviours.Length)
                    _currentEnemyBehaviourIndex = 0;
                _currentEnemyBehaviour = scriptBehaviours.patrolBehaviours[_currentEnemyBehaviourIndex];
            }
            else // PERSECUTING
            {
                if (_currentEnemyBehaviourIndex >= scriptBehaviours.persecutionBehaviours.Length)
                    _currentEnemyBehaviourIndex = 0;
                _currentEnemyBehaviour = scriptBehaviours.persecutionBehaviours[_currentEnemyBehaviourIndex];
            }
            InitializeSubState();
            CalculateStateMechanicDirection();
            InvokeStateMechanic();
            if (_currentEnemyBehaviour.time > 0)
            {
                //It is not a perpetual finite state,
                //so we will start the clock ;)
                StartCoroutine(TimerForEnemyBehaviour());
            }
        }

        protected void InitializeSubState()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    InitializeStopSubStateMachine();
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    InitializeMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    InitializePersecuteTheAvatarSubStateMachine();
                    break;
                case EnemyBehaviourType.FIRE:
                    InitializeFireSubStateMachine();
                    break;
                case EnemyBehaviourType.DEACTIVATE:
                    InitializeDeactivateSubStateMachine();
                    break;
            }
        }

        protected void FinalizeSubState()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    FinalizeStopSubStateMachine();
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    FinalizeMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    FinalizePersecuteTheAvatarSubStateMachine();
                    break;
                case EnemyBehaviourType.FIRE:
                    FinalizeFireSubStateMachine();
                    break;
                case EnemyBehaviourType.DEACTIVATE:
                    FinalizeDeactivateSubStateMachine();
                    break;
            }
        }

        protected void InitializePatrolBehaviour()
        {
            StopAllCoroutines();
            //To initialize the sub-finite state machine
            _currentEnemyBehaviourState = EnemyBehaviourState.PATROL;
            _currentEnemyBehaviourIndex = 0;

            if (scriptBehaviours.patrolBehaviours.Length > 0)
            {
                _currentEnemyBehaviour = scriptBehaviours.patrolBehaviours[0];
            }
            else
            {
                //Plan if the array is empty for this enemy NPC
                _currentEnemyBehaviour.type = EnemyBehaviourType.STOP;
                _currentEnemyBehaviour.time = -1; //-1 equals an infinity / perpetual state
            }
            //Initialize the proper sub-state
            InitializeSubState();
            CalculateStateMechanicDirection();
            InvokeStateMechanic();
            if (_currentEnemyBehaviour.time > 0 && gameObject.activeSelf)
            {
                //It is not a perpetual finite state,
                //so we will start the clock ;)
                StartCoroutine(TimerForEnemyBehaviour());
            }
        }

        protected void InitializePersecutionBehaviour()
        {
            StopAllCoroutines();
            _currentEnemyBehaviourState = EnemyBehaviourState.PERSECUTION;
            _currentEnemyBehaviourIndex = 0;

            if (scriptBehaviours.persecutionBehaviours.Length > 0)
            {
                _currentEnemyBehaviour = scriptBehaviours.persecutionBehaviours[0];
            }
            else
            {
                _currentEnemyBehaviour.type = EnemyBehaviourType.PERSECUTE_THE_AVATAR;
                _currentEnemyBehaviour.time = -1;
                _currentEnemyBehaviour.speed = 1.0f;
            }
            InitializeSubState();
            CalculateStateMechanicDirection();
            InvokeStateMechanic();
            if (_currentEnemyBehaviour.time > 0)
            {
                StartCoroutine(TimerForEnemyBehaviour());
            }
        }

        #endregion

        #region UnityMethods

        void Start()
        {
            InitializeAgent();
        }

        private void OnEnable()
        {
            _fsm.SetAllMovementSpeeds = _currentEnemyBehaviour.speed;
            if(isEnemyProjectile)
            {
                transform.localPosition = Vector3.zero;
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        void FixedUpdate()
        {
            switch (_currentEnemyBehaviour.type)
            {
                case EnemyBehaviourType.STOP:
                    ExecutingStopSubStateMachine();
                    break;
                case EnemyBehaviourType.MOVE_TO_RANDOM_DIRECTION:
                    ExecutingMoveToRandomDirectionSubStateMachine();
                    break;
                case EnemyBehaviourType.PERSECUTE_THE_AVATAR:
                    ExecutingPersecuteTheAvatarSubStateMachine();
                    break;
                case EnemyBehaviourType.FIRE:
                    ExecutingFireSubStateMachine();
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _avatarsTransform = other.gameObject.transform;
                InitializePersecutionBehaviour();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _avatarsTransform = null;
                InitializePatrolBehaviour();
            }
        }

        #endregion

        #region PublicMethods

        public override void InitializeAgent()
        {
            InitializePatrolBehaviour();
        }

        #endregion

        #region GettersSetters

        #endregion

        #region SubStateMachineStates

        #region StopSubStateMachineMethods

        protected void InitializeStopSubStateMachine()
        {
            _fsm.SetAllMovementSpeeds = _currentEnemyBehaviour.speed;
            _fsm.SetMovementDirection = Vector2.zero;
        }

        protected void ExecutingStopSubStateMachine()
        {
            //do nothing
        }

        protected void FinalizeStopSubStateMachine()
        {
            //do nothing
        }

        #endregion StopSubStateMachineMethods

        #region MoveToRandomDirectionSubStateMachineMethods

        protected void InitializeMoveToRandomDirectionSubStateMachine()
        {
            do
            {
                _movementDirection = new Vector2(
                                    UnityEngine.Random.Range(-1.0f, 1.0f),
                                    UnityEngine.Random.Range(-1.0f, 1.0f)
                                ).normalized;
            } while (_movementDirection.magnitude == 0.0f);
            _fsm.SetMovementDirection = _movementDirection;
            
            _fsm.SetAllMovementSpeeds = _currentEnemyBehaviour.speed;
        }

        protected void ExecutingMoveToRandomDirectionSubStateMachine()
        {
            //Do nothing
        }

        protected void FinalizeMoveToRandomDirectionSubStateMachine()
        {
            _fsm.SetMovementDirection = Vector2.zero;
            _fsm.SetAllMovementSpeeds = _currentEnemyBehaviour.speed;
        }

        #endregion MoveToRandomDirectionSubStateMachineMethods

        #region PersecuteTheAvatarSubStateMachineMethods

        protected void InitializePersecuteTheAvatarSubStateMachine()
        {
            if(isEnemyProjectile)
            {
                float _lastShorterDistance = 100f;
                GameObject _lastCloserPlayer = null;
                GameObject[] allPlayersInScene = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < allPlayersInScene.Length; i++)
                {
                    if (allPlayersInScene[i].activeSelf && _lastShorterDistance > Vector2.Distance(transform.position, allPlayersInScene[i].gameObject.transform.position))
                    {
                        _lastShorterDistance = Vector2.Distance(transform.position, allPlayersInScene[i].gameObject.transform.position);
                        _lastCloserPlayer = allPlayersInScene[i];
                    }
                }
                _avatarsTransform = _lastCloserPlayer.transform;
                Debug.Log("InitializePersecute - From: " + gameObject.name + " to: " + _lastCloserPlayer.name);
            }
            _fsm.SetMovementDirection = (_avatarsTransform.position - transform.position).normalized;
            _fsm.SetAllMovementSpeeds = _currentEnemyBehaviour.speed;
            _previousMovementStateMechanic = _movementStateMechanic;
        }

        protected void ExecutingPersecuteTheAvatarSubStateMachine()
        {
            //as the avatar may move, we have to update the direction towards him / her
            if(_avatarsTransform != null)
            {
                _fsm.SetMovementDirection = (_avatarsTransform.position - transform.position).normalized;
                CalculateStateMechanicDirection();
            }

            
            if (_previousMovementStateMechanic != _movementStateMechanic)
            {
                InvokeStateMechanic();
                _previousMovementStateMechanic = _movementStateMechanic;
            }
        }

        protected void FinalizePersecuteTheAvatarSubStateMachine()
        {
            _fsm.SetAllMovementSpeeds = _currentEnemyBehaviour.speed;
            _fsm.SetMovementDirection = Vector2.zero;
        }

        #endregion PersecuteTheAvatarSubStateMachineMethods

        #region FireSubStateMachineMethods

        protected void InitializeFireSubStateMachine()
        {
            enemyProjectile.SetActive(true);
        }

        protected void ExecutingFireSubStateMachine()
        {

        }

        protected void FinalizeFireSubStateMachine()
        {

        }

        #endregion

        #region DeactivateSubStateMachineMethods

        protected void InitializeDeactivateSubStateMachine()
        {
            InvokeStateMechanic();
        }

        protected void ExecutingDeactivateSubStateMachine()
        {

        }

        protected void FinalizeDeactivateSubStateMachine()
        {
        }

        #endregion

        #endregion SubStateMachineStates

    }
}