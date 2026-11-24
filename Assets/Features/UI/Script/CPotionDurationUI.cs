using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

[Serializable]
public class Sliders
{
    public Slider _slider;
    public int _num = 1;

    public Sliders(Slider slider)
    {
        _slider = slider;
    }
}

public class CPotionDurationUI : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _potion;
    [SerializeField] private CInventoryInputController _controller;
    [SerializeField] private Transform _layout;
    #endregion
     
    #region 내부 변수  
    private Dictionary<string, Sliders> _overTimes = new Dictionary<string, Sliders>(); 
    private Dictionary<string, Sliders> _effectToPlayerHPs = new Dictionary<string, Sliders>(); 
    private Dictionary<string, Sliders> _modifiers = new Dictionary<string, Sliders>(); 
    #endregion
    
    private void Awake()
    {
        if(_controller == null)
        { 
            Debug.Log("포션 UI에 컨트롤러가 없다");
            return;
        }
        _controller.durationUI += PotionDuration; 
    }
    void PotionDuration(CItemDataSO item)
    { 
        _potion.sprite = item.ItemSprite;
         
        for (int i = 0; i < item.Effect.Length; i++)
        {
            if(item.Effect[i] == null)
            {
                return;
            }

            if (item.Effect[i] is CItemEffectHealOverTime)
            {
                CItemEffectHealOverTime overTime = (CItemEffectHealOverTime)item.Effect[i];

                //TODO
                //but 사용할 이유가 없음

            }
            else if(item.Effect[i] is CItemEffectAddStatusEffectToPlayerHP)
            { 
                CItemEffectAddStatusEffectToPlayerHP effectToPlayerHP = (CItemEffectAddStatusEffectToPlayerHP)item.Effect[i];
                
                if (_effectToPlayerHPs.ContainsKey(effectToPlayerHP.StatusEffect.EffectName))
                {
                    if (_effectToPlayerHPs[effectToPlayerHP.StatusEffect.EffectName]._slider.IsActive())
                    {
                        AddDuration(effectToPlayerHP.StatusEffect, _effectToPlayerHPs[effectToPlayerHP.StatusEffect.EffectName]);
                    }
                    else
                    {
                        _effectToPlayerHPs[effectToPlayerHP.StatusEffect.EffectName]._slider.gameObject.SetActive(true);
                        Duration(effectToPlayerHP, _effectToPlayerHPs[effectToPlayerHP.StatusEffect.EffectName]).Forget();
                    }
                }
                else
                {
                    InitSlider(effectToPlayerHP);

                    //실행
                    Duration(effectToPlayerHP, _effectToPlayerHPs[effectToPlayerHP.StatusEffect.EffectName]).Forget();
                }
                   
                
            }
            else if(item.Effect[i] is CItemEffectAddStatusEffectToPlayerModifier)
            {
                CItemEffectAddStatusEffectToPlayerModifier modifier = (CItemEffectAddStatusEffectToPlayerModifier)item.Effect[i];
                
                if (_modifiers.ContainsKey(modifier.StatusEffect.EffectName))
                {
                    if (_modifiers[modifier.StatusEffect.EffectName]._slider.IsActive())
                    {
                        AddDuration(modifier.StatusEffect, _modifiers[modifier.StatusEffect.EffectName]);
                    }
                    else
                    {
                        _modifiers[modifier.StatusEffect.EffectName]._slider.gameObject.SetActive(true);
                        Duration(modifier, _modifiers[modifier.StatusEffect.EffectName]).Forget();
                    }
                }
                else
                {
                    InitSlider(modifier);

                    //실행
                    Duration(modifier, _modifiers[modifier.StatusEffect.EffectName]).Forget();
                }
            }
              
        }

    }

    #region InitSlider
    private void InitSlider(CItemEffectAddStatusEffectToPlayerHP effectToPlayerHP)
    {
        if(_slider == null || _layout == null || _potion == null)
        {
            return;
        }
        //생성
         
        Slider slider = Instantiate(_slider, _layout);
        Image image = Instantiate(_potion, slider.transform);
        Sliders sliders = new Sliders(slider);

        //저장
        _effectToPlayerHPs.Add(effectToPlayerHP.StatusEffect.EffectName, sliders);

        if(effectToPlayerHP.StatusEffect.Duration == 0)
        {
            Debug.Log("지속시간이 0");
            return;
        }

        //초기화
        slider.fillRect = image.rectTransform;
        slider.value = effectToPlayerHP.StatusEffect.DurationTimer / effectToPlayerHP.StatusEffect.Duration;
    }
    private void InitSlider(CItemEffectAddStatusEffectToPlayerModifier modifier)
    {
        //생성
        Slider slider = Instantiate(_slider, _layout);
        Image image = Instantiate(_potion, slider.transform);
        Sliders sliders = new Sliders(slider);

        //저장
        _modifiers.Add(modifier.StatusEffect.EffectName, sliders);

        if (modifier.StatusEffect.Duration == 0)
        {
            Debug.Log("지속시간이 0");
            return;
        }

        //초기화
        slider.fillRect = image.rectTransform;
        slider.value = modifier.StatusEffect.DurationTimer / modifier.StatusEffect.Duration;
    }
    #endregion

    #region Duration
    private async UniTask Duration(CItemEffectAddStatusEffectToPlayerHP effectToPlayerHP, Sliders sliders)
    {
        if (effectToPlayerHP == null || sliders == null)
        {
            return;
        }

        await UniTask.WaitUntil(() => RefreshDuration(effectToPlayerHP, sliders),PlayerLoopTiming.Update,destroyCancellationToken);

        sliders._num = 1;
        sliders._slider.gameObject.SetActive(false);
    }
    private async UniTask Duration(CItemEffectAddStatusEffectToPlayerModifier modifier, Sliders sliders)
    {
        if (modifier == null || sliders == null)
        {
            return;
        }

        await UniTask.WaitUntil(() => RefreshDuration(modifier, sliders),PlayerLoopTiming.Update,destroyCancellationToken);

        sliders._num = 1;
        sliders._slider.gameObject.SetActive(false);
    }
    #endregion

    #region RefreshDuration
    private bool RefreshDuration(CItemEffectAddStatusEffectToPlayerHP effectToPlayerHP, Sliders sliders)
    {
        if(effectToPlayerHP == null || sliders == null)
        {
            return true;
        }

        sliders._slider.value = effectToPlayerHP.StatusEffect.DurationTimer / (effectToPlayerHP.StatusEffect.Duration * sliders._num);
         
        if (effectToPlayerHP.StatusEffect.DurationTimer <= 0)
        {
            return true;
        }

        return false;
    }
    private bool RefreshDuration(CItemEffectAddStatusEffectToPlayerModifier modifier, Sliders sliders)
    {
        if (modifier == null || sliders == null)
        {
            return true;
        }

        sliders._slider.value = modifier.StatusEffect.DurationTimer / (modifier.StatusEffect.Duration * sliders._num);
         
        if(modifier.StatusEffect.DurationTimer <= 0)
        {
            return true;
        }

        return false;
    }
    #endregion

    private void AddDuration(CStatusEffect effect,Sliders sliders)
    { 
        switch (effect.StackType)
        {
            case EStatusEffectStackType.None:
                sliders._num = 1;
                Debug.Log("EStatusEffectStackType이 None");
                break;
            case EStatusEffectStackType.Refreshable:
                sliders._num = 1;
                break;
            case EStatusEffectStackType.Stackable:
                sliders._num++;
                break;
            case EStatusEffectStackType.Extendable:
                sliders._num++;
                break;
        } 
    }
     
} 