using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CEnemyHPHandler : CHPHandler<CEnemyStatusDataSO>
{
    #region 인스펙터
    [SerializeField] private CEnemy _enemy;
    [SerializeField] private Animator _aniHP;
    [SerializeField] private float _distance = 10f;
    #endregion

    #region 내부변수
    private string _death = "BDeath";
    private string _stringDamaged = "TDamaged";
    private string _stringDeath = "DEATH";
    private bool _dead = false;
    #endregion

    private void Awake()
    {
        if(_enemy == null || _aniHP == null || _status == null)
        {
            Debug.Log("인스펙터 확인");
        }
         
        HP = MaxHP;

        OnDeath += DropItem;
        OnDeath += SetFalse;

    }

    private void DropItem()
    {
       
    }
    private void OnEnable()
    {

        _enemy.DieOrAlive(true);

        //if (!_dead)
        //{
        //    return;
        //}
        //
        //Revive().Forget(); 
    }

    private async UniTask Revive()
    {
        _dead = true;

        await UniTask.WaitUntil(Distance, PlayerLoopTiming.Update, destroyCancellationToken);

        _enemy.DieOrAlive(true);

        _dead = false;
    }

    private bool Distance()
    {
        float distance = (_enemy.PlayerTr.position - transform.position).sqrMagnitude;

        if (distance > _distance)
        {
            return true;
        }

        return false;
    }
    private void OnDisable()
    {
        
    }
    private void SetFalse()
    {
        gameObject.SetActive(false);
        HP = MaxHP;
    }

    //TODO
    //invoke에 파티클, 사운드, 경험치, 아이템, destroy 구현 필요
    public override async UniTask OnHit(float damage)
    {
        if(damage <= 0 || HP <= 0)
        {
            return;
        }

        HP -= damage;
        OnHitInvoke();

        _aniHP.speed = 1f;
        _aniHP.SetTrigger(_stringDamaged);

        if (HP <= 0f)
        {
            _enemy.DieOrAlive(false);
            _aniHP.SetBool(_death,true); 

            await UniTask.WaitUntil(UntilDeath, PlayerLoopTiming.Update, destroyCancellationToken); 

            OnDeathInvoke();
        }
       
    }

    bool UntilDeath()
    {
        if (_aniHP.GetCurrentAnimatorStateInfo(0).IsName(_stringDeath))
        {
            if (_aniHP.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            { 
                return true;
            }
        } 
        return false;
    }
    public override UniTask OnHeal(float amount)
    { 
        HP += amount;
        return UniTask.CompletedTask;
    }
}
