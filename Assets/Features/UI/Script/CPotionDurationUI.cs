using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPotionDurationUI : MonoBehaviour
{
    [SerializeField] CStatusEffectHandler _effectHandler;
    [SerializeField] private Image _potion;
     
    #region 내부 변수
    private float _duration;
    #endregion

    private void Awake()
    {
        //_effectHandler.ActionApply += GetDuration;
    }
    void GetDuration(CStatusEffect effect)
    {
        _duration += effect.Duration; 

        this.gameObject.SetActive(true);
    }

    private void Update()
    {
        _duration -= Time.deltaTime;
    }
    private void DurationUI()
    {
        _potion.fillAmount = _duration;
    }
}
