using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CItemWeaponSlotController : CItemSlotController
{

    #region 인스펙터
    [SerializeField] private Image _weaponImage;
    [SerializeField] private CWeapon _weapon;
    #endregion

    private void Start()
    {
        if (_weapon == null)
        {
            CPlayerCharacterController player = CPlayerCharacterController.Instance;
            if (player != null)
            {
                _weapon = player.Weapon;
            }
        }

        BindWeapon();
    }

    private void OnEnable()
    {
        BindWeapon();
    }

    private void OnDisable()
    {
        if (_weapon != null)
        {
            _weapon.OnWeaponChanged -= UpdateWeaponIcon;
        }
    }

    private void BindWeapon()
    {
        if (_weapon != null)
        {
            _weapon.OnWeaponChanged -= UpdateWeaponIcon;
            _weapon.OnWeaponChanged += UpdateWeaponIcon;
        }

        UpdateWeaponIcon(_weapon != null ? _weapon.Data : null);
    }

    private void UpdateWeaponIcon(CWeaponDataSO data)
    {
        if (_weaponImage == null)
        {
            return;
        }

        _weaponImage.sprite = data != null ? data.WeaponSprite : null;
        _weaponImage.enabled = _weaponImage.sprite != null;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData == null || eventData.pointerDrag == null || !gameObject.activeInHierarchy)
        {
            return;
        }

        GameObject dropedGO = eventData.pointerDrag;
        CDragableItemController item = dropedGO.GetComponent<CDragableItemController>();

        if (item == null || item.ItemData == null)
        {
            return;
        }

        if (item.Itemtype != EItemtype.Weapon) 
        {
            return;
        }

        item.TryEquipWeapon();
    }
}
