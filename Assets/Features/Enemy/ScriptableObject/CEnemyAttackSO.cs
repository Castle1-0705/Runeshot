using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ActionSO/AttackSO", fileName = "AttackSO")]
public class CEnemyAttackSO : CEnemyActionBaseSO 
{ 
    public override CEnemyActionBase Create(CEnemySetting enemySetting)
    {
        return new CEnemyAttack(enemySetting, _num);
    }
}
