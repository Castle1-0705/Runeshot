using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ActionSO/IdleSO", fileName = "IdleSO")]
public class CEnemyIdleSO : CEnemyActionBaseSO
{
    public override CEnemyActionBase Create(CEnemySetting enemySetting)
    {
        return new CEnemyIdle(enemySetting, _priority);
    }

}
