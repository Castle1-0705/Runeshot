using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPlayerHPUI : MonoBehaviour
{

    [SerializeField] Slider _hpSlider;
    [SerializeField] CPlayerHPHandler _playerHP; 

    private void Start()
    {
        if (_playerHP == null || _hpSlider == null)
        {
            _playerHP = transform.parent.GetComponent<CPlayerHPHandler>();
            Debug.Log("인스펙터 확인");
        }

        if(_playerHP == null)
        {
            return;
        }

        _playerHP.OnHitEvent += PlayerHP;
        PlayerHP();
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
