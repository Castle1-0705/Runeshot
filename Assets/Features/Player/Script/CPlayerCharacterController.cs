using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CPlayerCharacterController : CSingleton<CPlayerCharacterController>
{
	#region 인스팩터
	[SerializeField] private CPlayerInteractionController _interactionController;
	[SerializeField] private CPlayerHPHandler _hpHandler;
	[SerializeField] private CPlayerStatusHandler _statusHandler;
	[SerializeField] private CStatusEffectHandler _statusEffectHandler;
    [SerializeField] private CModifierHandler _modifierHandler;
	[SerializeField] private CWeapon _weapon;
	#endregion

	#region 프로퍼티
	public CPlayerInteractionController InteractionController => _interactionController;
	public CPlayerHPHandler HPhandler => _hpHandler;
	public CPlayerStatusHandler Status => _statusHandler;
	public CStatusEffectHandler StatusEffectHandler => _statusEffectHandler;
    public CModifierHandler ModifierHandler => _modifierHandler;
	public CWeapon Weapon => _weapon;
	public CPlayerStateController StateController => GetComponent<CPlayerStateController>();
	#endregion

    protected override void Awake()
    {
        base.Awake();

        // 중복 Player는 기존 Singleton에서 제거하므로 유지하거나 이벤트를 등록하지 않음
        if (Instance != this)
        {
            return;
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected override void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        base.OnDestroy();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Single)
        {
            return;
        }

        CPlayerAim aim = GetComponent<CPlayerAim>();
        if (aim != null)
        {
            aim.RefreshCamera();
        }

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            CCameraFollow cameraFollow = mainCamera.GetComponent<CCameraFollow>();
            if (cameraFollow != null)
            {
                cameraFollow.SetTarget(transform);
            }
        }

        // 새로 로드된 씬 안에서만 검색. 스폰이 없으면 기존 위치 유지
        GameObject[] roots = scene.GetRootGameObjects();
        for (int i = 0; i < roots.Length; i++)
        {
            Transform spawn = FindPlayerSpawn(roots[i].transform);
            if (spawn != null)
            {
                MoveToSpawn(spawn);
                return;
            }
        }
    }

    private Transform FindPlayerSpawn(Transform target)
    {
        if (target.name == "PlayerSpawn")
        {
            return target;
        }

        for (int i = 0; i < target.childCount; i++)
        {
            Transform spawn = FindPlayerSpawn(target.GetChild(i));
            if (spawn != null)
            {
                return spawn;
            }
        }

        return null;
    }

    // 추후 Map에서 정한 스폰 Transform을 직접 전달할 수 있음
    public void MoveToSpawn(Transform spawn)
    {
        if (spawn == null)
        {
            return;
        }

        CPlayerMovement movement = GetComponent<CPlayerMovement>();
        if (movement != null)
        {
            movement.ResetMovement();
        }

        CPlayerStateController stateController = StateController;
        if (stateController != null &&
            (stateController.CurrentState == EPlayerState.Move ||
             stateController.CurrentState == EPlayerState.Roll))
        {
            // Dash 중단은 기존 Roll 코드가 감지하여 쿨타임 처리
            stateController.ChangeState(EPlayerState.Idle);
        }

        // Hit/Dead와 HP 코루틴은 유지하여 무적 시간이나 사망 상태를 초기화하지 않음
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        transform.position = spawn.position;
        if (rigidbody != null)
        {
            rigidbody.position = spawn.position;
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0.0f;
        }
    }
}
