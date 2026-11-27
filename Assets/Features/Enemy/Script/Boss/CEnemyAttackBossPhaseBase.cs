using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyAttackBossPhaseBase : CEnemyAttack
{
    protected CEnemyHPHandler _enemyHP;


    protected float _sqrRange;

    protected bool _isPhaseInterval = false; 
    protected bool _isPhaseAttack = false;
    protected static bool _isOtherPhaseAttacking = false;

    protected float _phaseInterval;
    protected float _chargingTime;

    protected float _phaseHPPercent;

    protected float _damageModifier;

    public CEnemyAttackBossPhaseBase(CEnemySetting enemySetting, int num) : base(enemySetting, num)
    {
        _enemyHP = enemySetting._enemyHP;
    }
}
