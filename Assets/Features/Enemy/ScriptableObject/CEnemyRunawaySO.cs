using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ActionSO/RunawaySO", fileName = "RunawaySO")]
public class CEnemyRunawaySO : CEnemyActionBaseSO
{
    public override CEnemyActionBase Create(CEnemySetting enemySetting)
    {
        return new CEnemyRunaway(enemySetting,_num);
    }
}
