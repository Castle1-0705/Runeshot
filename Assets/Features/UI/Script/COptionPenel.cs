using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class COptionPenel : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private GameObject _optionPanel;
    [SerializeField] private GameObject _soundPenel;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _resumeButton;

    [SerializeField] private string _inputKey = "ESC";
    #endregion

    #region 내부 변수 
    private bool _enabled = false;
    #endregion

    private void Start()
    {
        BindOption();
        _optionPanel.SetActive(_enabled);
        _resumeButton.onClick.AddListener(Resume);
        _exitButton.onClick.AddListener(GameEnd);
        _soundButton.onClick.AddListener(ViewSoundOption);
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
        if (CInputDispatcher.Instance.InputActionMap.ContainsKey(_inputKey))
        {
            CInputDispatcher.Instance.InputActionMap[_inputKey].OnPerformedAction -= OpenOption;
        }
    }
    public void OpenOption()
    {
        
        _enabled = !_enabled;

        Time.timeScale = _enabled ? 0 : 1f;

        if (_soundPenel.activeSelf)
        {
            _soundPenel.SetActive(false); 
            return;
        }

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
    }
}
