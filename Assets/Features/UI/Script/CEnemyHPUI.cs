using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CEnemyHPUI : MonoBehaviour
{

    [SerializeField] Slider _hpSlider;
    [SerializeField] CEnemyHPHandler _enemyHP;

    private void Start()
    {
        if(_enemyHP == null || _hpSlider == null)
        {
            _enemyHP = transform.parent.GetComponent<CEnemyHPHandler>();
            Debug.Log("인스펙터 확인");
        }

        if (_enemyHP == null)
        {
            return;
        }
        _enemyHP.OnHitEvent += EnemyHP;
        EnemyHP();
    }

    public void EnemyHP()
    {
        if (_enemyHP.MaxHP != 0)
        {
            float playerhp = _enemyHP.HP / _enemyHP.MaxHP;

            _hpSlider.value = playerhp;
        }
    }
}
