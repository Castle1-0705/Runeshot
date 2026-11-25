using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class CEnemyAttack : CEnemyActionBase
{
     
    #region 내부변수
    protected float _attackInterval; 
    protected float _damage; 
    protected float _time;
    protected string _attack = "TAttack";
    protected CPlayerHPHandler _playerHP;
    #endregion
     
    public CEnemyAttack(CEnemySetting enemySetting, int num) : base(enemySetting, num)
    {
        _attackInterval = _enemy.Enemydata.WeaponFireInterval;
        _damage = _enemy.Enemydata.WeaponDamage;
    }

    public override bool Condition(Transform PlayerTr)
    {
        if(PlayerTr == null)
        {
            return false;
        }

        if((PlayerTr.position - _tr.position).sqrMagnitude < _enemy.Enemydata.AttackRange * _enemy.Enemydata.AttackRange)
        {
            return true;
        }

        return false;
    }
    public override void ActionEnter(Transform PlayerTr)
    {
        Debug.Log("Enter Attack"); 
        if (_playerHP == null)
        {
            _playerHP = PlayerTr.GetComponent<CPlayerHPHandler>();
        }
    }
    public override void ActionUpdate(Transform PlayerTr)
    {
       if (PlayerTr == null || _playerHP == null)
       {
           return;
       }
      
        Flip(PlayerTr.position);

        _time += Time.deltaTime;

        if (_time < _attackInterval)
        {
            return;
        }

        _playerHP.OnHit(_damage);

        _ani.SetTrigger(_attack);
        _time = 0;
    }

    public override void ActionExit()
    {
        Debug.Log("Exit Attack");
        _time = 0;
    }
     
}
