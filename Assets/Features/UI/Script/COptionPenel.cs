using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class COptionPenel : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private GameObject _optionPanel;
    [SerializeField] private GameObject _soundPenel;
    [SerializeField] private GameObject _displayPenel;
    [SerializeField] private GameObject _keyPenel;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _displayButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _returnMain;
    [SerializeField] private Button _keyButton;

    [SerializeField] private string _inputKey = "ESC";
    #endregion

    #region 내부 변수 
    private bool _enabled = false;
    private CGameManager _gameManager;
    private CInputActionData _optionInput;
    private string _stringTitleScene = "TitleScene";
    #endregion

    private void Start()
    {
        BindOption();
        AddTitleOption();
        AddBattleMapOption();
         
        Resume();
    }

    private void AddTitleOption()
    { 
        if (_soundButton == null || _displayButton == null || _keyButton == null)
        {
            return;
        } 

        _soundButton.onClick.AddListener(ViewSoundOption);
        _displayButton.onClick.AddListener(ViewDisplayOption);
        _keyButton.onClick.AddListener(ViewKeyOption);
    }
    private void AddBattleMapOption()
    {
        if (_resumeButton == null || _returnMain == null || _exitButton == null)
        {
            return; 
        }
         
        _exitButton.onClick.AddListener(GameEnd);
        _resumeButton.onClick.AddListener(Resume);
        _returnMain.onClick.AddListener(ReturnMain); 
    }

    private void BindOption()
    {
        if (_optionInput == null)
        {
            CInputDispatcher dispatcher = CInputDispatcher.Instance;
            if (dispatcher == null || !dispatcher.InputActionMap.TryGetValue(_inputKey, out _optionInput))
            {
                return;
            }
        }

        _optionInput.OnPerformedAction -= OpenOption;
        if (isActiveAndEnabled)
        {
            _optionInput.OnPerformedAction += OpenOption;
        }
    }

    private void OnEnable()
    {
        if (_optionInput != null)
        {
            BindOption();
        }
        UpdatePauseRequest();
    }

    private void OnDisable()
    {
        ReleasePauseRequest();
    }

    private void OnDestroy()
    {
        ReleasePauseRequest();
    }

    private void ReleasePauseRequest()
    {
        if (_optionInput != null)
        {
            _optionInput.OnPerformedAction -= OpenOption;
        }
        if (_gameManager != null)
        {
            _gameManager.SetOptionPaused(false);
        }
    }

    public void UpdatePauseRequest()
    {
        _enabled = _optionPanel != null && _optionPanel.activeSelf;
        bool isOpen = isActiveAndEnabled &&
            ((_optionPanel != null && _optionPanel.activeInHierarchy) ||
             (_soundPenel != null && _soundPenel.activeInHierarchy) ||
             (_displayPenel != null && _displayPenel.activeInHierarchy) ||
             (_keyPenel != null && _keyPenel.activeInHierarchy));

        _gameManager = CGameManager.Instance;
        if (_gameManager != null)
        {
            _gameManager.SetOptionPaused(isOpen);
        }
    }

    public void OpenOption()
    {
        if ((_soundPenel != null && _soundPenel.activeSelf) ||
            (_displayPenel != null && _displayPenel.activeSelf) ||
            (_keyPenel != null && _keyPenel.activeSelf))
        {
            if (CGameManager.Instance != null && CGameManager.Instance.IsGameOver)
            {
                Resume();
            }
            else
            {
                ShowPanel(_optionPanel);
            }
            return;
        }

        if (_optionPanel != null)
        {
            _enabled = !_optionPanel.activeSelf;
            if (_enabled && CGameManager.Instance != null && CGameManager.Instance.IsGameOver)
            {
                return;
            }
            _optionPanel.SetActive(_enabled);
        }

        UpdatePauseRequest();
    }

    public void Resume()
    {
        if (_optionPanel != null)
        {
            _optionPanel.SetActive(false);
        }
        if (_soundPenel != null)
        {
            _soundPenel.SetActive(false);
        }
        if (_displayPenel != null)
        {
            _displayPenel.SetActive(false);
        }
        if (_keyPenel != null)
        {
            _keyPenel.SetActive(false);
        }
        UpdatePauseRequest();
    }

    private void ShowPanel(GameObject panel)
    {
        if (panel == null || (CGameManager.Instance != null && CGameManager.Instance.IsGameOver))
        {
            return;
        }

        if (_optionPanel != null)
        {
            _optionPanel.SetActive(_optionPanel == panel);
        }
        if (_soundPenel != null)
        {
            _soundPenel.SetActive(_soundPenel == panel);
        }
        if (_displayPenel != null)
        {
            _displayPenel.SetActive(_displayPenel == panel);
        }
        if (_keyPenel != null)
        {
            _keyPenel.SetActive(_keyPenel == panel);
        }
        UpdatePauseRequest();
    }

    public void GameEnd()
    {
        Application.Quit();
    }
     
    public void ViewSoundOption()
    {
        ShowPanel(_soundPenel);
    }

    public void ReturnMain()
    {
        SceneManager.LoadScene(_stringTitleScene);
    }

    public void ViewDisplayOption()
    {
        ShowPanel(_displayPenel);
    }

    public void ViewKeyOption()
    {
        ShowPanel(_keyPenel);
    }
}
