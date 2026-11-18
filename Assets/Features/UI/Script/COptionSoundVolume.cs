using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class COptionSoundVolume : MonoBehaviour
{
    [SerializeField] GameObject _optionPanel;

    [SerializeField] Slider _master;
    [SerializeField] Slider _background;
    [SerializeField] Slider _sfx;
    [SerializeField] Slider _ui;

    private float _maxValue = 10f;
    private float _minValue = -40f;

    private void Awake()
    {
        _master.maxValue = _background.maxValue = _sfx.maxValue = _ui.maxValue = _maxValue;
        _master.minValue = _background.minValue = _sfx.minValue = _ui.minValue = _minValue;
         
        _master.onValueChanged.AddListener(ChangeMasterVol);
        _background.onValueChanged.AddListener(ChangeBackGroundVol);
        _sfx.onValueChanged.AddListener(ChangeSFXVol);
        _ui.onValueChanged.AddListener(ChangeUIVol);

        gameObject.SetActive(false);
    }
  
    void ChangeMasterVol(float vol)
    {
        CSoundManager.Instance.Master.audioMixer.SetFloat("Master", _master.value); 
    }
    void ChangeBackGroundVol(float vol)
    {
        CSoundManager.Instance.Master.audioMixer.SetFloat("BackGround", _background.value);
    }
    void ChangeSFXVol(float vol)
    {
        CSoundManager.Instance.Master.audioMixer.SetFloat("SFX", _sfx.value);
    }
    void ChangeUIVol(float vol)
    {
        CSoundManager.Instance.Master.audioMixer.SetFloat("UI", _ui.value); 
    }

    private void OnEnable()
    {
        _optionPanel.gameObject.SetActive(false);
    }

    //private void OnDisable()
    //{
    //    _optionPanel.gameObject.SetActive(true);
    //}
}
