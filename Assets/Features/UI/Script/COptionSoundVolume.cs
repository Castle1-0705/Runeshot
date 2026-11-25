using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class COptionSoundVolume : MonoBehaviour
{
    #region 인스펙터 
    [SerializeField] private GameObject _optionPanel;
                     
    [SerializeField] private Slider _master;
    [SerializeField] private Slider _background;
    [SerializeField] private Slider _sfx;
    [SerializeField] private Slider _ui;
    #endregion

    #region 내부 변수 
    private float _maxValue = 10f;
    private float _minValue = -80f;
    #endregion

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
     
}
