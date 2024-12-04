using Dante.DungeonCrawler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyNPC
{


    #region Unity Methods

    void Start()
    {
        InitializeAgent();
    }

    private void OnEnable()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        InitializeAgent();
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

    #region Local Methods

    protected override IEnumerator TimerForEnemyBehaviour()
    {
        Debug.Log(gameObject.name + " entró a la Corutina en el estado: " + _currentEnemyBehaviour.type.ToString());
        yield return new WaitForSeconds(_currentEnemyBehaviour.time);
        FinalizeSubState();
        if (_currentEnemyBehaviourIndex < scriptBehaviours.patrolBehaviours.Length)
            GoToNextEnemyBehaviour();
        Debug.Log(gameObject.name + " salió de la Corutina en el estado: " + _currentEnemyBehaviour.type.ToString());
    }

    protected override void GoToNextEnemyBehaviour()
    {
        _currentEnemyBehaviourIndex++;
        print(_currentEnemyBehaviourIndex.ToString());
        if (_currentEnemyBehaviourState == EnemyBehaviourState.PATROL)
        {
            print(_currentEnemyBehaviourIndex.ToString());
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

    #endregion
}
