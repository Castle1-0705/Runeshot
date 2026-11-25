using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CItemContainerInventoryHandler))]
public class CLootChestController : MonoBehaviour, IInteractable
{
    #region 인스팩터
    [SerializeField] private CLootChestDataSO _data;
    [SerializeField] private CItemContainerInventoryHandler _inventory;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Canvas _InteractIcon;
    #endregion

    #region 내부변수
    private bool _isOpend = false;
    private CInputActionData _interactAction;
    private CLootChestUIController _chestUI;
    #endregion

    private void Awake()
    { 
        if (_inventory == null)
        {
            if(!TryGetComponent<CItemContainerInventoryHandler>(out _inventory))
            {
                Debug.LogWarning("아이템 컨테이너 인벤토리 누락");
                enabled = false;
                return;
            }
        }

        if(_spriteRenderer == null)
        {
            if(!TryGetComponent<SpriteRenderer>(out _spriteRenderer))
            {
                Debug.LogWarning("스프라이트 렌더러 누락");
                enabled = false;
                return;
            }
        }


        if(_data == null)
        {
            Debug.LogWarning("CLootChestDataSO 누락");
            enabled = false;
            return;
        }

        if(_InteractIcon == null)
        {
            Debug.LogWarning("_InteractIcon 누락");
            enabled = false;
            return;
        }

        _spriteRenderer.sprite = _data.ChestSprite;
        _InteractIcon.gameObject.SetActive(false);

    }

    private void Start()
    {
        CInputDispatcher dispatcher = CInputDispatcher.Instance;
        if (dispatcher != null && dispatcher.InputActionMap.TryGetValue("Interact", out _interactAction))
        {
            _interactAction.OnPerformedAction += ChestToggle;
        }
    }

    private void OnDestroy()
    {
        if (_interactAction != null)
        {
            _interactAction.OnPerformedAction -= ChestToggle;
        }
    }

    public void Interact()
    {
        if ((CGameManager.Instance != null && CGameManager.Instance.IsGameOver) || !isActiveAndEnabled || _data == null || _inventory == null)
        {
            return;
        }
        if(!_isOpend)
        {
            if (_inventory == null)
            {
                Debug.LogWarning("인벤토리 핸들러 누락");
                return;
            }

            _inventory.InitializeInventory(_data.InventorySize);

            IRewardable[] itemRewards = _inventory.InventorySize > 0 ?
                CLootTableManager.GetLootTableElements(_data.LootTableID, _inventory.InventorySize) : null;

            for (int i = 0; itemRewards != null && i < itemRewards.Length; i++)
            {
                if (!(itemRewards[i] is CItemReward itemReward))
                {
                    continue;
                }

                if(itemReward.Item == null)
                {
                    continue;
                }

                int minAmount = Mathf.Max(0, itemReward.MinAmount);
                int maxAmount = Mathf.Max(minAmount, itemReward.MaxAmount);
                maxAmount = Mathf.Min(maxAmount, Mathf.Max(0, itemReward.Item.MaxStack));
                minAmount = Mathf.Min(minAmount, maxAmount);
                int amount = Random.Range(minAmount, maxAmount + 1);
                if (amount <= 0)
                {
                    continue;
                }

                _inventory.AddToInventory(itemReward.Item, amount);
            }

            _isOpend = true;
        }

        _chestUI = CLootChestUIController.Instance;
        if (_chestUI == null)
        {
            return;
        }

        _chestUI.ToggleUI(true);
        _chestUI.UpdateSlot(_inventory);

    }

    public void CloseChest()
    {
        _enabled = false;
        if (_chestUI != null && _chestUI.IsOpenFor(_inventory))
        {
            _chestUI.ToggleUI(false);
        }
    }

    //for test=================

    //modified by June Gyu
    [ContextMenu("DebuggingTest(OpenChest)")]
    private void ChestOpenTest()
    {
        //_isOpend = false;
        Interact(); 
    }

    #region 내부 변수 
    private bool _isInteractable = false;
    private bool _enabled = false;
    private string _playerTag = "Player";
    private string _soundChest = "Chest";
    #endregion
     
    private void ChestToggle()
    {
        if (CGameManager.Instance != null && CGameManager.Instance.IsGameOver)
        {
            CloseChest();
            return;
        }

        if(!isActiveAndEnabled || !_isInteractable)
        {
            return;
        }

        _enabled = !CLootChestUIController.Instance.IsOpenFor(_inventory);

        CSoundControllerUI.PlaySound(_soundChest);
         
        if(_enabled)
        {
            ChestOpenTest();
        }
        else
        {
            CLootChestUIController.Instance.ToggleUI(false);
        }
         
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _playerTag)
        {
            _isInteractable = true;
            _InteractIcon.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == _playerTag)
        {
            int num = 0;
             
            num = collision.transform.position.y < _spriteRenderer.bounds.max.y ? 4 : 6;

            _InteractIcon.sortingOrder = _spriteRenderer.sortingOrder = num;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == _playerTag)
        {
            _isInteractable = false;
            _enabled = false;
            if (_chestUI != null)
            {
                _chestUI.ToggleUI(false);
            }

            _InteractIcon.gameObject.SetActive(false);
        }
    }
  

}
