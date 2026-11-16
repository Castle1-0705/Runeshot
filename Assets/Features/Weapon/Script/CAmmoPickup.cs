using System.Collections.Generic;
using UnityEngine;

public class CAmmoPickup : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private CInventoryManager _inventory;
    [SerializeField] private CItemDataSO _ammoItem;
    [SerializeField] private int _amount;
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        CPlayerStateController player = other.GetComponentInParent<CPlayerStateController>();
        if (player == null || player.CurrentState == EPlayerState.Dead || _inventory == null ||
            _ammoItem == null || _ammoItem.ItemType != EItemtype.Ammunition ||
            _amount <= 0 || !gameObject.activeInHierarchy)
        {
            return;
        }

        int before = GetAmmoAmount();
        if (_inventory.AddToInventory(_ammoItem, _amount))
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }

        // 인벤토리가 가득 차면 들어간 수량만 차감
        _amount -= GetAmmoAmount() - before;
    }

    private int GetAmmoAmount()
    {
        if (!_inventory.InventoryItemMap.TryGetValue(_ammoItem, out List<int> indices))
        {
            return 0;
        }

        int amount = 0;
        for (int i = 0; i < indices.Count; i++)
        {
            amount += _inventory[indices[i]].Amount;
        }

        return amount;
    }
}
