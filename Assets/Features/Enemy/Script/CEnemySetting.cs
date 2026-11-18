using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;

public class CEnemySetting : MonoBehaviour
{
    [SerializeField] public CEnemy _enemy;
    [SerializeField] public NavMeshAgent _navAgent;
    [SerializeField] public Animator _ani;
    [SerializeField] public Collider2D _col;
    [SerializeField] public CEnemyController _controll; 

    [SerializeField] public SpriteRenderer _meleeAttack;
    [SerializeField] public SpriteRenderer _rangeAttack;
    [SerializeField] public CWeapon _weapon;

    public CancellationTokenSource _source;
     
    public CancellationTokenSource TokenGen()
    {
        _source = new CancellationTokenSource();

        return _source;
    }
     
    public void TokenCancel()
    {
        if (_source != null)
        {
            _source.Cancel();
            _source.Dispose();
            _source = null;
        }
    }
     
    private void OnDestroy()
    {
        TokenCancel();
    }
}
