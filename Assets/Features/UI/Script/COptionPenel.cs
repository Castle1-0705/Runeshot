using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class COptionPenel : MonoBehaviour
{

    [SerializeField] GameObject _optionPanel;
    [SerializeField] GameObject _soundPenel;
    [SerializeField] Button _exitButton;
    [SerializeField] Button _soundButton;

    [SerializeField] Button _resumeButton;

    private bool _enabled = false;
    [SerializeField] private string _inputKey = "ESC";
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
