using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CEnemyHPUI : MonoBehaviour
{
    #region ¿ŒΩ∫∆Â≈Õ 
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private CEnemyHPHandler _enemyHP;
    #endregion

    private void Start()
    {
        if(_enemyHP == null || _hpSlider == null)
        {
            _enemyHP = transform.parent.GetComponent<CEnemyHPHandler>();
            Debug.Log("¿ŒΩ∫∆Â≈Õ »Æ¿Œ");
        }

        if (_enemyHP == null)
        {
            return;
        }

        _enemyHP.OnHitEvent += EnemyHP; 
        _enemyHP.OnHealEvent += EnemyHP;

        IniValue(); 
    }
    private void OnDestroy()
    {
        if (_enemyHP == null)
        {
            return; 
        }

        _enemyHP.OnHitEvent -= EnemyHP; 
        _enemyHP.OnHealEvent -= EnemyHP;
    }

    private void IniValue()
    {
        _hpSlider.value = 1f;
    }

    public void EnemyHP()
    {
        if (_hpSlider == null || _enemyHP == null)
        {
            return;
        }

        if (_enemyHP.MaxHP != 0)
        {
            float enemyHP = _enemyHP.HP / _enemyHP.MaxHP;

            _hpSlider.value = enemyHP;
        }
    }
}
