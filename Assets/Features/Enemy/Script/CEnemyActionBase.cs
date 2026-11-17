using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI; 

public class CEnemyActionBase
{
    public string _id;
    public int _num;

    protected NavMeshAgent _nav;
    protected Animator _ani;
    protected Collider2D _col;
    protected Transform _tr;
    protected CEnemy _enemy;
    protected CEnemyController _controll; 

    public CEnemyActionBase(CEnemySetting enemySetting,int num)
    {
        _nav = enemySetting._navAgent;
        _ani = enemySetting._ani;
        _col = enemySetting._col;
        _tr = enemySetting.transform;
        _enemy = enemySetting._enemy;
        _controll = enemySetting._controll;
        _num = num;
    }
     
    public virtual bool Condition(Transform PlayerTr)
    {
        return false;
    }
     
    public virtual void ActionEnter(Transform PlayerTr)
    {

    }
     
    public virtual void ActionUpdate(Transform PlayerTr)
    {

    }
    public virtual void ActionExit()
    {

    }

    public virtual void OnDisable()
    {

    }
    protected void Flip(Vector3 position)
    {
        _tr.rotation = (position - _tr.position).x <= 0 ? Quaternion.identity : _tr.rotation = Quaternion.Euler(0, 180f, 0);
    }
    protected void TokenCancel(CancellationTokenSource tokenSource)
    {
        if (tokenSource != null)
        {   
            tokenSource.Cancel();
            tokenSource.Dispose();
            tokenSource = null;
        }
    }
}
