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
    [SerializeField] private SpriteRenderer _meleeAttack;
    [SerializeField] private SpriteRenderer _rangeAttack;
    #endregion

    #region 내부변수
    private string _attackMelee = "TAttack";
    private string _attackRange = "TAttackRange";
    private string _attackCharge = "IMove";
    private bool _isCharging;
    private float _chargeAttackSpeed = 15f;
    private CancellationTokenSource _source;
    private LayerMask _mask;
    private string _playerLayer = "Player";
    #endregion
     
    public CEnemyAttackBoss(CEnemySetting enemySetting, int num) : base(enemySetting, num)
    { 
        _meleeAttack = enemySetting._meleeAttack;
        _rangeAttack = enemySetting._rangeAttack;

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

        _time += Time.deltaTime;

        if (_time < _attackInterval)
        {
            return;
        }

        float distance = (PlayerTr.position - _tr.position).magnitude;

        if (distance < 2f)
        { 
            ChangeAlpha(true);
            _playerHP.OnHit(_damage);
            _ani.SetTrigger(_attackMelee);
        }
        else if (distance < 6f)
        {
            ChangeAlpha(false);
            _ani.SetTrigger(_attackRange);
        } 
        else 
        { 
            _source = new CancellationTokenSource();
            ChargeAttack(PlayerTr, _source).Forget();
        }
         
        _time = 0;

    }
    public override void ActionExit()
    {
        base.ActionExit();

        TokenCancel(_source); 
    }
     
    public override void OnDisable()
    { 
        TokenCancel(_source);
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
            _ani.SetInteger(_attackCharge, 1);
        }
        else
        {
            _nav.autoBraking = true;
            _nav.speed = _enemy.Enemydata.MovementSpeed;
            _nav.stoppingDistance = _enemy.Enemydata.AttackRange;
            _ani.SetInteger(_attackCharge, 0);
        }

    } 
    private bool ArriveDestination(Transform PlayerTr)
    {
        if(_col.IsTouchingLayers(_mask))
        {
            _playerHP.OnHit(_damage);
            TokenCancel(_source); 
        }
       
        if ((_nav.destination - _tr.position).sqrMagnitude < 0.1f)
        { 
            return true;
        } 
        return false;
    }
   
}
