using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CWeaponMagazineUI : MonoBehaviour
{

    #region 인스펙터
    [SerializeField] private Slider _magazineSlider;
    [SerializeField] private CWeapon _weapon;
    #endregion

    #region 내부 변수
    private float _time;
    #endregion 

    //for test
    #region for test
    private Image _magazineUI;
    private Image _icon;
    private Image _reload;
    private List<Image> _images = new List<Image>();
    #endregion
     
    private void Start()
    { 
        if(_magazineSlider == null || _weapon == null)
        {
            Debug.Log("탄창 인스펙터 확인");
            return;
        }

        //_weapon.reloading += RunReloading;
        //_weapon.fire += UpdateAmmo2;

        //UpdateAmmo();
    }

    private void Update()
    {
        if(!_weapon.IsReloading)
        {
            UpdateAmmo();
        }
        else
        {
            ReloadAmmo();
        }
        
    }

    //임시
    public void UpdateAmmo()
    {
        if (_weapon.IsReloading)
        {
            return;
        }
         
        if (_weapon.Data.MagazineSize != 0)
        {
            _time = 0f;
            _magazineSlider.value = (float)_weapon.MagazineAmmo / _weapon.Data.MagazineSize;
        }
    }
     
    public void ReloadAmmo()
    {

        if(_weapon.Data.MagazineSize != 0)
        {
            _time += Time.deltaTime;
            _magazineSlider.value = _time / _weapon.Data.ReloadTime;
        }
        else if(_time >= _weapon.Data.MagazineSize)
        {
            _reload.fillAmount = 1f;
            
        }
        
    }

    //test2

    public void RunReloading()
    {
        Reloading().Forget();
    }
    private async UniTask Reloading()
    {
        await UniTask.WaitUntil(Reload,PlayerLoopTiming.Update,destroyCancellationToken);
    }

    private bool Reload()
    {
        if (_weapon.Data.ReloadTime != 0)
        {
            _time += Time.deltaTime;
            _magazineSlider.value = _time / _weapon.Data.ReloadTime;
        }
        
        
        if (_time >= _weapon.Data.ReloadTime)
        {
            _magazineSlider.value = 1f;
            _time = 0f;

            return true; 
        }

        return false;
    }

    public void UpdateAmmo2()
    {
        if (_weapon.IsReloading)
        {
            return;
        }

        if (_time != 0)
        {
            return;
        }

        if (_weapon.Data.MagazineSize != 0)
        {
            _time = 0f;
            _magazineSlider.value = (float)_weapon.MagazineAmmo / _weapon.Data.MagazineSize;
        }
    }

    //test
    public void MakeAmmo()
    {
        _images.Clear();

        float sizeX = _magazineUI.rectTransform.sizeDelta.x / _weapon.Data.MagazineSize;

        _icon.rectTransform.sizeDelta = new Vector2(sizeX, _magazineUI.rectTransform.rect.height);

        for (int i = 0; i < _weapon.Data.MagazineSize; i++)
        {
            Vector2 location = new Vector2
                (
                _magazineUI.rectTransform.position.x
                - _magazineUI.rectTransform.rect.width * 0.5f
                + sizeX * i + _icon.rectTransform.rect.width * 0.5f,
                _magazineUI.transform.position.y
                );

            Image image = Instantiate(_icon, location, _magazineUI.transform.localRotation, _magazineUI.rectTransform);

            _images.Add(image);

        }
    }

    public void DestroyAmmo()
    {
        Destroy(_images[_weapon.MagazineAmmo - 1].gameObject);
        //_images.RemoveAt(_weapon.MagazineAmmo-1);
    }
}




