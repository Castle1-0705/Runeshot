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
    private string _stringTitleScene = "TitleScene";
    #endregion

    private void Start()
    {
        BindOption();
        AddTitleOption();
        AddBattleMapOption();
         
        _optionPanel.SetActive(_enabled);
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
        if (CInputDispatcher.Instance.InputActionMap.ContainsKey(_inputKey))
        {
            CInputDispatcher.Instance.InputActionMap[_inputKey].OnPerformedAction += OpenOption;
        }
    }

    private void OnDisable()
    {
        if (CInputDispatcher.Instance == null)
        {
            return;
        }

        if (CInputDispatcher.Instance.InputActionMap.ContainsKey(_inputKey))
        {
            CInputDispatcher.Instance.InputActionMap[_inputKey].OnPerformedAction -= OpenOption;
        }
    }
    public void OpenOption()
    {
        _enabled = _optionPanel.activeSelf;

        _enabled = !_enabled;
         
        if (_soundPenel.activeSelf)
        {
            _soundPenel.SetActive(false);
            Time.timeScale = 1f;
            return;
        }
        else if(_displayPenel.activeSelf)
        {
            _displayPenel.SetActive(false); 
            Time.timeScale = 1f;
            return;
        }
        else if(_keyPenel.activeSelf)
        {
            _keyPenel.SetActive(false);
            Time.timeScale = 1f;
            return;
        }

        Time.timeScale = _enabled ? 0 : 1f;

        _optionPanel.SetActive(_enabled);
    }

     
    public void Resume()
    {
        Time.timeScale = 1f;

        _enabled = false;

        _optionPanel.SetActive(_enabled);
    }
    public void GameEnd()
    {
        Application.Quit();
    }
     
    public void ViewSoundOption()
    {
        _soundPenel.SetActive(true);
        _optionPanel.SetActive(false);
    }

    public void ReturnMain()
    {
        SceneManager.LoadScene(_stringTitleScene);
    }

    public void ViewDisplayOption()
    {
        _displayPenel.SetActive(true);
        _optionPanel.SetActive(false);
    }

    public void ViewKeyOption()
    {
        _keyPenel.SetActive(true);
        _optionPanel.SetActive(false);
    }
}
