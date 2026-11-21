using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI; 

public class CEnemyAttackBoss : CEnemyAttack
{
    #region 인스펙터 
    private SpriteRenderer _meleeAttack;
    private SpriteRenderer _rangeAttack;
    private CEnemySetting _enemySetting;
    private CWeapon _rangeWeaponAttack;
    private Transform _muzzle;
    #endregion

    #region 내부변수
    private string _stringAttackMelee = "TAttack";
    private string _stringAttackRange = "TAttackRange";
    private string _stringAttackCharge = "IMove";
    private bool _isCharging;
    private float _chargeAttackSpeed = 15f;
    private float _movementSpeed;
    private float _attackRange;
    private LayerMask _mask;
    private string _playerLayer = "Player";
    private float _rangeFireInterval = 0.5f;
    #endregion

    private AttackState _attackState;
    private enum AttackState
    {
        MeleeAttack,
        RangeAttack,
        ChargeAttack,
    }
    public CEnemyAttackBoss(CEnemySetting enemySetting, int num) : base(enemySetting, num)
    { 
        _meleeAttack = enemySetting._meleeAttack;
        _rangeAttack = enemySetting._rangeAttack;
        _enemySetting = enemySetting;

        _rangeWeaponAttack = enemySetting._weapon;
        _muzzle = enemySetting._muzzle;

        _movementSpeed = _enemy.Enemydata.MovementSpeed;
        _attackRange = _enemy.Enemydata.AttackRange;

        _mask = 1 << LayerMask.NameToLayer(_playerLayer);
    }
    
     
    public override void ActionUpdate(Transform PlayerTr)
    {
        if (PlayerTr == null || _playerHP == null)
        {
            return;
        }

        if (_isCharging)
        {
            return;
        }

        Flip(PlayerTr.position);
         
        float distance = (PlayerTr.position - _tr.position).magnitude;

        if (distance < 1.5f)
        {
            _attackState = AttackState.MeleeAttack;
        }
        else if (distance < _enemy.Enemydata.AttackRange * 0.8f)
        {
            _attackState = AttackState.RangeAttack;
        }
        else
        {
            _attackState = AttackState.ChargeAttack;
        }

        if (_attackState != AttackState.RangeAttack)
        {
            _time += Time.deltaTime;

            if (_time < _attackInterval)
            {
                return;
            }
        }
        else
        {
            _time += Time.deltaTime;

            if (_time < _rangeFireInterval)
            {
                return;
            }
        }
         
        Attack(PlayerTr);

        _time = 0;

    }
    public override void ActionExit()
    {
        base.ActionExit();
        _enemySetting.TokenCancel(); 
    }
     
    private void Attack(Transform PlayerTr)
    {
        switch (_attackState)
        {
            case AttackState.MeleeAttack:
                MeleeAttack();
                break;
            case AttackState.RangeAttack:
                RangeAttack(PlayerTr);
                break;
            case AttackState.ChargeAttack:
                ChargeAttack(PlayerTr);
                break;
        }
    }

    private void MeleeAttack()
    {
        ChangeAlpha(true);
        _playerHP.OnHit(_damage);
        _ani.SetTrigger(_stringAttackMelee);
    }

    private void RangeAttack(Transform PlayerTr)
    {
        ChangeAlpha(false);
        Vector2 direction = (PlayerTr.position - _muzzle.position).normalized;

        _rangeWeaponAttack.TryFire(direction);

        _ani.SetTrigger(_stringAttackRange);
    }

    private void ChargeAttack(Transform PlayerTr)
    {
        ChargeAttack(PlayerTr, _enemySetting.TokenGen()).Forget();
    }
      
    private void ChangeAlpha(bool isMeleeAttack)
    {
        Color colorMelee = _meleeAttack.color;
        Color colorRange = _rangeAttack.color;

        if (isMeleeAttack)
        {
            colorMelee.a = 1f;
            colorRange.a = 0f;
        }
        else
        {
            colorMelee.a = 0f;
            colorRange.a = 1f;
        }

        _meleeAttack.color = colorMelee;
        _rangeAttack.color = colorRange;
    }

    private async UniTask ChargeAttack(Transform PlayerTr, CancellationTokenSource source)
    {

        IsCharging(true);

        if (!_nav.pathPending)
        {
            _nav.SetDestination(PlayerTr.position);
        }

        try
        {
            await UniTask.WaitUntil(() => ArriveDestination(PlayerTr), PlayerLoopTiming.Update, source.Token);
        }
        finally
        {
            IsCharging(false);
        }
          
    }

    private void IsCharging(bool ischarging)
    {
        _isCharging = ischarging;

        if (_isCharging)
        {
            _nav.autoBraking = false;
            _nav.speed = _chargeAttackSpeed;
            _nav.stoppingDistance = 0;
            _ani.SetInteger(_stringAttackCharge, 1);
        }
        else
        {
            if(_enemy.Enemydata == null)
            {
                return;
            }

            _nav.autoBraking = true;
            _nav.speed = _movementSpeed;
            _nav.stoppingDistance = _attackRange;
            _ani.SetInteger(_stringAttackCharge, 0);
        }

    } 
    private bool ArriveDestination(Transform PlayerTr)
    {
        if(_col.IsTouchingLayers(_mask))
        {
            _playerHP.OnHit(_damage);
            _enemySetting.TokenCancel(); 
        }
       
        if ((_nav.destination - _tr.position).sqrMagnitude < 0.1f)
        { 
            return true;
        } 
        return false;
    }
   
}
