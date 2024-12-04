using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Dante.DungeonCrawler
{
    #region Enums

    public enum GameStates { NONE, GAME, PAUSE, LOSE, VICTORY}

    #endregion

    public class DC_GameReferee : MonoBehaviour
    {

        #region References

        [SerializeField] private ControllerInputHandler controllerInputHandler;

        [SerializeField] private List<PlayersAvatar> currentActivePlayers;

        #endregion

        #region Runtime Variables

        private GameStates _currentGameState;

        protected int _gamePausePlayerIndex;
        protected bool _isTheGamePaused;
        private bool _allActivePlayersAreDead;

        #endregion

        #region Unity Methods

        private void Start()
        {
            InitializeGameReferee();
        }

        private void FixedUpdate()
        {
            ExecutingGameState();
        }

        #endregion

        #region Runtime Methods

        private void InitializeGameReferee()
        {
            _currentGameState = GameStates.GAME;
            _allActivePlayersAreDead = false;
            InitializeGameState();
        }

        private void InitializeGameState()
        {
            switch(_currentGameState)
            {
                case GameStates.GAME:
                    InitializeGAMEStateMechanic();
                    break;
                case GameStates.PAUSE:
                    InitializePAUSEDStateMechanic();
                    break;
                case GameStates.VICTORY:
                    InitializeVICTORYStateMechanic();
                    break;
                case GameStates.LOSE:
                    InitializeLOSEStateMechanic();
                    break;
            }
        }

        private void ExecutingGameState()
        {
            Debug.Log("Current Game State: " +  _currentGameState.ToString());
            switch (_currentGameState)
            {
                case GameStates.GAME:
                    ExecutingGAMEStateMechanic();
                    break;
                case GameStates.PAUSE:
                    ExecutingPAUSEDStateMechanic();
                    break;
                case GameStates.VICTORY:
                    ExecutingVICTORYStateMechanic();
                    break;
                case GameStates.LOSE:
                    ExecutingLOSEStateMechanic();
                    break;
            }
        }

        private void FinalizeGameState()
        {
            switch (_currentGameState)
            {
                case GameStates.GAME:
                    FinalizeGAMEStateMechanic();
                    break;
                case GameStates.PAUSE:
                    FinalizePAUSEDStateMechanic();
                    break;
                case GameStates.VICTORY:
                    FinalizeVICTORYStateMechanic();
                    break;
                case GameStates.LOSE:
                    FinalizeLOSEStateMechanic();
                    break;
            }
        }

        private void FinalizePreviousStateAndInitializeNewState(GameStates gameState)
        {
            FinalizeGameState();
            _currentGameState = gameState;
            InitializeGameState();
        }

        #endregion

        #region Public Methods

        public void GameStateMechanic(GameStates state)
        {
            switch(state)
            {
                case GameStates.GAME:
                    if (_currentGameState == GameStates.PAUSE)
                    {
                        FinalizePreviousStateAndInitializeNewState(state);
                    }
                    break;
                case GameStates.PAUSE:
                    if (_currentGameState == GameStates.GAME)
                    {
                        FinalizePreviousStateAndInitializeNewState(state);
                    }
                    break;
                case GameStates.VICTORY:
                    if (_currentGameState == GameStates.GAME)
                    {
                        FinalizePreviousStateAndInitializeNewState(state);
                    }
                    break;
                case GameStates.LOSE:
                    if (_currentGameState == GameStates.GAME)
                    {
                        FinalizePreviousStateAndInitializeNewState(state);
                    }
                    break;
            }
        }

        public void CheckActivePlayers()
        {
            _allActivePlayersAreDead = true;
            foreach(PlayersAvatar avatar in currentActivePlayers)
            {
                if (!avatar.IsDead)
                {
                    _allActivePlayersAreDead = false;
                    break;
                }
            }
            if (_allActivePlayersAreDead)
            {
                GameStateMechanic(GameStates.LOSE);
            }
        }

        #endregion

        #region GameStateStateMechanics

        #region GAME

        private void InitializeGAMEStateMechanic()
        {

        }

        private void ExecutingGAMEStateMechanic()
        {

        }


        private void FinalizeGAMEStateMechanic()
        {

        }

        #endregion

        #region PAUSED

        private void InitializePAUSEDStateMechanic()
        {
            UIManager.Instance.TogglePausePanel();
            currentActivePlayers[_gamePausePlayerIndex].MyControllerInputHandler.SwitchCurrentActionMapOfThePlayerInput("UI");
            _isTheGamePaused = true;
            Time.timeScale = 0;
        }

        private void ExecutingPAUSEDStateMechanic()
        {

        }


        private void FinalizePAUSEDStateMechanic()
        {
            UIManager.Instance.TogglePausePanel();
            currentActivePlayers[_gamePausePlayerIndex].MyControllerInputHandler.SwitchCurrentActionMapOfThePlayerInput("Gameplay");
            _isTheGamePaused = false;
            Time.timeScale = 1;
        }

        #endregion

        #region LOSE

        private void InitializeLOSEStateMechanic()
        {

        }

        private void ExecutingLOSEStateMechanic()
        {

        }


        private void FinalizeLOSEStateMechanic()
        {

        }

        #endregion

        #region VICTORY

        private void InitializeVICTORYStateMechanic()
        {

        }

        private void ExecutingVICTORYStateMechanic()
        {

        }


        private void FinalizeVICTORYStateMechanic()
        {

        }

        #endregion

        #endregion

        #region GettersSetters

        public int SetPlayerWhoPausedTheGame
        {
            set { _gamePausePlayerIndex = value; }
        }

        public PlayersAvatar AddActivePlayersAvatar
        {
            set { currentActivePlayers.Add(value); }
        }

        public bool IsTheGamePaused
        {
            get { return _isTheGamePaused; }
        }

        #endregion
    }

}
