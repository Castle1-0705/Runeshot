 using UnityEngine;

[CreateAssetMenu(menuName = "SO/ActionSO/AttackBossPhase2SO", fileName = "AttackBossPhase2SO")]
public class CEnemyAttackBossPhase2SO : CEnemyActionBaseSO
{ 
    public override CEnemyActionBase Create(CEnemySetting enemySetting)
    {
        return new CEnemyAttackBossPhase2(enemySetting,_priority);
    }

} 
