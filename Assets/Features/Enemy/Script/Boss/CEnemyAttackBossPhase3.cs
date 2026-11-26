using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class CEnemyAttackBossPhase3 : CEnemyAttack
{
    private CEnemyHPHandler _enemyHP; 
    private CWeaponDataSO _otherWeapon;
    private CWeapon _weapon;
    private Transform _muzzle;
    private Transform _point;
     
    private new float _attackInterval;
    private float _attackTime;
    private float _sqrRange;
    private float _rangeModify = 0.6f;
    private float _phaseHPPercent = 0.6f;

    private bool _isPhaseInverval = false;
    private float _phaseInterval = 17f;
    private bool _isPhaseAttack = false;
    private float _chargingTime = 1.5f;

    private float _angle;
    private float _angelInverval = 10f;
    private float _angelMax = 360f;

    private Vector2 _direction;
    private Vector3 isClockwise;


    public CEnemyAttackBossPhase3(CEnemySetting enemySetting, int num, CWeaponDataSO weapon, float attackInterval) : base(enemySetting, num)
    {
        _enemyHP = enemySetting.GetComponent<CEnemyHPHandler>();
        _weapon = enemySetting._weapon;
        _otherWeapon = weapon;
        _muzzle = enemySetting._muzzle;
        _attackInterval = attackInterval;
        _point = enemySetting._point;

        _sqrRange = _enemy.Enemydata.AttackRange * _enemy.Enemydata.AttackRange* _rangeModify;
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

        if ((PlayerTr.position - _tr.position).sqrMagnitude < _sqrRange)
        {
            if (_enemyHP.HP < _enemyHP.MaxHP * _phaseHPPercent)
            {
                if (_isPhaseInverval)
                {
                    _time += Time.deltaTime;

                    if (_time < _phaseInterval)
                    {
                        return false;
                    }
                    else
                    {
                        _time = 0;
                        _isPhaseInverval = false;
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

        _ani.SetInteger("IMove", 0);

        _direction = (PlayerTr.position - _muzzle.position).normalized;

        int ranNum = Random.Range(0, 2); 
        isClockwise = ranNum == 0 ? Vector3.back : Vector3.forward;
        _point.localScale = Vector3.one;
    }

    public override void ActionUpdate(Transform PlayerTr)
    {  
        if (PlayerTr == null || _playerHP == null)
        {
            return;
        }
         
        _time += Time.deltaTime;

        if (_time < _chargingTime)
        {
            return;
        }
         
        if(_point.localScale == Vector3.one)
        {
            _point.localScale = Vector3.zero;
        }
        _attackTime += Time.deltaTime;

        if (_attackTime < _attackInterval)
        {
            return;
        }
        
        _angle += _angelInverval;
          
        Quaternion quaternion = Quaternion.AngleAxis(_angle, isClockwise);
        Vector2 dir = quaternion*_direction;

        _weapon.TryFire(dir, _otherWeapon);
        _ani.SetTrigger(_attack);
        
        _attackTime = 0;
        
        if (_angle < _angelMax)
        {
            return;
        }

        _angle = 0;
        _isPhaseAttack = false;
        _isPhaseInverval = true;
        _time = 0;
    }
    public override void ActionExit()
    {
        base.ActionExit(); 
    }

}
