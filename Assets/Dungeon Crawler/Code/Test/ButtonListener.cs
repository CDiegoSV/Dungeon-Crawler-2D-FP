using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Dante.DungeonCrawler;

public class ButtonListener : MonoBehaviour
{
    #region References
    protected Button _button;
    protected UnityAction _onClickAction;

    [Header("Scene Loader Asset References")]

    [SerializeField] private AnimationClip _transitionOutClip;
    [SerializeField] private Animator _transitionPanel;

    #endregion

    #region Knobs
    [Header("Button Presets")]

    [SerializeField] private bool selectOnEnable;

    [Header("Button Selector")]

    [SerializeField] private bool sceneLoaderButton;
    [SerializeField] private int sceneID;

    [SerializeField] private bool resumeButton;

    [SerializeField] private bool quitButton;


    #endregion

    #region Unity Methods
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(_onClickAction);
        InitializeButton();
        if (selectOnEnable)
        {
            _button.Select();
        }
    }

    private void OnEnable()
    {
        InitializeButton();
    }

    private void OnDisable()
    {
        _onClickAction -= SceneLoaderOnClickAction;
    }
    #endregion

    #region Runtime Methods
    private void InitializeButton()
    {
        
        if (sceneLoaderButton)
        {
            _onClickAction += SceneLoaderOnClickAction;
        }
        else if (quitButton)
        {
            _onClickAction += QuitOnClickAction;
        }
        else if(resumeButton)
        {
            _onClickAction += ResumeOnClickAction;
        }
    }

    private void FinalizeButton()
    {
        if (sceneLoaderButton)
        {
            _onClickAction -= SceneLoaderOnClickAction;
        }
        else if (quitButton)
        {
            _onClickAction -= QuitOnClickAction;
        }
    }

    IEnumerator SceneTransitionCoroutine()
    {
        _transitionPanel.Play("TransitionOut");

        yield return new WaitForSeconds(_transitionOutClip.length);

        SceneManager.LoadScene(sceneID);
    }

    #endregion

    #region OnClick Actions
    protected void SceneLoaderOnClickAction()
    {
        StartCoroutine(SceneTransitionCoroutine());
    }
    protected void QuitOnClickAction()
    {
        Application.Quit();
    }

    protected void ResumeOnClickAction()
    {
        DC_GameReferee gameReferee = FindAnyObjectByType<DC_GameReferee>();
        gameReferee.GameStateMechanic(GameStates.GAME);
    }
    #endregion

}
