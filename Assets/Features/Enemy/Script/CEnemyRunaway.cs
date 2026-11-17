using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI; 
public class CEnemyRunaway : CEnemyActionBase
{ 
    #region 내부변수
    string _Runaway = "IMove";
    #endregion
     
    public CEnemyRunaway(CEnemySetting enemySetting,int num) : base(enemySetting, num)
    {
            
    }
     
    public override bool Condition(Transform PlayerTr)
    {
        if(PlayerTr == null)
        {
            return false;
        }

        if((PlayerTr.position - _tr.position).sqrMagnitude < 2f)
        {
            return true;
        }

        return false;
    }
    public override void ActionEnter(Transform PlayerTr)
    {
        if(PlayerTr == null)
        {
            return;
        }
    
        _ani.SetInteger(_Runaway,1);
        _nav.stoppingDistance = 0;
        Debug.Log("지금 호출되나?");
    }
    public override void ActionUpdate(Transform PlayerTr)
    {
        Vector3 dir = (_tr.position - PlayerTr.position).normalized;

        _nav.destination = _tr.position + dir * _enemy.Enemydata.AttackRange;
    
        ReverseFlip(PlayerTr);
    }
    public override void ActionExit()
    {
        _nav.stoppingDistance = 2;
        _ani.SetInteger(_Runaway, 0);
         
    }
    protected void ReverseFlip(Transform PlayerTr)
    {
        _tr.rotation = (PlayerTr.position - _tr.position).x > 0 ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);
    }

    //private bool UpdateRun(Transform PlayerTr,float distance)
    //{ 
    //    Vector3 dir = (_transform.position - PlayerTr.position).normalized;
    //
    //    _navMove.destination = _transform.position + dir * _enemy.Enemydata.AttackRange;
    //
    //    ReverseFlip(PlayerTr);
    //
    //    if((PlayerTr.position - _transform.position).sqrMagnitude >= distance * distance)
    //    {
    //        return true;
    //    }
    //    Debug.Log("호출?");
    //    return false;
    //}
    //
    //public async UniTask Runaway(Transform PlayerTr, float distance,CancellationTokenSource source)
    //{
    //    if (PlayerTr == null)
    //    {
    //        return;
    //    }
    //
    //    source = new CancellationTokenSource();
    //
    //    _aniRunaway.SetInteger(_Runaway, 1);
    //    _navMove.stoppingDistance = 0;
    //
    //    try
    //    {
    //        await UniTask.WaitUntil(() => UpdateRun(PlayerTr, distance), PlayerLoopTiming.Update, source.Token);
    //    }
    //    finally
    //    {
    //        _isActing._isActing = false;
    //        _navMove.stoppingDistance = 2;
    //        _aniRunaway.SetInteger(_Runaway, 0);
    //
    //        source.Dispose();
    //        source = null;
    //    }
    //}
}
