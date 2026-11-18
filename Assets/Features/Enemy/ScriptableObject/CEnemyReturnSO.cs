using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ActionSO/ReturnSO", fileName = "ReturnSO")]
public class CEnemyReturnSO : CEnemyActionBaseSO
{
    public override CEnemyActionBase Create(CEnemySetting enemySetting)
    {
        return new CEnemyReturn(enemySetting,_priority);
    }

}
