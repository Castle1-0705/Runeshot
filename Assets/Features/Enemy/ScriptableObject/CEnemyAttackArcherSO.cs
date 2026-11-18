using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ActionSO/AttackArcherSO", fileName = "AttackArcherSO")]
public class CEnemyAttackArcherSO : CEnemyActionBaseSO
{
    public override CEnemyActionBase Create(CEnemySetting enemySetting)
    {
        return new CEnemyAttackArcher(enemySetting, _priority);
    }

}
