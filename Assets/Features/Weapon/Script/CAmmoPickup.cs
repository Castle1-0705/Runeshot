using System.Collections.Generic;
using UnityEngine;

public class CAmmoPickup : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private CItemDataSO _ammoItem;
    [SerializeField] private int _amount;
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        CPlayerStateController player = other.GetComponentInParent<CPlayerStateController>();
        if (player == null || player.CurrentState == EPlayerState.Dead ||
            _ammoItem == null || _ammoItem.ItemType != EItemtype.Ammunition ||
            _amount <= 0 || !gameObject.activeInHierarchy)
        {
            return;
        }

        CInventoryManager inventory = FindInventory();
        if (inventory == null)
        {
            return;
        }

        int before = GetAmmoAmount(inventory);
        if (inventory.AddToInventory(_ammoItem, _amount))
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }

        // 인벤토리가 가득 차면 들어간 수량만 차감
        _amount -= GetAmmoAmount(inventory) - before;
    }

    private CInventoryManager FindInventory()
    {
        CInventoryManager[] inventories = FindObjectsByType<CInventoryManager>(FindObjectsSortMode.None);
        for (int i = 0; i < inventories.Length; i++)
        {
            CInventoryManager inventory = inventories[i];
            if (inventory.gameObject.scene == gameObject.scene && inventory.isActiveAndEnabled &&
                inventory.Inventory.Count > 0)
            {
                return inventory;
            }
        }

        return null;
    }

    private int GetAmmoAmount(CInventoryManager inventory)
    {
        if (!inventory.InventoryItemMap.TryGetValue(_ammoItem, out List<int> indices))
        {
            return 0;
        }

        int amount = 0;
        for (int i = 0; i < indices.Count; i++)
        {
            amount += inventory[indices[i]].Amount;
        }

        return amount;
    }
}
