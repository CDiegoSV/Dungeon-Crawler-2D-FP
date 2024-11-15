using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SotomaYorch.DungeonCrawler
{
    public class ControllerInputHandler : MonoBehaviour
    {
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
            foreach (PlayersAvatar avatar in _allAvatarsInScene)
            {
                if ((int)avatar.playerIndex == _playerInput.playerIndex)
                {
                    Debug.Log(":D");
                    _avatar = avatar;
                    _avatar.gameObject.SetActive(true);
                    this.transform.parent = avatar.transform;
                    this.transform.localPosition = Vector2.zero;
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

        #endregion
    }
}