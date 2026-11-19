using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI; 

public class CEnemyReturn : CEnemyActionBase
{
     
    #region 내부변수
    private CEnemySetting _enemySetting;

    private float _maxReturnTime = 3f;
    private float _time;
    private Vector3 _setPosition; 
    private string _move = "IMove"; 
    private float _navMeshRadius = 5f;
    #endregion

    public CEnemyReturn(CEnemySetting enemySetting,int num) : base(enemySetting, num)
    {
        _setPosition = _tr.position;
        _enemySetting = enemySetting;
    }
      
    public override bool Condition(Transform PlayerTr)
    {
        if (PlayerTr == null)
        {
            float distance = (_setPosition - _tr.position).sqrMagnitude;

            if (distance > 0.1f)
            { 
                return true;
            }

        }

        return false;
    }
    public override void ActionEnter(Transform PlayerTr)
    {   
        if(_enemySetting._source == null)
        {
            _nav.stoppingDistance = 0f;
            ReturnPosition(_enemySetting.TokenGen()).Forget(); 
        }
        else
        {
            return;
        }    
         
    }
     
    public override void ActionExit()
    {  
        _enemySetting.TokenCancel(); 
        
    }
    async UniTask ReturnPosition(CancellationTokenSource tokenSource)
    { 
        _ani.SetInteger(_move, 1);
          
        await UniTask.WaitUntil(IsDestinationArrive, PlayerLoopTiming.Update, tokenSource.Token);

        float _time = UnityEngine.Random.Range(0f, _maxReturnTime);

        await UniTask.Delay(TimeSpan.FromSeconds(_maxReturnTime), false, PlayerLoopTiming.Update, tokenSource.Token);
         
        _nav.velocity = Vector3.zero;

        NavMeshHit hit;

        if(NavMesh.SamplePosition(_setPosition, out hit, _navMeshRadius, _nav.areaMask))
        {
            _nav.destination = hit.position;
        } 

        _ani.SetInteger(_move, 1);

        Flip(_setPosition);
  
    }

    private bool IsDestinationArrive()
    {
        bool isArrive = false;
         
        
        if ((_nav.destination - _tr.position).magnitude <= 0.1f)
        {
            isArrive = true;
            _time = 0;
            _ani.SetInteger(_move, 0);
        }
        else if ((_nav.destination - _tr.position).magnitude < _col.bounds.size.x)
        {
            _time += Time.deltaTime;
        }

        if (_time > _maxReturnTime)
        {
            _time = 0;
            _ani.SetInteger(_move, 0);
            return true;
        }

        return isArrive;
    }
     
}
