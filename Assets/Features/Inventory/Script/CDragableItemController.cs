using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CDragableItemController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region 인스팩터
    [SerializeField] private Image _itemImage;
    [SerializeField] private CItemSlotController _slot;
    [SerializeField] private Text _amountText;
    #endregion

    #region 내부변수
    private CItemDataSO _itemData;
    private CGameManager _gameManager;
    private IitemContainer _dragContainer;
    private bool _isDragging;
    private bool _isDragCancelled;
    private Vector3 _localPosition;
    #endregion

    #region 프로퍼티
    public CItemDataSO ItemData => _itemData;
    public EItemtype Itemtype => _itemData.ItemType;
    #endregion

    private void Awake()
    {
        if(_itemImage ==  null || _amountText == null)
        {
            Debug.LogWarning("Missing UI element");
            enabled = false;
            return;
        }
    }

    public void UpdateSlotItem(CItemDataSO item)
    {
        _itemData = item;

        if (_itemData != null)
        {
            _itemImage.enabled = true;
            _amountText.enabled = true;
            _itemImage.sprite = _itemData.ItemSprite;
            _amountText.text = $"{_slot.ItemContainer[_slot.SlotIndex].Amount}";
        }

        else
        {
            _itemImage.enabled = false;
            _amountText.enabled = false;
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragCancelled = false;
        if (!HasValidSource())
        {
            return;
        }

        _gameManager = CGameManager.Instance;
        if (_gameManager != null)
        {
            _gameManager.OnGameOver -= CancelDrag;
            _gameManager.OnGameOver += CancelDrag;
        }

        _dragContainer = _slot.ItemContainer;
        _isDragging = true;
        _localPosition = transform.localPosition;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _itemImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CGameManager.Instance != null && CGameManager.Instance.IsGameOver)
        {
            CancelDrag();
            return;
        }

        if (_isDragging)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CancelDrag();
    }

    public void CancelDrag()
    {
        if (_gameManager != null)
        {
            _gameManager.OnGameOver -= CancelDrag;
        }
        _gameManager = null;

        if (!_isDragging)
        {
            return;
        }

        _isDragging = false;
        _isDragCancelled = true;
        _dragContainer = null;
        transform.SetParent(_slot.transform);
        transform.localPosition = _localPosition;
        _itemImage.raycastTarget = true;
    }

    private bool HasValidSource()
    {
        if ((CGameManager.Instance != null && CGameManager.Instance.IsGameOver) || _isDragCancelled || _slot == null || !_slot.gameObject.activeInHierarchy || _itemData == null ||
            _slot.ItemContainer == null || _slot.SlotIndex < 0 ||
            _slot.SlotIndex >= _slot.ItemContainer.Inventory.Count ||
            (_isDragging && _dragContainer != _slot.ItemContainer))
        {
            return false;
        }

        CItemSlotData source = _slot.ItemContainer[_slot.SlotIndex];
        return source != null && source.Item == _itemData && source.Amount > 0;
    }

    public bool TryEquipWeapon()
    {
        if (!HasValidSource() || !(_slot.ItemContainer is CItemContainerInventoryHandler) ||
            _itemData.ItemType != EItemtype.Weapon || !_itemData.TryEquipWeapon())
        {
            return false;
        }

        _slot.ItemContainer.RemoveFromInventory(_slot.SlotIndex, 1);
        return true;
    }

    public void SetSlot(CItemSlotController slot)
    {
        if (slot == null || !slot.gameObject.activeInHierarchy || !HasValidSource() || _slot == slot)
        {
            return;
        }

        if (slot is CItemWeaponSlotController)
        {
            TryEquipWeapon();
            return;
        }

        if (slot.SlotIndex < 0 || slot.ItemContainer == null ||
            slot.SlotIndex >= slot.ItemContainer.Inventory.Count)
        {
            return;
        }

        if(slot.ItemContainer == null || _slot.ItemContainer == null)
        {
            Debug.LogWarning("아이템 컨테이너 누락");
            return;
        }

        if ((slot.SlotType != EItemtype.None && slot.SlotType != _itemData.ItemType) ||
            (slot.ItemContainer is CInventoryManager && _itemData.ItemType == EItemtype.Weapon))
        {
            return;
        }

        IitemContainer originContainer = _slot.ItemContainer;
        IitemContainer targetContainer = slot.ItemContainer;

        int amount = originContainer[_slot.SlotIndex].Amount;
        int leftAmount;

        CItemSlotData originSlotData = originContainer[_slot.SlotIndex];
        CItemSlotData targetSlotData = targetContainer[slot.SlotIndex];

        if (targetSlotData != null && (targetSlotData.Item == null || targetSlotData.Amount <= 0))
        {
            return;
        }

        if (targetSlotData != null && targetSlotData.Item != _itemData &&
            ((_slot.SlotType != EItemtype.None && _slot.SlotType != targetSlotData.Item.ItemType) ||
             (originContainer is CInventoryManager && targetSlotData.Item.ItemType == EItemtype.Weapon)))
        {
            return;
        }

        if (targetSlotData != null)
        {
            if (targetSlotData.Item != _itemData)
            {
                originContainer.RemoveFromInventory(_slot.SlotIndex);
                targetContainer.RemoveFromInventory(slot.SlotIndex);

                originContainer.AddToInventory(targetSlotData.Item, _slot.SlotIndex, targetSlotData.Amount, out leftAmount);
                targetContainer.AddToInventory(originSlotData.Item, slot.SlotIndex, originSlotData.Amount, out leftAmount);
                return;
            }
        }

        if (targetContainer.AddToInventory(_itemData, slot.SlotIndex, amount, out leftAmount) && amount > leftAmount)
        {
            originContainer.RemoveFromInventory(_slot.SlotIndex, amount - leftAmount);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_itemData != null)
        {
            CTooltipController.ShowTooltip(_itemData.Name, _itemData.Description,this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CTooltipController.HideTooltip(this);
    }

    private void OnDisable()
    {
        CancelDrag();
        if(CTooltipController.IsDestroyed)
        {
            return;
        }

        CTooltipController.HideTooltip(this);
    }
}
