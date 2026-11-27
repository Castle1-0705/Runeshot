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
    private LineRenderer _lineRen;
    #endregion

    #region 내부변수
    private string _stringAttackMelee = "TAttack";
    private string _stringAttackRange = "TAttackRange";
    private string _stringAttackCharge = "IMove";
    private bool _isCharging;
    private float _chargeAttackSpeed = 20f;
    private float _movementSpeed;
    private float _attackRange;
    private LayerMask _mask;
    private string _playerLayer = "Player";
    private float _rangeFireInterval = 0.5f;
    Vector3 dir;
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
        _lineRen = enemySetting._lineRen;

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

        if (distance < 2.5f)
        {
            _attackState = AttackState.MeleeAttack;
        }
        else if (distance < _enemy.Enemydata.AttackRange * 0.7f)
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
        _playerHP.OnHit(_damage*3f);
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
        dir = (PlayerTr.position - _tr.position).normalized * 3f;

        IsCharging(true);

        if (!_nav.pathPending)
        {
            
            _nav.SetDestination(PlayerTr.position + dir);
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
            _lineRen.positionCount = 2;

            _lineRen.startWidth = _tr.localScale.magnitude;
            _lineRen.endWidth = _tr.localScale.magnitude;

            _lineRen.SetPosition(0,_tr.position); 
            _lineRen.SetPosition(1,_playerHP.transform.position + dir);

            _nav.autoBraking = false;
            _nav.speed = _chargeAttackSpeed;
            _nav.stoppingDistance = 0;
            _nav.acceleration = 100f;
            ChangeAlpha(true);
            _ani.SetInteger(_stringAttackCharge, 1);
        }
        else
        {
            if(_enemy.Enemydata == null)
            {
                return;
            }
            _lineRen.positionCount = 0;

            _nav.acceleration = 15f;
            _nav.autoBraking = true;
            _nav.speed = _movementSpeed;
            _nav.stoppingDistance = _attackRange;
            _ani.SetInteger(_stringAttackCharge, 0);

            _ani.SetTrigger(_stringAttackMelee);
        }

    } 
    private bool ArriveDestination(Transform PlayerTr)
    {
        if(_col.IsTouchingLayers(_mask))
        {
            _ani.SetTrigger(_stringAttackMelee);
            _playerHP.OnHit(_damage);
            _enemySetting.TokenCancel(); 
        }

        float distance = (_nav.destination - _tr.position).sqrMagnitude;
          
        if (distance < 1f)
        {
            return true;
        }
         
        return false;
    }
   
}
