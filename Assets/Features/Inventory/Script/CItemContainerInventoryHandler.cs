using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CItemContainerInventoryHandler : MonoBehaviour, IitemContainer
{
    #region 인스팩터
    [SerializeField] private int _inventorySize;
    [SerializeField] private List<CItemSlotController> _inventorySlots;
    #endregion

    #region 내부변수
    private List<CItemSlotData> _inventory = new List<CItemSlotData>();
    #endregion

    #region 프로퍼티
    public IReadOnlyList<CItemSlotData> Inventory => _inventory;
    public int InventorySize => _inventorySize;
    #endregion

    #region 인덱서
    public CItemSlotData this[int index] => _inventory[index];
    #endregion


    public void InitializeInventory(int inventorySize)
    {
        CItemSlotController[] slots = CLootChestUIController.Instance.Slots;

        if (slots == null)
        {
            return;
        }

        _inventorySize = Mathf.Clamp(inventorySize, 0, slots.Length);
        _inventory.Clear();
        if (_inventorySlots == null)
        {
            _inventorySlots = new List<CItemSlotController>();
        }
        _inventorySlots.Clear();

        for (int i = 0; i < _inventorySize; i++)
        {
            _inventory.Add(null);

            CItemSlotController slot = slots[i];

            if (slot != null)
            {
                slot.InitializeSlot(i, this, null);
            }
            _inventorySlots.Add(slot);
        }
    }

    private void UpdateSlot(int index)
    {
        CItemSlotController slot = _inventorySlots[index];
        if (slot != null && object.ReferenceEquals(slot.ItemContainer, this))
        {
            slot.UpdateSlot(_inventory[index] != null ? _inventory[index].Item : null);
        }
    }

    public bool AddToInventory(CItemDataSO item, int amount)
    {
        if ((CGameManager.Instance != null && !CGameManager.Instance.IsPlaying) || item == null || amount <= 0 || item.MaxStack <= 0)
        {
            return false;
        }
        for (int i = 0; i < _inventory.Count; i++)
        {
            if (_inventorySlots[i] == null || _inventorySlots[i].SlotType != EItemtype.None)
            {
                continue;
            }

            if (_inventory[i] == null || _inventory[i].Item == item)
            {
                AddToInventory(item, i, amount, out amount);

                if (amount <= 0)
                {
                    return true;
                }
            }
        }

        Debug.LogWarning("아이템 컨테이너 인밴토리가 가득참");
        return false;
    }

    public bool AddToInventory(CItemDataSO item, int index, int amount, out int leftAmount)
    {
        leftAmount = amount;
        if ((CGameManager.Instance != null && !CGameManager.Instance.IsPlaying) || item == null || item.MaxStack <= 0 || amount <= 0 || index < 0 || index >= _inventory.Count ||
            _inventorySlots[index] == null ||
            (_inventorySlots[index].SlotType != EItemtype.None && _inventorySlots[index].SlotType != item.ItemType))
        {
            return false;
        }

        if (_inventory[index] == null)
        {
            _inventory[index] = new CItemSlotData(item, amount);
            UpdateSlot(index);

            if (amount <= item.MaxStack)
            {
                leftAmount = 0;
            }

            else
            {
                leftAmount = amount - item.MaxStack;
            }

            return true;
        }

        if (_inventory[index].Item != item && _inventory[index].Item != null)
        {
            Debug.Log("이미 이 아이템 슬롯을 점유하고 있음");
            leftAmount = amount;
            return false;
        }

        if (_inventory[index].AddToSlot(item, amount, out leftAmount))
        {
            UpdateSlot(index);

            return true;
        }

        leftAmount = amount;
        return false;
    }

    public void RemoveFromInventory(int index)
    {
        if (CGameManager.Instance != null && !CGameManager.Instance.IsPlaying)
        {
            return;
        }



        if (index < 0 || index >= _inventory.Count || _inventory[index] == null)
        {
            Debug.LogWarning("이 아이템슬롯은 이미 비어있음");
            return;
        }

        _inventory[index] = null;
        UpdateSlot(index);
    }

    public void RemoveFromInventory(int index, int amount)
    {
        if (CGameManager.Instance != null && !CGameManager.Instance.IsPlaying)
        {
            return;
        }

        if (amount <= 0)
        {
            return;
        }

        if (_inventory[index] == null)
        {
            Debug.LogWarning("이 아이템슬롯은 이미 비어있음");
            return;
        }

        if (!_inventory[index].RemoveFormSlot(amount))
        {
            _inventory[index] = null;
            UpdateSlot(index);
            return;
        }

        UpdateSlot(index);
    }
}
