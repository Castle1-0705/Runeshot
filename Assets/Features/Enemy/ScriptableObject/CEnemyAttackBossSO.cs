using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ActionSO/AttackBossSO", fileName = "AttackBossSO")]
public class CEnemyAttackBossSO : CEnemyActionBaseSO
{
    public override CEnemyActionBase Create(CEnemySetting enemySetting)
    {
        return new CEnemyAttackBoss(enemySetting, _num);
    } 
}
