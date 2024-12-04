using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dante.DungeonCrawler
{
    public class ControllerInputHandler : MonoBehaviour
    {
        #region References

        private DC_GameReferee gameReferee;

        #endregion

        #region LocalVariables

        protected PlayerInput _playerInput;

        protected PlayersAvatar[] _allAvatarsInScene;
        protected PlayersAvatar _avatar;

        #endregion

        #region UnityMethods

        void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            _allAvatarsInScene = GameObject.FindObjectsOfType<PlayersAvatar>(true);

            gameReferee = GameObject.FindAnyObjectByType<DC_GameReferee>();
            foreach (PlayersAvatar avatar in _allAvatarsInScene)
            {
                if ((int)avatar.playerIndex == _playerInput.playerIndex && !avatar.IsDead)
                {
                    Debug.Log(":D");
                    _avatar = avatar;
                    gameReferee.AddActivePlayersAvatar = avatar;
                    avatar.MyControllerInputHandler = this;
                    _avatar.gameObject.SetActive(true);
                    this.transform.parent = avatar.transform.parent;
                    this.transform.localPosition = Vector2.zero;
                    UIManager.Instance.StartPressed((int)avatar.playerIndex);
                }
            }
            gameObject.name = this.name + "_Player" + _playerInput.playerIndex;
        }

        #endregion

        #region CallbackContextMethods

        public void OnMove(InputAction.CallbackContext value)
        {
            _avatar?.OnMOVE(value);
        }

        public void OnAttack(InputAction.CallbackContext value)
        {
            _avatar?.OnATTACK(value);
        }

        public void OnSprint(InputAction.CallbackContext value)
        {
            _avatar?.OnSPRINT(value);
        }

        public void OnPause(InputAction.CallbackContext value)
        {
            _avatar?.OnPAUSE(value);
        }

        public void OnInteract(InputAction.CallbackContext value)
        {
            _avatar?.OnINTERACT(value);
        }

        #endregion


        #region Public Methods

        public void SwitchCurrentActionMapOfThePlayerInput(string mapName)
        {
            _playerInput.SwitchCurrentActionMap(mapName);
        }

        #endregion
    }
}