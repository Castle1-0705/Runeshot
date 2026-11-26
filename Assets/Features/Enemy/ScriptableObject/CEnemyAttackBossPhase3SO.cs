using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ActionSO/AttackBossPhase3SO", fileName = "AttackBossPhase3SO")]
public class CEnemyAttackBossPhase3SO : CEnemyActionBaseSO
{
    [SerializeField] private CWeaponDataSO _otherWeapon;
    [SerializeField] private float attackInterval; 

    public override CEnemyActionBase Create(CEnemySetting enemySetting)
    {
        return new CEnemyAttackBossPhase3(enemySetting, _priority, _otherWeapon, attackInterval);
    }
}
