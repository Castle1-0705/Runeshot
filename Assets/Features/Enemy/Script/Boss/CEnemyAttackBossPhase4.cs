using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyAttackBossPhase4 : CEnemyAttackBossPhaseBase
{ 
    private Collider2D _rangeCol;
    private Transform _colTr;
     
    private LayerMask _layer;
    private string _stringPlayer = "Player";  
    
    public CEnemyAttackBossPhase4(CEnemySetting enemySetting, int num, float range, float damageModifier) : base(enemySetting, num)
    { 
        _rangeCol = enemySetting._poCol;
        _colTr = enemySetting._coltr;

        _sqrRange = range * range; 
        _damageModifier = damageModifier;
         
        InitObject();
    }

    private void InitObject()
    {
        _phaseHPPercent = 0.6f;
        _phaseInterval = 10f;
        _chargingTime = 3f;

        _layer = 1 << LayerMask.NameToLayer(_stringPlayer);
        

        _colTr.localScale = Vector3.zero;
    }

    public override bool Condition(Transform PlayerTr)
    {
        if (PlayerTr == null)
        {
            return false;
        }

        if (_isPhaseAttack)
        {
            return true;
        }

        if (_isOtherPhaseAttacking)
        {
            return false;
        }

        if ((PlayerTr.position - _tr.position).sqrMagnitude < _sqrRange)
        {
            if (_enemyHP.HP < _enemyHP.MaxHP * _phaseHPPercent)
            {
                if (_isPhaseInterval)
                {
                    _time += Time.deltaTime;

                    if (_time < _phaseInterval)
                    {
                        return false;
                    }
                    else
                    {
                        _time = 0;
                        _isPhaseInterval = false;
                    }

                }

                _isPhaseAttack = true;
                return true;
            }
        }

        return false;
    }

    public override void ActionEnter(Transform PlayerTr)
    {
        base.ActionEnter(PlayerTr);

        _rangeCol.gameObject.SetActive(true);

        _ani.SetInteger("IMove", 0);

        _isOtherPhaseAttacking = _isPhaseAttack;

    }

    public override void ActionUpdate(Transform PlayerTr)
    {
        if (PlayerTr == null || _playerHP == null)
        {
            return;
        }

        if (_chargingTime != 0f)
        {
            _colTr.localScale = new Vector2(_time / _chargingTime, _time / _chargingTime);
        }

        _time += Time.deltaTime;

        if (_time < _chargingTime)
        {
            return;
        }
         
        _ani.SetTrigger(_attack);

        _isPhaseAttack = false;
        _isPhaseInterval = true;
        _time = 0;

        _isOtherPhaseAttacking = _isPhaseAttack;
    }

    public override void ActionExit()
    {
        base.ActionExit();

        _colTr.localScale = Vector3.zero;
        _rangeCol.gameObject.SetActive(false);
    }


}
