using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPlayerHPUI : MonoBehaviour
{
    #region ¿ŒΩ∫∆Â≈Õ 
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private CPlayerHPHandler _playerHP;
    #endregion
     
    private void Start()
    {
        if (_playerHP == null || _hpSlider == null)
        {
            _playerHP = transform.parent.GetComponent<CPlayerHPHandler>();
            Debug.Log("¿ŒΩ∫∆Â≈Õ »Æ¿Œ");
        }

        if(_playerHP == null)
        {
            return;
        }

        _playerHP.OnHitEvent += PlayerHP;
        _playerHP.OnHealEvent += PlayerHP;
        IniValue(); 
    }
    
    private void IniValue()
    {
        _hpSlider.value = 1f;
    }
    public void PlayerHP()
    {
        if(_playerHP.MaxHP != 0)
        {
            float playerhp = _playerHP.HP / _playerHP.MaxHP;
                
           _hpSlider.value = playerhp;
        } 
    }
       
}
