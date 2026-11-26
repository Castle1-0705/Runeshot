 using UnityEngine;

[CreateAssetMenu(menuName = "SO/ActionSO/AttackBossPhase2SO", fileName = "AttackBossPhase2SO")]
public class CEnemyAttackBossPhase2SO : CEnemyActionBaseSO
{
    [SerializeField] private float _damageModifier = 3f;
    public float _range = 4f;

    public override CEnemyActionBase Create(CEnemySetting enemySetting)
    {
        return new CEnemyAttackBossPhase2(enemySetting,_priority, _range, _damageModifier);
    }

} 
