using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(10)]
public class CPlayerAim : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private Transform _gunHolder;
    [SerializeField] private Vector2 _aimOffset = new Vector2(0.2f, 0.16f);
    [SerializeField, Min(0.0f)] private float _maxAimOffset = 0.25f;
    [SerializeField] private Transform _aimArm;
    [SerializeField] private Vector2 _armGripOffset = new Vector2(-0.125f, -0.15625f);
    [SerializeField, Min(0.0f)] private float _maxArmReach = 0.4f;
    #endregion

    #region 내부 변수
    private Camera _mainCamera;
    private CWeapon _weapon;
    private CPlayerStateController _stateController;
    private Vector3 _aimBasePosition;
    private Vector3 _armBaseScale;
    private Vector3 _armLocalPosition;
    private Quaternion _armLocalRotation;
    private Vector3 _armLocalScale;
    private bool _wasAimingArm;
    #endregion

    private void Awake()
    {
        RefreshCamera();

        if (_gunHolder == null)
        {
            Debug.LogWarning("GunHolder가 비어있음");
            enabled = false;
            return;
        }

        _aimBasePosition = _gunHolder.localPosition;
        _stateController = GetComponent<CPlayerStateController>();
        CPlayerCharacterController player = GetComponent<CPlayerCharacterController>();
        if (player != null)
        {
            _weapon = player.Weapon;
        }
        if (_aimArm != null)
        {
            _armBaseScale = _aimArm.localScale;
        }
    }

    private void OnDisable()
    {
        RestoreArmPose();
    }

    // 새 씬 카메라로 갱신. 카메라가 없는 동안에는 조준을 대기
    public void RefreshCamera()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        RestoreArmPose();
        Aim();
    }

    private void LateUpdate()
    {
        Aim();
        UpdateArmPose();
    }

    // 마우스 방향 조준
    private void Aim()
    {
        if (CGameManager.Instance != null && !CGameManager.Instance.IsPlaying)
        {
            return;
        }

        if (_mainCamera == null)
        {
            RefreshCamera();
        }

        if (_mainCamera == null || _gunHolder == null || Mouse.current == null)
        {
            return;
        }
        
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector2 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);

        Vector3 basePosition = transform.TransformPoint(_aimBasePosition);
        Vector2 direction = mouseWorldPosition - (Vector2)basePosition;

        if (direction == Vector2.zero)
        {
            return;
        }

        direction.Normalize();
        Vector3 localDirection = transform.InverseTransformVector(direction);
        Vector3 offset = new Vector3(localDirection.x * Mathf.Max(0.0f, _aimOffset.x),
            localDirection.y * Mathf.Max(0.0f, _aimOffset.y), 0.0f);
        offset = Vector3.ClampMagnitude(offset, Mathf.Max(0.0f, _maxAimOffset));
        _gunHolder.localPosition = _aimBasePosition + offset;

        direction = mouseWorldPosition - (Vector2)_gunHolder.position;
        if (direction == Vector2.zero)
        {
            return;
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _gunHolder.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    private void UpdateArmPose()
    {
        if (CGameManager.Instance != null && !CGameManager.Instance.IsPlaying)
        {
            return;
        }

        if (_aimArm == null || _aimArm.parent == null || _weapon == null ||
            !_weapon.isActiveAndEnabled || _weapon.Data == null || _weapon.Data.WeaponSprite == null ||
            _stateController == null || _stateController.CurrentState == EPlayerState.Dead ||
            _armGripOffset.sqrMagnitude < 0.0001f)
        {
            return;
        }
        if (Vector2.Distance(_aimArm.parent.position, _gunHolder.position) > _maxArmReach)
        {
            return;
        }

        // Animator가 만든 자세는 다음 프레임 전에 복원
        _armLocalPosition = _aimArm.localPosition;
        _armLocalRotation = _aimArm.localRotation;
        _armLocalScale = _aimArm.localScale;
        _wasAimingArm = true;

        Vector3 direction = _aimArm.parent.InverseTransformVector(_gunHolder.right);
        Vector3 grip = Vector3.Scale(_armGripOffset, _armBaseScale);
        float angle = Mathf.Atan2(direction.y, direction.x) - Mathf.Atan2(grip.y, grip.x);
        _aimArm.localScale = _armBaseScale;
        _aimArm.localRotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg);
        _aimArm.position += _gunHolder.position - _aimArm.TransformPoint(_armGripOffset);
    }

    private void RestoreArmPose()
    {
        if (!_wasAimingArm || _aimArm == null)
        {
            return;
        }
        _aimArm.localPosition = _armLocalPosition;
        _aimArm.localRotation = _armLocalRotation;
        _aimArm.localScale = _armLocalScale;
        _wasAimingArm = false;
    }
}