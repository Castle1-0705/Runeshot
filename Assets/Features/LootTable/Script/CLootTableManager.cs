using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRewardable
{

}

[System.Serializable]
public class CLootTableElement
{
    #region 인스팩터
    [SerializeField] private float _weight;
    [SerializeField] private List<string> _modifiers;
    [SerializeReference, SubclassSelector] private IRewardable _element;
    [SerializeField] private float _luckModifier = 1;
    #endregion

    #region 프로퍼티
    public IRewardable Element => _element;
    public float LuckModifier => _luckModifier;
    public float Weight
    {
        get 
        { 
            float weight = _weight;

            if(_modifiers == null || _modifiers.Count == 0)
            {
                return weight;
            }

            CPlayerCharacterController player = CPlayerCharacterController.Instance;
            if (player == null || player.ModifierHandler == null)
            {
                return Mathf.Max(0.0f, weight);
            }

            float modifier = 0;

            for (int i = 0; i < _modifiers.Count; i++)
            {
                modifier = player.ModifierHandler.GetModifiedValue(_modifiers[i], modifier);
            }

            modifier = modifier * _luckModifier;

            return Mathf.Max(0, weight + modifier); 
        }
    }
    #endregion
}

[System.Serializable]
public class CItemReward : IRewardable
{
    #region 인스팩터
    [SerializeField, Min(0)] private int _minAmount;
    [SerializeField, Min(0)] private int _maxAmount;
    [SerializeField] private CItemDataSO _item;
    #endregion


    #region 프로퍼티
    public int MinAmount => _minAmount;
    public int MaxAmount => _maxAmount;
    public CItemDataSO Item => _item;
    #endregion

}

[System.Serializable]
public class CSkillReward : IRewardable
{
    #region 인스팩터
    [SerializeField] private CSkillDataSO _skill;
    #endregion

    #region 프로퍼티
    public CSkillDataSO Skill => _skill;
    #endregion
}

public class CLootTableManager : CSingleton<CLootTableManager>
{

    public static CLootTableDataSO GetLootTable(string id)
    {
        CLootTableDataSO lootTable;
        if (string.IsNullOrEmpty(id) || CDataBaseManager.Instance == null ||
            CDataBaseManager.Instance.LootTableDB == null ||
            !CDataBaseManager.Instance.LootTableDB.TryGet(id, out lootTable))
        {
            return null;
        }

        return lootTable;
    }

    public static IRewardable[] GetLootTableElements(string id, int amount = 1)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("잘못된 메게변수 입력: 루트태이블에서 1개이상의 요소를 호출해야합니다.");
            return null;
        }

        IRewardable[] rewards = new IRewardable[amount];

        for (int i = 0; i < amount; i++)
        {
            rewards[i] = Instance.GetLootTableElement(id);
        }

        return rewards;
    }

    private IRewardable GetLootTableElement(string id)
    {
        CLootTableDataSO lootTable = GetLootTable(id);
        if (lootTable == null || lootTable.LootTableElements == null)
        {
            return null;
        }

        float ranMax = 0.0f;
        for (int i = 0; i < lootTable.LootTableElements.Count; i++)
        {
            CLootTableElement entry = lootTable.LootTableElements[i];
            if (entry != null)
            {
                ranMax += Mathf.Max(0.0f, entry.Weight);
            }
        }

        if (ranMax <= 0.0f)
        {
            return null;
        }

        float ranNum = Random.Range(0.0f, ranMax);
        IRewardable element = null;
        for (int i = 0; i < lootTable.LootTableElements.Count; i++)
        {
            CLootTableElement entry = lootTable.LootTableElements[i];
            if (entry == null || entry.Weight <= 0.0f)
            {
                continue;
            }

            element = entry.Element;
            ranNum -= entry.Weight;
            if (ranNum <= 0.0f)
            {
                break;
            }
        }

        return element;
    }
}
