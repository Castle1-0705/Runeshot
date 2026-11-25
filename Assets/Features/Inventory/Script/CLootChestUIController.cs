using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CLootChestUIController : CSingleton<CLootChestUIController>
{

	#region 인스팩터
	[SerializeField] private CItemSlotController[] _slots;
	[SerializeField] private GameObject _panel;
	[SerializeField] private TMP_Text _chestNameText;
	#endregion

    #region 내부변수
    private CItemContainerInventoryHandler _container;
    private CGameManager _gameManager;
    #endregion

	#region 프로퍼티
	public CItemSlotController[] Slots => _slots;
    #endregion
	  
    private void Start()
    {
        ToggleUI(false);
    }

    private void OnEnable()
    {
        BindGameManager();
        if (_gameManager != null && _gameManager.IsGameOver)
        {
            HandleGameOver();
            return;
        }
        SetChestPaused(_panel != null && _panel.activeInHierarchy);
    }

    private void OnDisable()
    {
        UnbindGameManager();
        if (_gameManager != null)
        {
            _gameManager.SetChestPaused(false);
        }

        if (_slots == null)
        {
            return;
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] != null)
            {
                _slots[i].CancelDrag();
            }
        }
    }

    protected override void OnDestroy()
    {
        UnbindGameManager();
        if (_gameManager != null)
        {
            _gameManager.SetChestPaused(false);
        }

        base.OnDestroy();
    }

    private void BindGameManager()
    {
        UnbindGameManager();
        _gameManager = CGameManager.Instance;
        if (_gameManager != null && isActiveAndEnabled)
        {
            _gameManager.OnGameOver += HandleGameOver;
        }
    }

    private void UnbindGameManager()
    {
        if (_gameManager != null)
        {
            _gameManager.OnGameOver -= HandleGameOver;
        }
    }

    private void HandleGameOver()
    {
        CloseCurrentChest();
    }

    public void CloseCurrentChest()
    {
        CLootChestController chest = _container != null ? _container.GetComponent<CLootChestController>() : null;
        if (chest != null)
        {
            chest.CloseChest();
        }
        ToggleUI(false);
        _container = null;
    }

    private void SetChestPaused(bool isPaused)
    {
        if (_gameManager == null)
        {
            BindGameManager();
        }

        if (_gameManager != null)
        {
            _gameManager.SetChestPaused(isPaused);
        }
    }

    public bool IsOpenFor(CItemContainerInventoryHandler container)
    {
        return _panel != null && _panel.activeInHierarchy && object.ReferenceEquals(_container, container);
    }

    public void UpdateSlot(CItemContainerInventoryHandler container)
	{
        if ((CGameManager.Instance != null && CGameManager.Instance.IsGameOver) || container == null || _slots == null)
        {
            return;
        }

        _container = container;
		for (int i = 0; i < _slots.Length; i++)
		{
            if (_slots[i] == null)
            {
                continue;
            }

            _slots[i].CancelDrag();
			if (i >= container.InventorySize)
			{
				_slots[i].gameObject.SetActive(false);
				continue;
			}

			_slots[i].gameObject.SetActive(true);

			CItemDataSO item = null;

			if(container.Inventory[i] != null)
			{
				item = container.Inventory[i].Item;
			}
			_slots[i].InitializeSlot(i, container, item);
		}
	}

	public void ToggleUI(bool toggle)
	{
        if (toggle && (CGameManager.Instance != null && CGameManager.Instance.IsGameOver))
        {
            return;
        }

        if (_panel == null)
        {
            return;
        }

        if (!toggle && _slots != null)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] != null)
                {
                    _slots[i].CancelDrag();
                }
            }
        }

        _panel.SetActive(toggle);

        SetChestPaused(toggle && _panel.activeInHierarchy && isActiveAndEnabled);
    }
}
