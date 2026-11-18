using UnityEngine;

public class CWeaponPickup : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private CWeaponDataSO _weaponData;
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        CPlayerCharacterController player = other.GetComponentInParent<CPlayerCharacterController>();
        if (player == null || player.Weapon == null || !gameObject.activeInHierarchy)
        {
            return;
        }

        if (player.Weapon.EquipWeapon(_weaponData, true))
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
