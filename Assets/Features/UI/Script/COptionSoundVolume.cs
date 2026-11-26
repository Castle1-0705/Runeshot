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
    private float _maxValue = 1f;
    private float _minValue = 0.0001f;
    #endregion

    private void Awake()
    {
        _master.maxValue = _background.maxValue = _sfx.maxValue = _ui.maxValue = _maxValue;
        _master.minValue = _background.minValue = _sfx.minValue = _ui.minValue = _minValue;

        AddVol(); 
    }
     
    private void Start()
    {
        InitVol();

        gameObject.SetActive(false);
    }

    void ChangeMasterVol(float vol)
    {
        CSoundManager.Instance.Master.audioMixer.SetFloat("Master", Mathf.Log10(_master.value)*20); 
    }
    void ChangeBackGroundVol(float vol)
    {
        CSoundManager.Instance.Master.audioMixer.SetFloat("BackGround", Mathf.Log10(_background.value) * 20);
    }
    void ChangeSFXVol(float vol)
    {
        CSoundManager.Instance.Master.audioMixer.SetFloat("SFX", Mathf.Log10(_sfx.value) * 20);
    }
    void ChangeUIVol(float vol)
    {
        CSoundManager.Instance.Master.audioMixer.SetFloat("UI", Mathf.Log10(_ui.value) * 20); 
    }

    private void AddVol()
    {
        _master.onValueChanged.RemoveAllListeners();
        _background.onValueChanged.RemoveAllListeners();
        _sfx.onValueChanged.RemoveAllListeners();
        _ui.onValueChanged.RemoveAllListeners();

        _master.onValueChanged.AddListener(ChangeMasterVol);
        _background.onValueChanged.AddListener(ChangeBackGroundVol);
        _sfx.onValueChanged.AddListener(ChangeSFXVol);
        _ui.onValueChanged.AddListener(ChangeUIVol);
    }
    private void InitVol()
    {
        float vol;
        
        CSoundManager.Instance.Master.audioMixer.GetFloat("Master", out vol);
        _master.value = Mathf.Pow(10f, vol / 20f);
        CSoundManager.Instance.Master.audioMixer.GetFloat("BackGround", out vol);
        _background.value = Mathf.Pow(10f, vol / 20f);
        CSoundManager.Instance.Master.audioMixer.GetFloat("SFX", out vol);
        _sfx.value = Mathf.Pow(10f, vol / 20f);
        CSoundManager.Instance.Master.audioMixer.GetFloat("UI", out vol);
        _ui.value = Mathf.Pow(10f, vol / 20f);
    }
}
