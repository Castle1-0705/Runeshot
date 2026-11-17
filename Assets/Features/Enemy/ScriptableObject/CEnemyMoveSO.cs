using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ActionSO/MoveSO", fileName = "MoveSO")]
public class CEnemyMoveSO : CEnemyActionBaseSO
{
    public override CEnemyActionBase Create(CEnemySetting enemySetting)
    {
        return new CEnemyMove(enemySetting,_num);
    }

}
