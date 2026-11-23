using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(20)]
public class CWeapon : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private CWeaponDataSO _data;
    [SerializeField] private CProjectilePool _projectilePool;
    [SerializeField] private Transform _gunHolder;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private string _triggerActionName = "TriggerWeapon";
    [SerializeField] private bool _usePlayerInput = true; 
    [SerializeField] private SpriteRenderer _weaponRenderer;
    #endregion

    #region 내부 변수
    private CPlayerWeaponInput _playerInput;
    private float _nextFireTime;
    private CPlayerStateController _playerState;
    private CPlayerStatusHandler _playerStatus;
    private CEnemyStatusHandler _enemyStatus;
    private readonly Dictionary<CWeaponDataSO, int> _magazines = new Dictionary<CWeaponDataSO, int>();
    private IitemContainer _ammoSource;
    private float _reloadEndTime;
    private int _weaponSortingOrder;
    private Vector3 _muzzleLocalPosition;
    #endregion

    #region 프로퍼티
    public CWeaponDataSO Data => _data;
    public bool IsAutomatic => _data != null && _data.Trigger != null && _data.Trigger.IsAutomatic;
    public bool IsReloading { get; private set; }

    public int MagazineAmmo
    {
        get
        {
            if (_data == null)
            {
                return 0;
            }

            if (_magazines.TryGetValue(_data, out int amount))
            {
                return amount;
            }

            return _data.MagazineSize;
        }
    }
    #endregion

    private void Awake()
    {
        _playerState = GetComponentInParent<CPlayerStateController>();
        _playerStatus = GetComponentInParent<CPlayerStatusHandler>();
        _enemyStatus = GetComponentInParent<CEnemyStatusHandler>();

        if (_muzzle != null)
        {
            _muzzleLocalPosition = _muzzle.localPosition;
        }

        if (_weaponRenderer != null)
        {
            _weaponSortingOrder = _weaponRenderer.sortingOrder;
        }
    }

    private void Start()
    {
        if (HasInvalidReferences(_data))
        {
            Debug.LogError("무기 데이터, 투사체 풀 또는 Muzzle이 비어있음");
            enabled = false;
            return;
        }

        if (_usePlayerInput)
        { 

            if (_gunHolder == null)
            {
                Debug.LogError("GunHolder가 비어있음");
                return;
            }

            _playerInput = GetComponent<CPlayerWeaponInput>();

            if (_playerInput == null)
            {
                _playerInput = gameObject.AddComponent<CPlayerWeaponInput>();
            }

            _playerInput.Initialize(this, _gunHolder, _triggerActionName);
        }

        UpdateWeaponVisual();
    }

    private void OnEnable()
    {
        UpdateWeaponVisual();

        if (_playerInput != null)
        {
            _playerInput.enabled = true;
        }
    }

    private void OnDisable()
    {
        IsReloading = false;

        if (_weaponRenderer != null)
        {
            _weaponRenderer.enabled = false;
        }

        if (_playerInput != null)
        {
            _playerInput.enabled = false;
        }
    }

    private void UpdateWeaponVisual()
    {
        if (_weaponRenderer == null || !_usePlayerInput || _enemyStatus != null)
        {
            return;
        }

        _weaponRenderer.sprite = _data != null ? _data.WeaponSprite : null;
        _weaponRenderer.enabled = isActiveAndEnabled && _weaponRenderer.sprite != null && !IsPlayerDead();
        if (_data != null)
        {
            _weaponRenderer.transform.localScale = Vector3.one * _data.WeaponSpriteScale;
            _weaponRenderer.flipX = _data.WeaponSpriteFlipX;
        }
        UpdateWeaponPose();
    }

    private void LateUpdate()
    {
        UpdateWeaponPose();
    }

    private void UpdateWeaponPose()
    {
        if (_weaponRenderer == null || _data == null || _gunHolder == null ||
            !_usePlayerInput || _enemyStatus != null)
        {
            return;
        }

        _weaponRenderer.enabled = _weaponRenderer.sprite != null && !IsPlayerDead();
        bool isLeft = _gunHolder.right.x < 0.0f;
        _weaponRenderer.flipY = isLeft;
        Sprite sprite = _weaponRenderer.sprite;
        Vector2 grip = _data.WeaponGripPixels;
        Vector2 muzzle = _data.WeaponMuzzlePixels;
        if (sprite != null && (muzzle - grip).sqrMagnitude > 0.0001f)
        {
            grip = (grip - sprite.pivot) / sprite.pixelsPerUnit;
            muzzle = (muzzle - sprite.pivot) / sprite.pixelsPerUnit;
            if (_data.WeaponSpriteFlipX)
            {
                grip.x = -grip.x;
                muzzle.x = -muzzle.x;
            }
            if (isLeft)
            {
                grip.y = -grip.y;
                muzzle.y = -muzzle.y;
            }

            float scale = _data.WeaponSpriteScale;
            _weaponRenderer.transform.localRotation = Quaternion.identity;
            _weaponRenderer.transform.localPosition = -grip * scale;
            if (_muzzle != null && _muzzle.parent == _gunHolder)
            {
                _muzzle.localPosition = (muzzle - grip) * scale;
            }
        }
        else
        {
            Vector2 offset = _data.WeaponSpriteOffset;
            if (isLeft)
            {
                offset.y = -offset.y;
            }
            _weaponRenderer.transform.localPosition = offset;
            _weaponRenderer.transform.localRotation = Quaternion.identity;
            if (_muzzle != null && _muzzle.parent == _gunHolder)
            {
                _muzzle.localPosition = _muzzleLocalPosition;
            }
        }
        _weaponRenderer.sortingOrder = _weaponSortingOrder;
    }

    private bool HasInvalidReferences(CWeaponDataSO data)
    {
        return data == null || data.Trigger == null || data.Projectile == null ||
            data.Projectile.Prefab == null || _projectilePool == null ||
            !_projectilePool.isActiveAndEnabled || _projectilePool.Prefab != data.Projectile.Prefab ||
            _muzzle == null;
    }

    public bool EquipWeapon(CWeaponDataSO data, bool fromPickup = false)
    {
        if (fromPickup && (!isActiveAndEnabled || IsPlayerDead()))
        {
            return false;
        }

        if (HasInvalidReferences(data))
        {
            Debug.LogError("무기 데이터, 투사체 풀 또는 Muzzle이 비어있음");
            return false;
        }

        if (_data == data && !fromPickup)
        {
            UpdateWeaponVisual();
            return true;
        }

        if (_playerState != null)
        {
            _magazines.Clear();
        }

        IsReloading = false;
        _data = data;
        UpdateWeaponVisual();

        if (_playerInput != null)
        {
            _playerInput.ClearPendingShots();
        }

        return true;
    }

    public bool TryFire(Vector2 direction)
    {
        if (!isActiveAndEnabled || HasInvalidReferences(_data) ||
            direction == Vector2.zero || IsReloading || (_enemyStatus == null && MagazineAmmo <= 0) || Time.time < _nextFireTime)
        {
            return false;
        }

        if (IsPlayerDead())
        {
            return false;
        }

        float damage = _data.Projectile.Damage;
        float fireInterval = _data.FireInterval;
        float spread = _data.SpreadAngle;

        if (_enemyStatus != null && _enemyStatus.isActiveAndEnabled)
        {
            damage *= _enemyStatus.WeaponDamage;
            fireInterval *= _enemyStatus.WeaponFireInterval;
            spread *= _enemyStatus.WeaponSpread;
        }
        else if (_playerStatus != null && _playerStatus.isActiveAndEnabled)
        {
            damage *= _playerStatus.WeaponDamage;
            fireInterval *= _playerStatus.WeaponFireInterval;
            spread *= _playerStatus.WeaponSpread;
        }

        if (spread > 0.0f)
        {
            float halfSpread = spread * 0.5f;
            float angle = Random.Range(-halfSpread, halfSpread);
            direction = Quaternion.Euler(0.0f, 0.0f, angle) * direction;
        }

        if (_enemyStatus == null)
        {
            _magazines[_data] = MagazineAmmo - 1;
        }
        _nextFireTime = Time.time + Mathf.Max(0.0f, fireInterval);
        Transform shooter = transform;
        if (_enemyStatus != null)
        {
            shooter = _enemyStatus.transform;
        }
        else if (_playerState != null)
        {
            shooter = _playerState.transform;
        }
        else if (_playerStatus != null)
        {
            shooter = _playerStatus.transform;
        }

        CSoundControllerSFXPlayer.PlaySound(Data.name);

        _data.Trigger.TriggerWeapon(_data.Projectile, _projectilePool, _muzzle.position, direction, damage, shooter).Forget();
        return true;
    }

    private bool IsPlayerDead()
    {
        return _playerState != null && _playerState.CurrentState == EPlayerState.Dead;
    }

    public void SetProjectilePool(CProjectilePool pool)
    {
        _projectilePool = pool;
    }

    public void SetAmmoSource(IitemContainer ammoSource)
    {
        IsReloading = false;
        _ammoSource = ammoSource;
    }

    public bool TryReload()
    {
        if (!isActiveAndEnabled || HasInvalidReferences(_data) || IsPlayerDead() ||
            _enemyStatus != null || IsReloading || MagazineAmmo >= _data.MagazineSize || _data.AmmoItem == null ||
            _data.AmmoItem.ItemType != EItemtype.Ammunition ||
            !HasAmmoSource() || !HasAmmo(_data.AmmoItem))
        {
            return false;
        }

        float reloadTime = _data.ReloadTime;
        if (_playerStatus != null && _playerStatus.isActiveAndEnabled)
        {
            reloadTime *= _playerStatus.ReloadSpeed;
        }

        _reloadEndTime = Time.time + Mathf.Max(0.01f, reloadTime);
        IsReloading = true;

        if (_playerInput != null)
        {
            _playerInput.ClearPendingShots();
        }
         
        CSoundControllerSFXPlayer.PlaySound("Reload");

        return true;
    }

    private void Update()
    {
        if (!IsReloading)
        {
            return;
        }

        if (IsPlayerDead() || HasInvalidReferences(_data) || !HasAmmoSource())
        {
            IsReloading = false;
            return;
        }

        if (Time.time < _reloadEndTime)
        {
            return;
        }

        CWeaponDataSO data = _data;
        int magazineAmmo = MagazineAmmo;
        int neededAmmo = data.MagazineSize - magazineAmmo;
        IsReloading = false;

        int loadedAmmo = TakeAmmo(data.AmmoItem, neededAmmo);
        _magazines[data] = magazineAmmo + loadedAmmo;
    }

    private bool HasAmmoSource()
    {
        return (UnityEngine.Object)_ammoSource != null;
    }

    private bool HasAmmo(CItemDataSO ammoItem)
    {
        IReadOnlyList<CItemSlotData> slots = _ammoSource.Inventory;
        for (int i = 0; i < slots.Count; i++)
        {
            CItemSlotData slot = slots[i];
            if (slot != null && slot.Item == ammoItem && slot.Amount > 0)
            {
                return true;
            }
        }

        return false;
    }

    private int TakeAmmo(CItemDataSO ammoItem, int neededAmmo)
    {
        IReadOnlyList<CItemSlotData> slots = _ammoSource.Inventory;
        int loadedAmmo = 0;
        for (int i = 0; i < slots.Count && loadedAmmo < neededAmmo; i++)
        {
            CItemSlotData slot = slots[i];
            if (slot == null || slot.Item != ammoItem || slot.Amount <= 0)
            {
                continue;
            }

            int before = slot.Amount;
            int amount = Mathf.Min(before, neededAmmo - loadedAmmo);
            _ammoSource.RemoveFromInventory(i, amount);
            CItemSlotData remaining = slots[i];
            int after = remaining != null && remaining.Item == ammoItem ? remaining.Amount : 0;
            loadedAmmo += Mathf.Clamp(before - after, 0, amount);
        }

        return loadedAmmo;
    }
}
