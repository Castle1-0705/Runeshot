using UnityEngine;

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
