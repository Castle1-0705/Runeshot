using UnityEngine;

public class CBattleMapStartingItems : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private CWeaponDataSO _pistol;
    #endregion

    private void Start()
    {
        CInventoryManager inventory = GetComponent<CInventoryManager>();
        if (inventory == null || inventory.Inventory.Count == 0 || _pistol == null || _pistol.AmmoItem == null)
        {
            Debug.LogWarning("BattleMap 시작 아이템 참조가 비어있음");
            return;
        }

        if (!inventory.InventoryItemMap.ContainsKey(_pistol.AmmoItem))
        {
            inventory.AddToInventory(_pistol.AmmoItem, 24);
        }
    }
}
