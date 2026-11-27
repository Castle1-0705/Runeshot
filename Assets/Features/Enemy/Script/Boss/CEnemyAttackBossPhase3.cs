using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class CEnemyAttackBossPhase3 : CEnemyAttackBossPhaseBase
{ 
    private CWeaponDataSO _otherWeapon;
    private CWeapon _weapon;
    private Transform _muzzle;
    private Transform _point;
     
    private new float _attackInterval;
    private float _attackTime;
    private float _rangeModify = 0.6f; 
      
    private float _angle;
    private float _angelInverval = 10f;
    private float _angelMax = 360f;

    private Vector2 _direction;
    private Vector3 isClockwise; 

    public CEnemyAttackBossPhase3(CEnemySetting enemySetting, int num, CWeaponDataSO weapon, float attackInterval) : base(enemySetting, num)
    { 
        _weapon = enemySetting._weapon;
        _otherWeapon = weapon;
        _muzzle = enemySetting._muzzle;
        _attackInterval = attackInterval;
        _point = enemySetting._point;
         
        InitObject(); 
    }
    private void InitObject()
    {
        _phaseInterval = 17f;
        _chargingTime = 1.5f;
        _phaseHPPercent = 0.6f;

        _sqrRange = _enemy.Enemydata.AttackRange * _enemy.Enemydata.AttackRange * _rangeModify;
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

        _ani.SetInteger("IMove", 0);

        _direction = (PlayerTr.position - _muzzle.position).normalized;

        int ranNum = Random.Range(0, 2); 
        isClockwise = ranNum == 0 ? Vector3.back : Vector3.forward;
        _point.localScale = Vector3.one;

        _isOtherPhaseAttacking = _isPhaseAttack;
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

        _phaseInterval = _phaseInterval + Random.Range(-3f,3f);
        _angelMax = _angelMax + Random.Range(-50f, 50f);

        _phaseInterval = Mathf.Clamp(_phaseInterval, 2f, 25f);
        _angelMax = Mathf.Clamp(_angelMax, 250f, 1000f);

        _angle = 0;
        _isPhaseAttack = false;
        _isPhaseInterval = true;
        _time = 0;

        _isOtherPhaseAttacking = _isPhaseAttack;
    }
    public override void ActionExit()
    {
        base.ActionExit(); 
    }

}
