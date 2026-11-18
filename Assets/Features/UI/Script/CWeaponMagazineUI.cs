using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CWeaponMagazineUI : MonoBehaviour
{ 
    [SerializeField] Image _magazine;
    [SerializeField] CWeapon _weapon;

    private void Start()
    { 
        UpdateAmmo();
    }
    // todo 무기 변경시,무기 재장전시,무기사용 시 업데이트
 
    public void UpdateAmmo()
    {
        if (_weapon.MagazineAmmo != 0)
        {
            // must change
            //_magazine.fillAmount = _weapon의 현재 총알갯수 / 탄창 용량
            _magazine.fillAmount = _weapon.MagazineAmmo;
        }
    }
}
